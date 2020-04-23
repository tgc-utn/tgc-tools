using Microsoft.DirectX.Direct3D;
using System.Drawing;
using TGC.Core.BoundingVolumes;
using TGC.Core.Collision;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using TGC.Core.Shaders;
using TGC.Core.Textures;
using TGC.Tools.UserControls;

namespace TGC.Tools.MeshCreator
{
    /// <summary>
    ///     Grilla del piso del escenario
    /// </summary>
    public class Grid
    {
        private const float BIG_VAL = 100000;
        private const float SMALL_VAL = 0.1f;

        private const float GRID_RADIUS = 100f;
        private const float LINE_SEPARATION = 20f;

        private readonly MeshCreatorModifier control;
        private readonly TgcBoundingAxisAlignBox pickingXYAabb;

        private readonly TgcBoundingAxisAlignBox pickingXZAabb;
        private readonly TgcBoundingAxisAlignBox pickingYZAabb;
        private CustomVertex.PositionColored[] vertices;

        public Grid(MeshCreatorModifier control)
        {
            this.control = control;

            //El bounding box del piso es bien grande para hacer colisiones
            BoundingBox = new TgcBoundingAxisAlignBox(new TGCVector3(-BIG_VAL, -SMALL_VAL, -BIG_VAL),
                new TGCVector3(BIG_VAL, 0, BIG_VAL));

            //Planos para colision de picking
            pickingXZAabb = new TgcBoundingAxisAlignBox(new TGCVector3(-BIG_VAL, -SMALL_VAL, -BIG_VAL),
                new TGCVector3(BIG_VAL, 0, BIG_VAL));
            pickingXYAabb = new TgcBoundingAxisAlignBox(new TGCVector3(-BIG_VAL, -BIG_VAL, -SMALL_VAL),
                new TGCVector3(BIG_VAL, BIG_VAL, 0));
            pickingYZAabb = new TgcBoundingAxisAlignBox(new TGCVector3(-SMALL_VAL, -BIG_VAL, -BIG_VAL),
                new TGCVector3(0, BIG_VAL, BIG_VAL));

            vertices = new CustomVertex.PositionColored[12 * 2 * 2];
            var color = Color.FromArgb(76, 76, 76).ToArgb();

            //10 lineas horizontales en X
            for (var i = 0; i < 11; i++)
            {
                vertices[i * 2] = new CustomVertex.PositionColored(-GRID_RADIUS, 0, -GRID_RADIUS + LINE_SEPARATION * i,
                    color);
                vertices[i * 2 + 1] = new CustomVertex.PositionColored(GRID_RADIUS, 0, -GRID_RADIUS + LINE_SEPARATION * i,
                    color);
            }

            //10 lineas horizontales en Z
            for (var i = 11; i < 22; i++)
            {
                vertices[i * 2] = new CustomVertex.PositionColored(-GRID_RADIUS * 3 + LINE_SEPARATION * i - LINE_SEPARATION, 0,
                    -GRID_RADIUS, color);
                vertices[i * 2 + 1] =
                    new CustomVertex.PositionColored(-GRID_RADIUS * 3 + LINE_SEPARATION * i - LINE_SEPARATION, 0,
                        GRID_RADIUS, color);
            }
        }

        /// <summary>
        ///     BoundingBox
        /// </summary>
        public TgcBoundingAxisAlignBox BoundingBox { get; }

        public void render()
        {
            var d3dDevice = D3DDevice.Instance.Device;
            var texturesManager = TexturesManager.Instance;

            texturesManager.clear(0);
            texturesManager.clear(1);

            var effect = TGCShaders.Instance.VariosShader;
            effect.Technique = TGCShaders.T_POSITION_COLORED;
            TGCShaders.Instance.SetShaderMatrixIdentity(effect);
            d3dDevice.VertexDeclaration = TGCShaders.Instance.VdecPositionColored;

            //Render con shader
            effect.Begin(0);
            effect.BeginPass(0);
            d3dDevice.DrawUserPrimitives(PrimitiveType.LineList, 22, vertices);
            effect.EndPass();
            effect.End();
        }

        public void dispose()
        {
            BoundingBox.Dispose();
            vertices = null;
        }

        /// <summary>
        ///     Picking al grid
        /// </summary>
        public TGCVector3 getPicking()
        {
            control.PickingRay.updateRay();
            TGCVector3 collisionPoint;
            if (TgcCollisionUtils.intersectRayAABB(control.PickingRay.Ray, BoundingBox, out collisionPoint))
            {
                return collisionPoint;
            }

            return TGCVector3.Empty;
            //throw new Exception("Sin colision con Grid");
        }

        /// <summary>
        ///     Picking con plano XZ ubicado en el centro del objeto
        /// </summary>
        public TGCVector3 getPickingXZ(TgcRay ray, TGCVector3 objCenter)
        {
            //Mover aabb en Y al centro del mesh
            pickingXZAabb.setExtremes(
                new TGCVector3(pickingXZAabb.PMin.X, objCenter.Y - SMALL_VAL, pickingXZAabb.PMin.Z),
                new TGCVector3(pickingXZAabb.PMax.X, objCenter.Y, pickingXZAabb.PMax.Z));
            TGCVector3 q;
            var r = TgcCollisionUtils.intersectRayAABB(ray, pickingXZAabb, out q);
            if (r)
                return clampPickingResult(q);
            return objCenter;
        }

        /// <summary>
        ///     Picking con plano XY ubicado en el centro del objeto
        /// </summary>
        public TGCVector3 getPickingXY(TgcRay ray, TGCVector3 objCenter)
        {
            //Mover aabb en Y al centro del mesh
            pickingXYAabb.setExtremes(
                new TGCVector3(pickingXYAabb.PMin.X, pickingXYAabb.PMin.Y, objCenter.Z - SMALL_VAL),
                new TGCVector3(pickingXYAabb.PMax.X, pickingXYAabb.PMax.Y, objCenter.Z));
            TGCVector3 q;
            var r = TgcCollisionUtils.intersectRayAABB(ray, pickingXYAabb, out q);
            if (r)
                return clampPickingResult(q);
            return objCenter;
        }

        /// <summary>
        ///     Picking con plano YZ ubicado en el centro del objeto
        /// </summary>
        public TGCVector3 getPickingYZ(TgcRay ray, TGCVector3 objCenter)
        {
            //Mover aabb en Y al centro del mesh
            pickingYZAabb.setExtremes(
                new TGCVector3(objCenter.X - SMALL_VAL, pickingYZAabb.PMin.Y, pickingYZAabb.PMin.Z),
                new TGCVector3(objCenter.X, pickingYZAabb.PMax.Y, pickingYZAabb.PMax.Z));
            TGCVector3 q;
            var r = TgcCollisionUtils.intersectRayAABB(ray, pickingYZAabb, out q);
            if (r)
                return clampPickingResult(q);
            return objCenter;
        }

        /// <summary>
        ///     Picking con los planos XZ e XY ubicados en el centro del objeto
        /// </summary>
        public TGCVector3 getPickingX(TgcRay ray, TGCVector3 objCenter)
        {
            //Mover ambos planos hacia el centro del objeto
            pickingXZAabb.setExtremes(
                new TGCVector3(pickingXZAabb.PMin.X, objCenter.Y - SMALL_VAL, pickingXZAabb.PMin.Z),
                new TGCVector3(pickingXZAabb.PMax.X, objCenter.Y, pickingXZAabb.PMax.Z));
            pickingXYAabb.setExtremes(
                new TGCVector3(pickingXYAabb.PMin.X, pickingXYAabb.PMin.Y, objCenter.Z - SMALL_VAL),
                new TGCVector3(pickingXYAabb.PMax.X, pickingXYAabb.PMax.Y, objCenter.Z));

            TGCVector3 q1, q2;
            bool r1, r2;
            r1 = TgcCollisionUtils.intersectRayAABB(ray, pickingXZAabb, out q1);
            r2 = TgcCollisionUtils.intersectRayAABB(ray, pickingXYAabb, out q2);

            if (r1 && r2)
            {
                var objPos = new TGCVector2(objCenter.Y, objCenter.Z);
                var diff1 = TGCVector2.Length(new TGCVector2(q1.Y, q1.Z) - objPos);
                var diff2 = TGCVector2.Length(new TGCVector2(q2.Y, q2.Z) - objPos);
                return diff1 < diff2 ? q1 : q2;
            }
            if (r1)
                return clampPickingResult(q1);
            if (r2)
                return clampPickingResult(q2);
            return objCenter;
        }

        /// <summary>
        ///     Picking con los planos XY e YZ ubicados en el centro del objeto
        /// </summary>
        public TGCVector3 getPickingY(TgcRay ray, TGCVector3 objCenter)
        {
            //Mover ambos planos hacia el centro del objeto
            pickingXYAabb.setExtremes(
                new TGCVector3(pickingXYAabb.PMin.X, pickingXYAabb.PMin.Y, objCenter.Z - SMALL_VAL),
                new TGCVector3(pickingXYAabb.PMax.X, pickingXYAabb.PMax.Y, objCenter.Z));
            pickingYZAabb.setExtremes(
                new TGCVector3(objCenter.X - SMALL_VAL, pickingYZAabb.PMin.Y, pickingYZAabb.PMin.Z),
                new TGCVector3(objCenter.X, pickingYZAabb.PMax.Y, pickingYZAabb.PMax.Z));

            TGCVector3 q1, q2;
            bool r1, r2;
            r1 = TgcCollisionUtils.intersectRayAABB(ray, pickingXYAabb, out q1);
            r2 = TgcCollisionUtils.intersectRayAABB(ray, pickingYZAabb, out q2);

            if (r1 && r2)
            {
                var objPos = new TGCVector2(objCenter.X, objCenter.Z);
                var diff1 = TGCVector2.Length(new TGCVector2(q1.X, q1.Z) - objPos);
                var diff2 = TGCVector2.Length(new TGCVector2(q2.X, q2.Z) - objPos);
                return diff1 < diff2 ? q1 : q2;
            }
            if (r1)
                return clampPickingResult(q1);
            if (r2)
                return clampPickingResult(q2);
            return objCenter;
        }

        /// <summary>
        ///     Picking con los planos XZ e YZ ubicados en el centro del objeto
        /// </summary>
        public TGCVector3 getPickingZ(TgcRay ray, TGCVector3 objCenter)
        {
            //Mover ambos planos hacia el centro del objeto
            pickingXZAabb.setExtremes(
                new TGCVector3(pickingXZAabb.PMin.X, objCenter.Y - SMALL_VAL, pickingXZAabb.PMin.Z),
                new TGCVector3(pickingXZAabb.PMax.X, objCenter.Y, pickingXZAabb.PMax.Z));
            pickingYZAabb.setExtremes(
                new TGCVector3(objCenter.X - SMALL_VAL, pickingYZAabb.PMin.Y, pickingYZAabb.PMin.Z),
                new TGCVector3(objCenter.X, pickingYZAabb.PMax.Y, pickingYZAabb.PMax.Z));

            TGCVector3 q1, q2;
            bool r1, r2;
            r1 = TgcCollisionUtils.intersectRayAABB(ray, pickingXZAabb, out q1);
            r2 = TgcCollisionUtils.intersectRayAABB(ray, pickingYZAabb, out q2);

            if (r1 && r2)
            {
                var objPos = new TGCVector2(objCenter.X, objCenter.Y);
                var diff1 = TGCVector2.Length(new TGCVector2(q1.X, q1.Y) - objPos);
                var diff2 = TGCVector2.Length(new TGCVector2(q2.X, q2.Y) - objPos);
                return diff1 < diff2 ? q1 : q2;
            }
            if (r1)
                return clampPickingResult(q1);
            if (r2)
                return clampPickingResult(q2);
            return objCenter;
        }

        private TGCVector3 clampPickingResult(TGCVector3 v)
        {
            v.X = FastMath.Clamp(v.X, -BIG_VAL, BIG_VAL);
            v.Y = FastMath.Clamp(v.Y, -BIG_VAL, BIG_VAL);
            v.Z = FastMath.Clamp(v.Z, -BIG_VAL, BIG_VAL);
            return v;
        }
    }
}