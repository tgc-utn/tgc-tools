using TGC.Core.BoundingVolumes;
using TGC.Core.Geometry;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Textures;
using TGC.Tools.Model;
using TGC.Tools.UserControls;

namespace TGC.Tools.MeshCreator.Primitives
{
    /// <summary>
    ///     Primitiva de plano en el eje XY
    /// </summary>
    public class TGCPlaneXYPrimitive : EditorPrimitive
    {
        private float creatingInitMouseY;
        private TGCVector3 initSelectionPoint;
        private TgcPlane mesh;
        private TGCVector3 originalSize;

        public TGCPlaneXYPrimitive(MeshCreatorModifier control)
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
            creatingInitMouseY = ToolsModel.Instance.Input.Ypos;

            //Crear plano inicial
            var TGCPlaneTexture = TgcTexture.createTexture(Control.getCreationTexturePath());
            mesh = new TgcPlane(initSelectionPoint, new TGCVector3(0, 0, 0), TgcPlane.Orientations.XYplane,
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
            var input = ToolsModel.Instance.Input;

            //Si hacen clic con el mouse, ver si hay colision con el suelo
            if (input.buttonDown(TgcD3dInput.MouseButtons.BUTTON_LEFT))
            {
                //Obtener altura en Y segun movimient en Y del mouse
                var heightY = creatingInitMouseY - input.Ypos;
                var adjustedHeightY = MeshCreatorUtils.getMouseIncrementHeightSpeed(Control.Camera, BoundingBox, heightY);

                //Determinar posicion X segun la colision con el grid
                var collisionPoint = Control.Grid.getPicking();
                var extensionPoint = new TGCVector3(collisionPoint.X, initSelectionPoint.Y + adjustedHeightY,
                    initSelectionPoint.Z);

                //Obtener maximo y minimo
                var min = TGCVector3.Minimize(initSelectionPoint, extensionPoint);
                var max = TGCVector3.Maximize(initSelectionPoint, extensionPoint);
                min.Z = initSelectionPoint.Z;
                max.Z = initSelectionPoint.Z + 1;

                //Configurar plano
                mesh.setExtremes(min, max);
                mesh.updateValues();
            }
            //Solto el clic del mouse, generar plano definitivo
            else if (input.buttonUp(TgcD3dInput.MouseButtons.BUTTON_LEFT))
            {
                //Tiene el tamaño minimo tolerado
                var size = mesh.BoundingBox.calculateSize();
                if (size.X > 1 && size.Y > 1)
                {
                    //Guardar size original del plano para hacer Scaling
                    originalSize = mesh.Size;

                    //Dejar cargado para que se pueda crear un nuevo plano
                    Control.CurrentState = MeshCreatorModifier.State.CreatePrimitiveSelected;
                    Control.CreatingPrimitive = new TGCPlaneXYPrimitive(Control);

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
            var p = new TGCPlaneXYPrimitive(Control);
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