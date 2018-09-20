using System.Collections.Generic;
using System.Drawing;
using System.Text;
using TGC.Core.Collision;
using TGC.Core.Mathematica;

namespace TGC.Tools.MeshCreator.EditablePoly.Primitives
{
    /// <summary>
    ///     Poligono de EditablePoly
    /// </summary>
    public class EditPolyPolygon : EditPolyPrimitive
    {
        public List<EditPolyEdge> edges;
        public int matId;
        public TGCPlane TGCPlane;
        public List<int> vbTriangles;
        public List<EditPolyVertex> vertices;

        public override EditablePoly.PrimitiveType Type
        {
            get { return EditablePoly.PrimitiveType.Polygon; }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < vertices.Count; i++)
            {
                sb.Append(vertices[i].vbIndex + ", ");
            }
            sb.Remove(sb.Length - 2, 2);
            return sb.ToString();
        }

        public override bool projectToScreen(TGCMatrix transform, out Rectangle box2D)
        {
            var v = new TGCVector3[vertices.Count];
            for (var i = 0; i < v.Length; i++)
            {
                v[i] = TGCVector3.TransformCoordinate(vertices[i].position, transform);
            }
            return MeshCreatorUtils.projectPolygon(v, out box2D);
        }

        public override bool intersectRay(TgcRay ray, TGCMatrix transform, out TGCVector3 q)
        {
            var v = new TGCVector3[vertices.Count];
            for (var i = 0; i < v.Length; i++)
            {
                v[i] = TGCVector3.TransformCoordinate(vertices[i].position, transform);
            }
            float t;
            return TgcCollisionUtils.intersectRayConvexPolygon(ray, v, out t, out q);
        }

        /// <summary>
        ///     Normal del plano del poligono
        /// </summary>
        public TGCVector3 getNormal()
        {
            return new TGCVector3(TGCPlane.A, TGCPlane.B, TGCPlane.C);
        }

        public override TGCVector3 computeCenter()
        {
            var sum = vertices[0].position;
            for (var i = 1; i < vertices.Count; i++)
            {
                sum += vertices[i].position;
            }
            return TGCVector3.Scale(sum, 1f / vertices.Count);
        }

        public override void move(TGCVector3 movement)
        {
            /*
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i].position += movement;
            }
            */
            for (var i = 0; i < vertices.Count; i++)
            {
                vertices[i].movement = movement;
            }
        }

        /// <summary>
        ///     Quitar arista de la lista
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