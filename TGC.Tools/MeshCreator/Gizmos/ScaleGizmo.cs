using Microsoft.DirectX.DirectInput;
using System.Drawing;
using TGC.Core.Collision;
using TGC.Core.Direct3D;
using TGC.Core.Geometry;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Tools.MeshCreator.Primitives;
using TGC.Tools.UserControls;

namespace TGC.Tools.MeshCreator.Gizmos
{
    /// <summary>
    /// Gizmo para escalar objetos
    /// </summary>
    public class ScaleGizmo : EditorGizmo
    {
        private const float LARGE_AXIS_SIZE = 200f;
        private const float SHORT_AXIS_SIZE = 5f;
        private readonly TGCBox boxX;
        private readonly TGCBox boxY;
        private readonly TGCBox boxZ;

        private State currentState;
        private TGCVector3 gizmoCenter;
        private TGCVector2 initMouseP;
        private Axis selectedAxis;

        public ScaleGizmo(MeshCreatorModifier control) : base(control)
        {
            selectedAxis = Axis.None;
            currentState = State.Init;

            boxX = TGCBox.fromExtremes(TGCVector3.Empty, new TGCVector3(LARGE_AXIS_SIZE, SHORT_AXIS_SIZE, SHORT_AXIS_SIZE), Color.FromArgb(200, 50, 50));
            boxX.AutoTransform = true;
            boxY = TGCBox.fromExtremes(TGCVector3.Empty, new TGCVector3(SHORT_AXIS_SIZE, LARGE_AXIS_SIZE, SHORT_AXIS_SIZE), Color.FromArgb(50, 200, 50));
            boxY.AutoTransform = true;
            boxZ = TGCBox.fromExtremes(TGCVector3.Empty, new TGCVector3(SHORT_AXIS_SIZE, SHORT_AXIS_SIZE, LARGE_AXIS_SIZE), Color.FromArgb(50, 50, 200));
            boxZ.AutoTransform = true;
        }

        public override void setEnabled(bool enabled)
        {
            //Activar
            if (enabled)
            {
                Control.CurrentState = MeshCreatorModifier.State.GizmoActivated;
                currentState = State.Init;

                //Posicionar gizmo
                var aabb = MeshCreatorUtils.getSelectionBoundingBox(Control.SelectionList);
                gizmoCenter = aabb.calculateBoxCenter();
                setAxisPositionAndSize();
            }
        }

        /// <summary>
        /// Configurar posicion y tamaño de ejes segun la distancia a la camara
        /// </summary>
        private void setAxisPositionAndSize()
        {
            var increment = MeshCreatorUtils.getTranslateGizmoSizeIncrement(Control.Camera, gizmoCenter);

            boxX.Size = TGCVector3.Multiply(new TGCVector3(LARGE_AXIS_SIZE, SHORT_AXIS_SIZE, SHORT_AXIS_SIZE), increment);
            boxY.Size = TGCVector3.Multiply(new TGCVector3(SHORT_AXIS_SIZE, LARGE_AXIS_SIZE, SHORT_AXIS_SIZE), increment);
            boxZ.Size = TGCVector3.Multiply(new TGCVector3(SHORT_AXIS_SIZE, SHORT_AXIS_SIZE, LARGE_AXIS_SIZE), increment);

            boxX.Position = gizmoCenter + TGCVector3.Multiply(boxX.Size, 0.5f) + new TGCVector3(SHORT_AXIS_SIZE, 0, 0);
            boxY.Position = gizmoCenter + TGCVector3.Multiply(boxY.Size, 0.5f) + new TGCVector3(0, SHORT_AXIS_SIZE, 0);
            boxZ.Position = gizmoCenter + TGCVector3.Multiply(boxZ.Size, 0.5f) + new TGCVector3(0, 0, SHORT_AXIS_SIZE);

            boxX.updateValues();
            boxY.updateValues();
            boxZ.updateValues();
        }

        public override void update()
        {
            var input = Control.creator.Input;

            switch (currentState)
            {
                case State.Init:

                    selectedAxis = Axis.None;

                    //Iniciar seleccion de eje
                    if (input.buttonDown(TgcD3dInput.MouseButtons.BUTTON_LEFT))
                    {
                        Control.PickingRay.updateRay();
                        TGCVector3 collP;

                        //Buscar colision con eje con Picking
                        if (TgcCollisionUtils.intersectRayAABB(Control.PickingRay.Ray, boxX.BoundingBox, out collP))
                        {
                            selectedAxis = Axis.X;
                        }
                        else if (TgcCollisionUtils.intersectRayAABB(Control.PickingRay.Ray, boxY.BoundingBox, out collP))
                        {
                            selectedAxis = Axis.Y;
                        }
                        else if (TgcCollisionUtils.intersectRayAABB(Control.PickingRay.Ray, boxZ.BoundingBox,
                            out collP))
                        {
                            selectedAxis = Axis.Z;
                        }
                        else
                        {
                            selectedAxis = Axis.None;
                        }

                        //Si eligio un eje, iniciar dragging
                        if (selectedAxis != Axis.None)
                        {
                            currentState = State.Dragging;
                            initMouseP = new TGCVector2(input.XposRelative, input.YposRelative);
                        }
                        else
                        {
                            if (input.buttonPressed(TgcD3dInput.MouseButtons.BUTTON_LEFT))
                            {
                                var additive = input.keyDown(Key.LeftControl) || input.keyDown(Key.RightControl);
                                Control.CurrentState = MeshCreatorModifier.State.SelectObject;
                                Control.SelectionRectangle.doDirectSelection(additive);
                            }
                        }
                    }
                    //Hacer mouse over sobre los ejes
                    else
                    {
                        Control.PickingRay.updateRay();
                        TGCVector3 collP;

                        //Buscar colision con eje con Picking
                        if (TgcCollisionUtils.intersectRayAABB(Control.PickingRay.Ray, boxX.BoundingBox, out collP))
                        {
                            selectedAxis = Axis.X;
                        }
                        else if (TgcCollisionUtils.intersectRayAABB(Control.PickingRay.Ray, boxY.BoundingBox, out collP))
                        {
                            selectedAxis = Axis.Y;
                        }
                        else if (TgcCollisionUtils.intersectRayAABB(Control.PickingRay.Ray, boxZ.BoundingBox,
                            out collP))
                        {
                            selectedAxis = Axis.Z;
                        }
                        else
                        {
                            selectedAxis = Axis.None;
                        }
                    }

                    break;

                case State.Dragging:

                    //Mover
                    if (input.buttonDown(TgcD3dInput.MouseButtons.BUTTON_LEFT))
                    {
                        //Obtener vector 2D de movimiento relativo del mouse
                        var mouseScreenVec = new TGCVector2(input.XposRelative, input.YposRelative);
                        //mouseScreenVec.Normalize();

                        //Projectar vector 2D del eje elegido
                        var currentAxisBox = getAxisBox(selectedAxis);
                        var axisScreenVec = MeshCreatorUtils.projectAABBScreenVec(currentAxisBox.BoundingBox);

                        //Hacer DOT product entre ambos vectores para obtener la contribucion del mouse en esa direccion
                        var scaling = TGCVector2.Dot(axisScreenVec, mouseScreenVec);
                        scaling = MeshCreatorUtils.getMouseScaleIncrementSpeed(Control.Camera, currentAxisBox.BoundingBox, scaling);
                        var currentScale = TGCVector3.Empty;

                        //Escala en eje X
                        if (selectedAxis == Axis.X)
                        {
                            currentScale.X = scaling;
                        }
                        //Escala en eje Y
                        else if (selectedAxis == Axis.Y)
                        {
                            currentScale.Y = scaling;
                        }
                        //Escala en eje Z
                        else if (selectedAxis == Axis.Z)
                        {
                            currentScale.Z = scaling;
                        }

                        //Escalar objetos
                        foreach (var p in Control.SelectionList)
                        {
                            //Agregar scaling, controlando que no sea menor a cero
                            var scale = p.Scale;
                            var oldMin = p.BoundingBox.PMin;

                            scale += currentScale;
                            scale.X = scale.X < 0.01f ? 0.01f : scale.X;
                            scale.Y = scale.Y < 0.01f ? 0.01f : scale.Y;
                            scale.Z = scale.Z < 0.01f ? 0.01f : scale.Z;

                            p.Scale = scale;

                            //Si se escala para una sola direccion hay que desplazar el objeto
                            if (!Control.ScaleBothDirections)
                            {
                                p.move(oldMin - p.BoundingBox.PMin);
                            }
                        }

                        //TODO: Hay que ir estirando los ejes a medida que se agranda la escala

                        //Actualizar datos de modify
                        Control.UpdateModifyPanel();
                    }

                    //Soltar movimiento
                    else if (input.buttonUp(TgcD3dInput.MouseButtons.BUTTON_LEFT))
                    {
                        currentState = State.Init;
                        selectedAxis = Axis.None;
                    }

                    break;
            }

            //Ajustar tamaño de ejes
            setAxisPositionAndSize();
        }

        public override void render()
        {
            //Desactivar Z-Buffer para dibujar arriba de todo el escenario
            D3DDevice.Instance.Device.RenderState.ZBufferEnable = false;

            //Dibujar
            boxX.Render();
            boxY.Render();
            boxZ.Render();

            var selectedBox = getAxisBox(selectedAxis);
            if (selectedBox != null)
            {
                selectedBox.BoundingBox.Render();
            }

            D3DDevice.Instance.Device.RenderState.ZBufferEnable = true;
        }

        private TGCBox getAxisBox(Axis axis)
        {
            switch (axis)
            {
                case Axis.X:
                    return boxX;

                case Axis.Y:
                    return boxY;

                case Axis.Z:
                    return boxZ;
            }
            return null;
        }

        /// <summary>
        ///     Mover ejes del gizmo
        /// </summary>
        private void moveGizmo(TGCVector3 movement)
        {
            gizmoCenter += movement;
            boxX.Move(movement);
            boxY.Move(movement);
            boxZ.Move(movement);
        }

        /// <summary>
        ///     Mover gizmo
        /// </summary>
        public override void move(EditorPrimitive selectedPrimitive, TGCVector3 movement)
        {
            moveGizmo(movement);
        }

        public override void dipose()
        {
            boxX.Dispose();
            boxY.Dispose();
            boxZ.Dispose();
        }

        private enum Axis
        {
            X,
            Y,
            Z,
            None
        }

        private enum State
        {
            Init,
            Dragging
        }
    }
}