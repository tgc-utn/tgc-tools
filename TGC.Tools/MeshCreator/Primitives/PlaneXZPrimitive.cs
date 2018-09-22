using TGC.Core.BoundingVolumes;
using TGC.Core.Geometry;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Textures;
using TGC.Tools.UserControls;

namespace TGC.Tools.MeshCreator.Primitives
{
    /// <summary>
    ///     Primitiva de plano en el eje XZ
    /// </summary>
    public class TGCPlaneXZPrimitive : EditorPrimitive
    {
        private TGCVector3 initSelectionPoint;
        private TgcPlane mesh;
        private TGCVector3 originalSize;

        public TGCPlaneXZPrimitive(MeshCreatorModifier control)
            : base(control)
        {
            Name = "TGCPlane_" + PRIMITIVE_COUNT++;
            ModifyCaps.ChangeRotation = false;
        }

        public override TgcBoundingAxisAlignBox BoundingBox
        {
            get { return mesh.BoundingBox; }
        }

        public override bool AlphaBlendEnable
        {
            get { return mesh.AlphaBlendEnable; }
            set { mesh.AlphaBlendEnable = value; }
        }

        public override TGCVector2 TextureOffset
        {
            get { return mesh.UVOffset; }
            set
            {
                mesh.UVOffset = value;
                mesh.updateValues();
            }
        }

        public override TGCVector2 TextureTiling
        {
            get { return new TGCVector2(mesh.UTile, mesh.VTile); }
            set
            {
                mesh.UTile = value.X;
                mesh.VTile = value.Y;
                mesh.updateValues();
            }
        }

        public override TGCVector3 Position
        {
            get { return mesh.Origin; }
            set
            {
                mesh.Origin = value;
                mesh.updateValues();
            }
        }

        public override TGCVector3 Rotation
        {
            get { return TGCVector3.Empty; }
        }

        public override TGCVector3 Scale
        {
            get { return TGCVector3.Div(mesh.Size, originalSize); }
            set
            {
                var newSize = TGCVector3.Mul(originalSize, value);
                mesh.Size = newSize;
                mesh.updateValues();
            }
        }

        public override void render()
        {
            mesh.Render();
        }

        public override void dispose()
        {
            mesh.Dispose();
        }

        public override void setSelected(bool selected)
        {
            this.selected = selected;
            var color = selected ? MeshCreatorUtils.SELECTED_OBJECT_COLOR : MeshCreatorUtils.UNSELECTED_OBJECT_COLOR;
            mesh.BoundingBox.setRenderColor(color);
        }

        /// <summary>
        ///     Iniciar la creacion
        /// </summary>
        public override void initCreation(TGCVector3 gridPoint)
        {
            initSelectionPoint = gridPoint;

            //Crear plano inicial
            var TGCPlaneTexture = TgcTexture.createTexture(Control.getCreationTexturePath());
            mesh = new TgcPlane(initSelectionPoint, new TGCVector3(0, 0, 0), TgcPlane.Orientations.XZplane,
                TGCPlaneTexture);
            mesh.AutoAdjustUv = false;
            mesh.UTile = 1;
            mesh.VTile = 1;
            mesh.BoundingBox.setRenderColor(MeshCreatorUtils.UNSELECTED_OBJECT_COLOR);
            Layer = Control.CurrentLayer;
        }

        /// <summary>
        ///     Construir plano
        /// </summary>
        public override void doCreation()
        {
            var input = Control.creator.Input;

            //Si hacen clic con el mouse, ver si hay colision con el suelo
            if (input.buttonDown(TgcD3dInput.MouseButtons.BUTTON_LEFT))
            {
                //Determinar el size en XZ del box
                var collisionPoint = Control.Grid.getPicking();

                //Obtener extremos del rectángulo de selección
                var min = TGCVector3.Minimize(initSelectionPoint, collisionPoint);
                var max = TGCVector3.Maximize(initSelectionPoint, collisionPoint);
                min.Y = 0;
                max.Y = 1;

                //Configurar plano
                mesh.setExtremes(min, max);
                mesh.updateValues();
            }
            //Solto el clic del mouse, generar plano definitivo
            else if (input.buttonUp(TgcD3dInput.MouseButtons.BUTTON_LEFT))
            {
                //Tiene el tamaño minimo tolerado
                var size = mesh.BoundingBox.calculateSize();
                if (size.X > 1 && size.Z > 1)
                {
                    //Guardar size original del plano para hacer Scaling
                    originalSize = mesh.Size;

                    //Dejar cargado para que se pueda crear un nuevo plano
                    Control.CurrentState = MeshCreatorModifier.State.CreatePrimitiveSelected;
                    Control.CreatingPrimitive = new TGCPlaneXZPrimitive(Control);

                    //Agregar plano a la lista de modelos
                    Control.addMesh(this);

                    //Seleccionar plano
                    Control.SelectionRectangle.clearSelection();
                    Control.SelectionRectangle.selectObject(this);
                    Control.updateModifyPanel();
                }
                //Sino, descartar
                else
                {
                    Control.CurrentState = MeshCreatorModifier.State.CreatePrimitiveSelected;
                    mesh.Dispose();
                    mesh = null;
                }
            }
        }

        public override void move(TGCVector3 move)
        {
            mesh.Origin += move;
            mesh.updateValues();
        }

        public override void setTexture(TgcTexture texture, int slot)
        {
            mesh.setTexture(texture);
        }

        public override TgcTexture getTexture(int slot)
        {
            return mesh.Texture;
        }

        public override void setRotationFromPivot(TGCVector3 rotation, TGCVector3 pivot)
        {
            //NO SOPORTADO ACTUALMENTE
        }

        public override TgcMesh createMeshToExport()
        {
            var m = mesh.toMesh(Name);
            m.UserProperties = UserProperties;
            m.Layer = Layer;
            return m;
        }

        public override EditorPrimitive clone()
        {
            var p = new TGCPlaneXZPrimitive(Control);
            p.mesh = mesh.clone();
            p.originalSize = originalSize;
            p.UserProperties = UserProperties;
            p.Layer = Layer;
            return p;
        }

        public override void updateBoundingBox()
        {
            var m = mesh.toMesh(Name);
            mesh.BoundingBox.setExtremes(m.BoundingBox.PMin, m.BoundingBox.PMax);
            m.Dispose();
        }
    }
}