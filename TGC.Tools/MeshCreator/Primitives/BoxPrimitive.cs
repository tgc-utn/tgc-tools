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
    /// Primitiva de Box 3D
    /// </summary>
    public class BoxPrimitive : EditorPrimitive
    {
        private float creatingBoxInitMouseY;
        private CreatingBoxState currentCreatingState;
        private TGCVector3 initSelectionPoint;

        private TGCBox mesh;
        private TGCVector3 originalSize;

        public BoxPrimitive(MeshCreatorModifier control) : base(control)
        {
            Name = "Box_" + PRIMITIVE_COUNT++;
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
            set { mesh.Position = value; }
        }

        public override TGCVector3 Rotation
        {
            get { return mesh.Rotation; }
        }

        /// <summary>
        /// Configurar tamaño del box
        /// </summary>
        public override TGCVector3 Scale
        {
            get
            {
                var size = mesh.BoundingBox.calculateSize();
                return TGCVector3.Div(size, originalSize);
            }
            set
            {
                var newSize = TGCVector3.Mul(originalSize, value);
                mesh.setPositionSize(mesh.Position, newSize);
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
        /// Iniciar la creacion
        /// </summary>
        public override void initCreation(TGCVector3 gridPoint)
        {
            initSelectionPoint = gridPoint;
            currentCreatingState = CreatingBoxState.DraggingSize;

            //Crear caja inicial
            var boxTexture = TgcTexture.createTexture(Control.CreationTexturePath());
            mesh = TGCBox.fromExtremes(initSelectionPoint, initSelectionPoint, boxTexture);
            mesh.BoundingBox.setRenderColor(MeshCreatorUtils.UNSELECTED_OBJECT_COLOR);
            mesh.AutoTransformEnable = true;
            Layer = Control.CurrentLayer;
        }

        /// <summary>
        /// Construir caja
        /// </summary>
        public override void doCreation()
        {
            var input = Control.creator.Input;

            switch (currentCreatingState)
            {
                case CreatingBoxState.DraggingSize:

                    //Si hacen clic con el mouse, ver si hay colision con el suelo
                    if (input.buttonDown(TgcD3dInput.MouseButtons.BUTTON_LEFT))
                    {
                        //Determinar el size en XZ del box
                        var collisionPoint = Control.Grid.getPicking();

                        //Obtener extremos del rectángulo de selección
                        var min = TGCVector3.Minimize(initSelectionPoint, collisionPoint);
                        var max = TGCVector3.Maximize(initSelectionPoint, collisionPoint);
                        min.Y = initSelectionPoint.Y;
                        max.Y = initSelectionPoint.Y + 0.2f;

                        //Configurar BOX
                        mesh.setExtremes(min, max);
                        mesh.updateValues();
                    }
                    //Solto el clic del mouse, pasar a configurar el Height del box
                    else if (input.buttonUp(TgcD3dInput.MouseButtons.BUTTON_LEFT))
                    {
                        //Tiene el tamaño minimo tolerado
                        var size = mesh.BoundingBox.calculateSize();
                        if (size.X > 1 && size.Z > 1)
                        {
                            currentCreatingState = CreatingBoxState.DraggingHeight;
                            creatingBoxInitMouseY = input.Ypos;
                        }
                        //Sino, descartar
                        else
                        {
                            Control.CurrentState = MeshCreatorModifier.State.CreatePrimitiveSelected;
                            mesh.Dispose();
                            mesh = null;
                        }
                    }

                    break;

                case CreatingBoxState.DraggingHeight:

                    //Si presiona clic, terminar de configurar la altura y generar box definitivo
                    if (input.buttonPressed(TgcD3dInput.MouseButtons.BUTTON_LEFT))
                    {
                        //Guardar size original del Box para hacer Scaling
                        originalSize = mesh.BoundingBox.calculateSize();

                        //Dejar cargado para que se pueda crear un nuevo box
                        Control.CurrentState = MeshCreatorModifier.State.CreatePrimitiveSelected;
                        Control.CreatingPrimitive = new BoxPrimitive(Control);

                        //Agregar box a la lista de modelos
                        Control.AddMesh(this);

                        //Seleccionar Box
                        Control.SelectionRectangle.clearSelection();
                        Control.SelectionRectangle.selectObject(this);
                        Control.UpdateModifyPanel();
                    }
                    //Determinar altura en base a la posicion Y del mouse
                    else
                    {
                        var heightY = creatingBoxInitMouseY - input.Ypos;
                        var adjustedHeightY = MeshCreatorUtils.getMouseIncrementHeightSpeed(Control.Camera, BoundingBox,
                            heightY);

                        var min = mesh.BoundingBox.PMin;
                        min.Y = initSelectionPoint.Y;
                        var max = mesh.BoundingBox.PMax;
                        max.Y = initSelectionPoint.Y + adjustedHeightY;

                        //Configurar BOX
                        mesh.setExtremes(min, max);
                        mesh.updateValues();
                    }

                    break;
            }
        }

        public override void move(TGCVector3 move)
        {
            mesh.Move(move);
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
            var m = mesh.ToMesh(Name);
            m.UserProperties = UserProperties;
            m.Layer = Layer;
            m.AutoTransformEnable = true;
            return m;
        }

        public override EditorPrimitive clone()
        {
            var p = new BoxPrimitive(Control);
            p.mesh = mesh.clone();
            p.mesh.AutoTransformEnable = true;
            p.originalSize = originalSize;
            p.UserProperties = UserProperties;
            p.Layer = Layer;
            return p;
        }

        public override void updateBoundingBox()
        {
            var m = mesh.ToMesh(Name);
            mesh.BoundingBox.setExtremes(m.BoundingBox.PMin, m.BoundingBox.PMax);
            m.Dispose();
        }

        /// <summary>
        /// Estado cuando se esta creando un Box
        /// </summary>
        private enum CreatingBoxState
        {
            DraggingSize,
            DraggingHeight
        }
    }
}