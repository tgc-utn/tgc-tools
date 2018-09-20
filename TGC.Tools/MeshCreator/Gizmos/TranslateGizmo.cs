﻿using Microsoft.DirectX.DirectInput;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Tools.MeshCreator.Primitives;
using TGC.Tools.Model;
using TGC.Tools.UserControls;

namespace TGC.Tools.MeshCreator.Gizmos
{
    /// <summary>
    ///     Gizmo para trasladar objetos
    /// </summary>
    public class TranslateGizmo : EditorGizmo
    {
        private TGCVector3 acumMovement;

        private State currentState;
        private readonly TranslateGizmoMesh gizmoMesh;
        private TGCVector3 initMouseP3d;

        public TranslateGizmo(MeshCreatorModifier control)
            : base(control)
        {
            gizmoMesh = new TranslateGizmoMesh();
            currentState = State.Init;
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
                gizmoMesh.setCenter(aabb.calculateBoxCenter(), Control.Camera);
            }
        }

        public override void update()
        {
            var input = ToolsModel.Instance.Input;

            switch (currentState)
            {
                case State.Init:

                    acumMovement = TGCVector3.Empty;
                    gizmoMesh.unSelect();

                    //Iniciar seleccion de eje
                    if (input.buttonDown(TgcD3dInput.MouseButtons.BUTTON_LEFT))
                    {
                        //Buscar colision con ejes del gizmo
                        Control.PickingRay.updateRay();
                        gizmoMesh.selectAxisByPicking(Control.PickingRay.Ray);
                        if (gizmoMesh.SelectedAxis == TranslateGizmoMesh.Axis.X)
                        {
                            initMouseP3d = Control.Grid.getPickingX(Control.PickingRay.Ray, gizmoMesh.GizmoCenter);
                        }
                        else if (gizmoMesh.SelectedAxis == TranslateGizmoMesh.Axis.Y)
                        {
                            initMouseP3d = Control.Grid.getPickingY(Control.PickingRay.Ray, gizmoMesh.GizmoCenter);
                        }
                        else if (gizmoMesh.SelectedAxis == TranslateGizmoMesh.Axis.Z)
                        {
                            initMouseP3d = Control.Grid.getPickingZ(Control.PickingRay.Ray, gizmoMesh.GizmoCenter);
                        }
                        else if (gizmoMesh.SelectedAxis == TranslateGizmoMesh.Axis.XZ)
                        {
                            initMouseP3d = Control.Grid.getPickingXZ(Control.PickingRay.Ray, gizmoMesh.GizmoCenter);
                        }
                        else if (gizmoMesh.SelectedAxis == TranslateGizmoMesh.Axis.XY)
                        {
                            initMouseP3d = Control.Grid.getPickingXY(Control.PickingRay.Ray, gizmoMesh.GizmoCenter);
                        }
                        else if (gizmoMesh.SelectedAxis == TranslateGizmoMesh.Axis.YZ)
                        {
                            initMouseP3d = Control.Grid.getPickingYZ(Control.PickingRay.Ray, gizmoMesh.GizmoCenter);
                        }

                        //Si eligio un eje, iniciar dragging
                        if (gizmoMesh.SelectedAxis != TranslateGizmoMesh.Axis.None)
                        {
                            currentState = State.Dragging;
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
                        //Buscar colision con eje con Picking
                        Control.PickingRay.updateRay();
                        gizmoMesh.selectAxisByPicking(Control.PickingRay.Ray);
                    }

                    break;

                case State.Dragging:

                    //Mover
                    if (input.buttonDown(TgcD3dInput.MouseButtons.BUTTON_LEFT))
                    {
                        Control.PickingRay.updateRay();
                        var endMouseP3d = initMouseP3d;

                        //Solo se mueve un eje
                        var currentMove = new TGCVector3(0, 0, 0);
                        if (gizmoMesh.isSingleAxis(gizmoMesh.SelectedAxis))
                        {
                            //Desplazamiento en eje X
                            if (gizmoMesh.SelectedAxis == TranslateGizmoMesh.Axis.X)
                            {
                                endMouseP3d = Control.Grid.getPickingX(Control.PickingRay.Ray, gizmoMesh.GizmoCenter);
                                currentMove.X = endMouseP3d.X - initMouseP3d.X;
                            }
                            //Desplazamiento en eje Y
                            else if (gizmoMesh.SelectedAxis == TranslateGizmoMesh.Axis.Y)
                            {
                                endMouseP3d = Control.Grid.getPickingY(Control.PickingRay.Ray, gizmoMesh.GizmoCenter);
                                currentMove.Y = endMouseP3d.Y - initMouseP3d.Y;
                            }
                            //Desplazamiento en eje Z
                            else if (gizmoMesh.SelectedAxis == TranslateGizmoMesh.Axis.Z)
                            {
                                endMouseP3d = Control.Grid.getPickingZ(Control.PickingRay.Ray, gizmoMesh.GizmoCenter);
                                currentMove.Z = endMouseP3d.Z - initMouseP3d.Z;
                            }
                        }
                        //Desplazamiento en dos ejes
                        else
                        {
                            //Plano XZ
                            if (gizmoMesh.SelectedAxis == TranslateGizmoMesh.Axis.XZ)
                            {
                                endMouseP3d = Control.Grid.getPickingXZ(Control.PickingRay.Ray, gizmoMesh.GizmoCenter);
                                currentMove.X = endMouseP3d.X - initMouseP3d.X;
                                currentMove.Z = endMouseP3d.Z - initMouseP3d.Z;
                            }
                            //Plano XY
                            else if (gizmoMesh.SelectedAxis == TranslateGizmoMesh.Axis.XY)
                            {
                                endMouseP3d = Control.Grid.getPickingXY(Control.PickingRay.Ray, gizmoMesh.GizmoCenter);
                                currentMove.X = endMouseP3d.X - initMouseP3d.X;
                                currentMove.Y = endMouseP3d.Y - initMouseP3d.Y;
                            }
                            //Plano YZ
                            else if (gizmoMesh.SelectedAxis == TranslateGizmoMesh.Axis.YZ)
                            {
                                endMouseP3d = Control.Grid.getPickingYZ(Control.PickingRay.Ray, gizmoMesh.GizmoCenter);
                                currentMove.Y = endMouseP3d.Y - initMouseP3d.Y;
                                currentMove.Z = endMouseP3d.Z - initMouseP3d.Z;
                            }
                        }

                        //Actualizar pos del mouse 3D para proximo cuadro
                        initMouseP3d = endMouseP3d;

                        //Ajustar currentMove con Snap to grid
                        if (Control.SnapToGridEnabled)
                        {
                            var currentMoveX = currentMove.X;
                            var currentMoveY = currentMove.Y;
                            var currentMoveZ = currentMove.Z;

                            var acumMovementX = acumMovement.X;
                            var acumMovementY = acumMovement.Y;
                            var acumMovementZ = acumMovement.Z;

                            snapMovementToGrid(ref currentMoveX, ref acumMovementX, Control.SnapToGridCellSize);
                            snapMovementToGrid(ref currentMoveY, ref acumMovementY, Control.SnapToGridCellSize);
                            snapMovementToGrid(ref currentMoveZ, ref acumMovementZ, Control.SnapToGridCellSize);

                            currentMove = new TGCVector3(currentMoveX, currentMoveY, currentMoveZ);
                            acumMovement = new TGCVector3(acumMovementX, acumMovementY, acumMovementZ);
                        }

                        //Mover objetos
                        foreach (var p in Control.SelectionList)
                        {
                            p.move(currentMove);
                        }

                        //Mover ejes
                        gizmoMesh.moveGizmo(currentMove);

                        //Actualizar datos de modify
                        Control.updateModifyPanel();
                    }

                    //Soltar movimiento
                    else if (input.buttonUp(TgcD3dInput.MouseButtons.BUTTON_LEFT))
                    {
                        currentState = State.Init;
                        gizmoMesh.unSelect();
                    }

                    break;
            }

            //Ajustar tamaño de ejes
            gizmoMesh.setCenter(gizmoMesh.GizmoCenter, Control.Camera);
        }

        public override void render()
        {
            gizmoMesh.render();
        }

        /// <summary>
        ///     Mover gizmo
        /// </summary>
        public override void move(EditorPrimitive selectedPrimitive, TGCVector3 movement)
        {
            gizmoMesh.moveGizmo(movement);
        }

        /// <summary>
        ///     Calcular desplazamiento en unidades de grilla
        /// </summary>
        private float snapMovementToGrid(ref float currentMove, ref float acumMove, float cellSize)
        {
            //Se movio algo?
            var totalMove = acumMove + currentMove;
            const float epsilon = 0.1f;
            var absMove = FastMath.Abs(totalMove);
            float snapMove = 0;
            if (absMove > epsilon)
            {
                if (absMove > cellSize)
                {
                    //Moverse en unidades de la grilla
                    currentMove = (int)(totalMove / cellSize) * cellSize;
                    acumMove = 0;
                }
                else
                {
                    //Acumular movimiento para la proxima
                    acumMove += currentMove;
                    currentMove = 0;
                }
            }

            return snapMove;
        }

        public override void dipose()
        {
            gizmoMesh.dispose();
        }

        private enum State
        {
            Init,
            Dragging
        }
    }
}