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
    ///     Primitiva de Sphere 3D
    /// </summary>
    public class SpherePrimitive : EditorPrimitive
    {
        private readonly TgcBoundingAxisAlignBox bb;
        private TGCVector3 initSelectionPoint;
        private TGCSphere mesh;
        private float originalRadius;
        private float scale = 1;

        public SpherePrimitive(MeshCreatorModifier control)
            : base(control)
        {
            bb = new TgcBoundingAxisAlignBox();
            Name = "Sphere_" + PRIMITIVE_COUNT++;
        }

        /*public override TgcBoundingSphere BoundingSphere
        {
            get { return mesh.BoundingSphere; }
        }*/

        public override TgcBoundingAxisAlignBox BoundingBox
        {
            get { return bb; }
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
            get { return mesh.UVTiling; }
            set
            {
                mesh.UVTiling = value;
                mesh.updateValues();
            }
        }

        public override TGCVector3 Position
        {
            get { return mesh.Position; }
            set
            {
                mesh.Position = value;
                updateBB();
            }
        }

        public override TGCVector3 Rotation
        {
            get { return mesh.Rotation; }
        }

        /// <summary>
        ///     Configurar tamaño del sphere
        /// </summary>
        public override TGCVector3 Scale
        {
            get { return new TGCVector3(scale, scale, scale); }
            set
            {
                if (scale != value.X)
                {
                    scale = value.X;
                }
                else if (scale != value.Y)
                {
                    scale = value.Y;
                }
                else if (scale != value.Z)
                {
                    scale = value.Z;
                }

                mesh.Radius = originalRadius * scale;

                mesh.updateValues();
                updateBB();
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
            // mesh.BoundingSphere.setRenderColor(color);
            bb.setRenderColor(color);
        }

        /// <summary>
        ///     Iniciar la creacion
        /// </summary>
        public override void initCreation(TGCVector3 gridPoint)
        {
            initSelectionPoint = gridPoint;

            //Crear caja inicial
            var sphereTexture = TgcTexture.createTexture(Control.getCreationTexturePath());
            mesh = new TGCSphere();

            mesh.setTexture(sphereTexture);
            // mesh.BoundingSphere.setRenderColor(MeshCreatorUtils.UNSELECTED_OBJECT_COLOR);
            bb.setRenderColor(MeshCreatorUtils.UNSELECTED_OBJECT_COLOR);
            Layer = Control.CurrentLayer;
        }

        /// <summary>
        ///     Construir caja
        /// </summary>
        public override void doCreation()
        {
            var input = ToolsModel.Instance.Input;

            //Si hacen clic con el mouse, ver si hay colision con el suelo
            if (input.buttonDown(TgcD3dInput.MouseButtons.BUTTON_LEFT))
            {
                //Determinar el size en XZ del box
                var collisionPoint = Control.Grid.getPicking();

                mesh.Position = initSelectionPoint;
                //Configurar BOX
                mesh.Radius = (collisionPoint - initSelectionPoint).Length();
                mesh.updateValues();
            }
            else
            {
                originalRadius = mesh.Radius;
                updateBB();
                //Dejar cargado para que se pueda crear un nuevo sphere
                Control.CurrentState = MeshCreatorModifier.State.CreatePrimitiveSelected;
                Control.CreatingPrimitive = new SpherePrimitive(Control);

                //Agregar sphere a la lista de modelos
                Control.addMesh(this);

                //Seleccionar Box
                Control.SelectionRectangle.clearSelection();
                Control.SelectionRectangle.selectObject(this);
                Control.updateModifyPanel();
            }
        }

        public override void move(TGCVector3 move)
        {
            mesh.Move(move);
            updateBB();
        }

        private void updateBB()
        {
            var r = new TGCVector3(mesh.Radius, mesh.Radius, mesh.Radius);
            bb.setExtremes(TGCVector3.Subtract(mesh.Position, r), TGCVector3.Add(mesh.Position, r));
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
            mesh.Rotation = rotation;
            var translation = pivot - mesh.Position;
            var m = TGCMatrix.Translation(-translation) * TGCMatrix.RotationYawPitchRoll(rotation.Y, rotation.X, rotation.Z) *
                    TGCMatrix.Translation(translation);
            mesh.Move(new TGCVector3(m.M41, m.M42, m.M43));
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
            var p = new SpherePrimitive(Control);
            p.mesh = mesh.clone();
            p.originalRadius = originalRadius;
            p.Scale = Scale;
            p.UserProperties = UserProperties;
            p.Layer = Layer;
            return p;
        }

        public override void updateBoundingBox()
        {
            var m = mesh.toMesh(Name);
            bb.setExtremes(m.BoundingBox.PMin, m.BoundingBox.PMax);
            m.Dispose();
        }
    }
}