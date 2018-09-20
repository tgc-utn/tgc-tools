using System.Collections.Generic;
using System.Drawing;
using TGC.Core.BoundingVolumes;
using TGC.Core.Collision;
using TGC.Core.Mathematica;

namespace TGC.Tools.MeshCreator.EditablePoly.Primitives
{
    /// <summary>
    ///     Vertice de EditablePoly
    /// </summary>
    public class EditPolyVertex : EditPolyPrimitive
    {
        /// <summary>
        ///     Sphere for ray-collisions
        /// </summary>
        private static readonly TgcBoundingSphere COLLISION_SPHERE = new TgcBoundingSphere(new TGCVector3(0, 0, 0), 2);

        /*public TGCVector3 normal;
        public TGCVector2 texCoords;
        public TGCVector2 texCoords2;
        public int color;*/
        public List<EditPolyEdge> edges;
        public TGCVector3 movement;

        public TGCVector3 position;
        public int vbIndex;

        public EditPolyVertex()
        {
            movement = new TGCVector3(0, 0, 0);
        }

        public override EditablePoly.PrimitiveType Type
        {
            get { return EditablePoly.PrimitiveType.Vertex; }
        }

        public override string ToString()
        {
            return "Index: " + vbIndex + ", Pos: " + TGCVector3.PrintVector3(position);
        }

        public override bool projectToScreen(TGCMatrix transform, out Rectangle box2D)
        {
            return MeshCreatorUtils.projectPoint(TGCVector3.TransformCoordinate(position, transform), out box2D);
        }

        public override bool intersectRay(TgcRay ray, TGCMatrix transform, out TGCVector3 q)
        {
            COLLISION_SPHERE.setCenter(TGCVector3.TransformCoordinate(position, transform));
            float t;
            return TgcCollisionUtils.intersectRaySphere(ray, COLLISION_SPHERE, out t, out q);
        }

        public override TGCVector3 computeCenter()
        {
            return position;
        }

        public override void move(TGCVector3 movement)
        {
            //position += movement;
            this.movement = movement;
        }

        /// <summary>
        ///     Eliminar arista de la lista
        /// </summary>
        public void removeEdge(EditPolyEdge e)
        {
            for (var i = 0; i < edges.Count; i++)
            {
                if (e == edges[i])
                {
                    edges.RemoveAt(i);
                    break;
                }
            }
        }
    }
}