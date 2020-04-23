using System.Drawing;
using TGC.Core.Collision;
using TGC.Core.Geometry;
using TGC.Core.Input;
using TGC.Core.Mathematica;

namespace TGC.Tools.SceneEditor
{
    internal class SceneEditorTranslateGizmo
    {
        public enum Axis
        {
            X,
            Y,
            Z,
            None
        }

        private const float LARGE_AXIS_FACTOR_SIZE = 2f;
        private const float LARGE_AXIS_MIN_SIZE = 50f;
        private const float SHORT_AXIS_SIZE = 2f;
        private const float MOVE_FACTOR = 4f;

        private readonly TGCBox boxX;
        private readonly TGCBox boxY;
        private readonly TGCBox boxZ;
        private TGCVector2 initMouseP;

        private TGCBox selectedAxisBox;

        private TgcD3dInput input;

        public SceneEditorTranslateGizmo(TgcD3dInput input)
        {
            boxX = TGCBox.fromExtremes(TGCVector3.Empty, new TGCVector3(LARGE_AXIS_FACTOR_SIZE, SHORT_AXIS_SIZE, SHORT_AXIS_SIZE), Color.Red);
            boxX.AutoTransformEnable = true;
            boxY = TGCBox.fromExtremes(TGCVector3.Empty, new TGCVector3(SHORT_AXIS_SIZE, LARGE_AXIS_FACTOR_SIZE, SHORT_AXIS_SIZE), Color.Green);
            boxY.AutoTransformEnable = true;
            boxZ = TGCBox.fromExtremes(TGCVector3.Empty, new TGCVector3(SHORT_AXIS_SIZE, SHORT_AXIS_SIZE, LARGE_AXIS_FACTOR_SIZE), Color.Blue);
            boxZ.AutoTransformEnable = true;

            this.input = input;
        }

        /// <summary>
        ///     Objeto sobre el cual se aplica el movimiento
        /// </summary>
        public SceneEditorMeshObject MeshObj { get; private set; }

        /// <summary>
        ///     Eje seleccionado
        /// </summary>
        public Axis SelectedAxis { get; private set; }

        /// <summary>
        ///     Acomodar Gizmo en base a un Mesh
        /// </summary>
        public void setMesh(SceneEditorMeshObject meshObj)
        {
            MeshObj = meshObj;
            SelectedAxis = Axis.None;

            /*
            float aabbbR = meshObj.mesh.BoundingBox.calculateBoxRadius();
            float largeSize = LARGE_AXIS_SIZE * aabbbR;
            float shortSize = SHORT_AXIS_SIZE;

            boxX.Size = new TGCVector3(largeSize, shortSize, shortSize);
            boxY.Size = new TGCVector3(shortSize, largeSize, shortSize);
            boxZ.Size = new TGCVector3(shortSize, shortSize, largeSize);

            TGCVector3 pos = meshObj.mesh.Position;
            boxX.Position = pos + TGCVector3.Scale(boxX.Size, 0.5f);
            boxY.Position = pos + TGCVector3.Scale(boxY.Size, 0.5f);
            boxZ.Position = pos + TGCVector3.Scale(boxZ.Size, 0.5f);
            */

            var meshCenter = meshObj.mesh.BoundingBox.calculateBoxCenter();
            var axisRadius = meshObj.mesh.BoundingBox.calculateAxisRadius();

            var largeX = axisRadius.X + LARGE_AXIS_MIN_SIZE;
            var largeY = axisRadius.Y + LARGE_AXIS_MIN_SIZE;
            var largeZ = axisRadius.Z + LARGE_AXIS_MIN_SIZE;

            boxX.Size = new TGCVector3(largeX, SHORT_AXIS_SIZE, SHORT_AXIS_SIZE);
            boxY.Size = new TGCVector3(SHORT_AXIS_SIZE, largeY, SHORT_AXIS_SIZE);
            boxZ.Size = new TGCVector3(SHORT_AXIS_SIZE, SHORT_AXIS_SIZE, largeZ);

            boxX.Position = meshCenter + TGCVector3.Multiply(boxX.Size, 0.5f);
            boxY.Position = meshCenter + TGCVector3.Multiply(boxY.Size, 0.5f);
            boxZ.Position = meshCenter + TGCVector3.Multiply(boxZ.Size, 0.5f);

            boxX.updateValues();
            boxY.updateValues();
            boxZ.updateValues();
        }

        /// <summary>
        ///     Detectar el eje seleccionado
        /// </summary>
        public void detectSelectedAxis(TgcPickingRay pickingRay)
        {
            pickingRay.updateRay();
            TGCVector3 collP;

            //Buscar colision con eje con Picking
            if (TgcCollisionUtils.intersectRayAABB(pickingRay.Ray, boxX.BoundingBox, out collP))
            {
                SelectedAxis = Axis.X;
                selectedAxisBox = boxX;
            }
            else if (TgcCollisionUtils.intersectRayAABB(pickingRay.Ray, boxY.BoundingBox, out collP))
            {
                SelectedAxis = Axis.Y;
                selectedAxisBox = boxY;
            }
            else if (TgcCollisionUtils.intersectRayAABB(pickingRay.Ray, boxZ.BoundingBox, out collP))
            {
                SelectedAxis = Axis.Z;
                selectedAxisBox = boxZ;
            }
            else
            {
                SelectedAxis = Axis.None;
                selectedAxisBox = null;
            }

            //Desplazamiento inicial
            if (SelectedAxis != Axis.None)
            {
                initMouseP = new TGCVector2(input.XposRelative, input.YposRelative);
            }
        }

        /// <summary>
        ///     Actualizar posición de la malla en base a movimientos del mouse
        /// </summary>
        public void updateMove()
        {
            var currentMove = TGCVector3.Empty;

            //Desplazamiento segun el mouse en X
            if (SelectedAxis == Axis.X)
            {
                currentMove.X += (input.XposRelative - initMouseP.X) * MOVE_FACTOR;
            }
            //Desplazamiento segun el mouse en Y
            else if (SelectedAxis == Axis.Y)
            {
                currentMove.Y -= (input.YposRelative - initMouseP.Y) * MOVE_FACTOR;
            }
            //Desplazamiento segun el mouse en X
            else if (SelectedAxis == Axis.Z)
            {
                currentMove.Z -= (input.YposRelative - initMouseP.Y) * MOVE_FACTOR;
            }

            //Mover mesh
            MeshObj.mesh.Move(currentMove);

            //Mover ejes
            boxX.Move(currentMove);
            boxY.Move(currentMove);
            boxZ.Move(currentMove);
        }

        /// <summary>
        ///     Termina el arrastre sobre un eje
        /// </summary>
        public void endDragging()
        {
            SelectedAxis = Axis.None;
            selectedAxisBox = null;
        }

        /// <summary>
        ///     Esconder Gizmo
        /// </summary>
        public void hide()
        {
            MeshObj = null;
        }

        public void render()
        {
            if (MeshObj == null)
                return;

            boxX.Render();
            boxY.Render();
            boxZ.Render();

            if (SelectedAxis != Axis.None)
            {
                selectedAxisBox.BoundingBox.Render();
            }
        }
    }
}