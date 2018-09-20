using System.Collections.Generic;
using System.Drawing;
using TGC.Core.BoundingVolumes;
using TGC.Core.Collision;
using TGC.Core.Mathematica;

namespace TGC.Tools.MeshCreator.EditablePoly.Primitives
{
    /// <summary>
    ///     Arista de EditablePoly
    /// </summary>
    public class EditPolyEdge : EditPolyPrimitive
    {
        private static readonly TgcBoundingOrientedBox COLLISION_OBB = new TgcBoundingOrientedBox();

        public EditPolyVertex a;
        public EditPolyVertex b;
        public List<EditPolyPolygon> faces;

        public override EditablePoly.PrimitiveType Type
        {
            get { return EditablePoly.PrimitiveType.Edge; }
        }

        public override string ToString()
        {
            return a.vbIndex + " => " + b.vbIndex;
        }

        public override bool projectToScreen(TGCMatrix transform, out Rectangle box2D)
        {
            return MeshCreatorUtils.projectSegmentToScreenRect(
                TGCVector3.TransformCoordinate(a.position, transform),
                TGCVector3.TransformCoordinate(b.position, transform), out box2D);
        }

        public override bool intersectRay(TgcRay ray, TGCMatrix transform, out TGCVector3 q)
        {
            //Actualizar OBB con posiciones de la arista para utilizar en colision
            EditablePolyUtils.updateObbFromSegment(COLLISION_OBB,
                TGCVector3.TransformCoordinate(a.position, transform),
                TGCVector3.TransformCoordinate(b.position, transform),
                0.4f);

            //ray-obb
            return TgcCollisionUtils.intersectRayObb(ray, COLLISION_OBB, out q);
        }

        public override TGCVector3 computeCenter()
        {
            return (a.position + b.position) * 0.5f;
        }

        public override void move(TGCVector3 movement)
        {
            /*
            a.position += movement;
            b.position += movement;
             */
            a.movement = movement;
            b.movement = movement;
        }

        /// <summary>
        ///     Quitar poligono de la lista
        /// </summary>
        public void removePolygon(EditPolyPolygon p)
        {
            for (var i = 0; i < faces.Count; i++)
            {
                if (faces[i] == p)
                {
                    faces.RemoveAt(i);
                    break;
                }
            }
        }
    }
}