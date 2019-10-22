using System.Collections.Generic;
using TGC.Core.BoundingVolumes;
using TGC.Core.Mathematica;
using TGC.Tools.MeshCreator.EditablePoly.Primitives;

namespace TGC.Tools.MeshCreator.EditablePoly
{
    public class EditablePolyUtils
    {
        /// <summary>
        ///     Epsilon para comparar si dos vertices son iguales
        /// </summary>
        public static readonly float EPSILON = 0.0001f;

        /*
        /// <summary>
        /// Filtrar todas las aristas que tiene un poligono y dejarle solo las que son parte del borde del poligono
        /// (Se quitan todas las aristas interiores)
        /// </summary>
        public static void computePolygonExternalEdges(Polygon p)
        {
            if (p.vertices.Count == 3)
                return;

            TGCVector3 TGCPlaneNorm = p.getNormal();
            List<Edge> externalEdges = new List<Edge>();
            foreach (Edge e in p.edges)
            {
                //Half-TGCPlane entre la arista y la normal del poligono
                TGCVector3 vec = e.b.position - e.a.position;
                TGCVector3 n = TGCVector3.Cross(TGCPlaneNorm, vec);
                TGCPlane halfTGCPlane = TGCPlane.FromPointNormal(e.a.position, n);

                //Checkear el signo de todos los demas vertices del poligono
                bool first = true;
                TgcCollisionUtils.PointTGCPlaneResult lastR = TgcCollisionUtils.PointTGCPlaneResult.COINCIDENT;
                bool inside = false;
                foreach (Vertex v in p.vertices)
                {
                    if(v.vbIndex != e.a.vbIndex && v.vbIndex != e.b.vbIndex )
                    {
                        TgcCollisionUtils.PointTGCPlaneResult r = TgcCollisionUtils.classifyPointTGCPlane(v.position, halfTGCPlane);
                        if(first)
                        {
                            first = false;
                            lastR = r;
                        }
                        else if(r != lastR)
                        {
                            inside = true;
                            break;
                        }
                    }
                }
                if(!inside)
                {
                    externalEdges.Add(e);
                }
            }
            p.edges = externalEdges;
        }

        /// <summary>
        /// Ordenar los vertices del poligono en base al recorrido de sus aristas externas
        /// </summary>
        public static void sortPolygonVertices(Polygon p)
        {
            if (p.vertices.Count == 3)
                return;

            List<Vertex> sortedVertices = new List<Vertex>();
            Edge lastEdge = p.edges[0];
            for (int i = 1; i < p.edges.Count; i++)
            {
                sortedVertices.Add(lastEdge.a);
                bool found = false;
                foreach (Edge e in p.edges)
                {
                    if(lastEdge.b.vbIndex == e.a.vbIndex)
                    {
                        lastEdge = e;
                        found = true;
                        break;
                    }
                }
                if(!found)
                    throw new Exception("No se pudo recorrer aristas de poligono en loop. Poligono: " + p);
            }
            sortedVertices.Add(lastEdge.a);
            p.vertices = sortedVertices;
        }
        */

        /// <summary>
        ///     Agregar un vertice a un poligono existente, ubicandolo en el medio de los dos vertices de la arista que compartian
        ///     entre si
        /// </summary>
        public static void addVertexToPolygon(EditPolyPolygon p, EditPolyEdge sharedEdge, EditPolyVertex newV)
        {
            for (var i = 0; i < p.vertices.Count; i++)
            {
                if (p.vertices[i].vbIndex == sharedEdge.a.vbIndex)
                {
                    p.vertices.Add(null);
                    for (var j = p.vertices.Count - 2; j >= i + 1; j--)
                    {
                        p.vertices[j + 1] = p.vertices[j];
                    }
                    p.vertices[i + 1] = newV;
                    break;
                }
            }
        }

        /// <summary>
        ///     Indica si dos aristas son iguales
        /// </summary>
        public static bool sameEdge(EditPolyEdge e1, EditPolyEdge e2)
        {
            return (sameVextex(e1.a, e2.a) && sameVextex(e1.b, e2.b))
                   || (sameVextex(e1.a, e2.b) && sameVextex(e1.b, e2.a));
        }

        /// <summary>
        ///     Indica si dos vertices son iguales
        /// </summary>
        /// <returns></returns>
        public static bool sameVextex(EditPolyVertex a, EditPolyVertex b)
        {
            return equalsTGCVector3(a.position, b.position);
        }

        /// <summary>
        ///     Indica si dos TGCVector3 son iguales
        /// </summary>
        public static bool equalsTGCVector3(TGCVector3 a, TGCVector3 b)
        {
            return equalsFloat(a.X, b.X)
                   && equalsFloat(a.Y, b.Y)
                   && equalsFloat(a.Z, b.Z);
        }

        /// <summary>
        ///     Compara que dos floats sean iguales, o casi
        /// </summary>
        public static bool equalsFloat(float f1, float f2)
        {
            return FastMath.Abs(f1 - f2) <= EPSILON;
        }

        /// <summary>
        ///     Compara si dos planos son iguales
        /// </summary>
        public static bool sameTGCPlane(TGCPlane p1, TGCPlane p2)
        {
            //TODO: comparar en ambos sentidos por las dudas
            return equalsTGCVector3(new TGCVector3(p1.A, p1.B, p1.C), new TGCVector3(p2.A, p2.B, p2.C))
                   && equalsFloat(p1.D, p2.D);
        }

        /// <summary>
        ///     Busca si ambos poligonos tienen una arista igual.
        ///     Si encontro retorna el indice de la arista igual de cada poligono.
        /// </summary>
        public static bool findShareEdgeBetweenPolygons(EditPolyPolygon p1, EditPolyPolygon p2, out int p1Edge,
            out int p2Edge)
        {
            for (var i = 0; i < p1.edges.Count; i++)
            {
                for (var j = 0; j < p2.edges.Count; j++)
                {
                    if (sameEdge(p1.edges[i], p2.edges[j]))
                    {
                        p1Edge = i;
                        p2Edge = j;
                        return true;
                    }
                }
            }
            p1Edge = -1;
            p2Edge = -1;
            return false;
        }

        /// <summary>
        ///     Agrega una nueva arista a la lista si es que ya no hay otra igual.
        ///     Devuelve el indice de la nuevo arista o de la que ya estaba.
        /// </summary>
        public static int addEdgeToListIfUnique(List<EditPolyEdge> edges, EditPolyEdge e, out bool newEdgeAdded)
        {
            for (var i = 0; i < edges.Count; i++)
            {
                if (sameEdge(edges[i], e))
                {
                    newEdgeAdded = false;
                    return i;
                }
            }
            newEdgeAdded = true;
            e.faces = new List<EditPolyPolygon>();
            edges.Add(e);
            return edges.Count - 1;
        }

        /// <summary>
        ///     Agrega un nuevo vertice a la lista si es que ya no hay otro igual.
        ///     Devuelve el indice del nuevo vertice o del que ya estaba.
        /// </summary>
        public static int addVertexToListIfUnique(List<EditPolyVertex> vertices, EditPolyVertex v)
        {
            for (var i = 0; i < vertices.Count; i++)
            {
                if (sameVextex(vertices[i], v))
                {
                    return i;
                }
            }
            v.vbIndex = vertices.Count;
            v.edges = new List<EditPolyEdge>();
            vertices.Add(v);
            return v.vbIndex;
        }

        /// <summary>
        ///     Crear BoundingBox a partir de un conjunto de primitivas
        /// </summary>
        /// <param name="list">primitivas</param>
        /// <returns>BoundingBox</returns>
        public static TgcBoundingAxisAlignBox getSelectionBoundingBox(List<EditPolyPrimitive> primitives)
        {
            if (primitives.Count == 0)
                return null;

            var vertices = new TGCVector3[primitives.Count];
            for (var i = 0; i < primitives.Count; i++)
            {
                vertices[i] = primitives[i].computeCenter();
            }
            return TgcBoundingAxisAlignBox.computeFromPoints(vertices);
        }

        public static void updateObbFromSegment(TgcBoundingOrientedBox obb, TGCVector3 a, TGCVector3 b, float thickness)
        {
            var lineDiff = b - a;
            var lineLength = lineDiff.Length();
            var lineVec = TGCVector3.Normalize(lineDiff);

            //Obtener angulo y vector de rotacion
            var upVec = TGCVector3.Up;
            var angle = FastMath.Acos(TGCVector3.Dot(upVec, lineVec));
            var axisRotation = TGCVector3.Cross(upVec, lineVec);
            axisRotation.Normalize();

            //Obtener matriz de rotacion para este eje y angulo
            var rotM = TGCMatrix.RotationAxis(axisRotation, angle);

            //Actualizar orientacion de OBB en base a matriz de rotacion
            obb.Orientation[0] = new TGCVector3(rotM.M11, rotM.M12, rotM.M13);
            obb.Orientation[1] = new TGCVector3(rotM.M21, rotM.M22, rotM.M23);
            obb.Orientation[2] = new TGCVector3(rotM.M31, rotM.M32, rotM.M33);

            //Actualizar extent de OBB segun el thickness del segmento
            obb.Extents = new TGCVector3(thickness, lineLength / 2, thickness);

            //Actualizar centro del OBB segun centro del segmento
            obb.Center = a + TGCVector3.Scale(lineDiff, 0.5f);

            //Regenerar OBB
            obb.updateValues();
        }

        /// <summary>
        ///     Hacer zoom a un grupo de primitivas
        /// </summary>
        public static void zoomPrimitives(MeshCreatorCamera camera, List<EditPolyPrimitive> primitives, TGCMatrix transform)
        {
            var aabb = getSelectionBoundingBox(primitives);
            if (aabb != null)
            {
                camera.CameraCenter = TGCVector3.TransformCoordinate(aabb.calculateBoxCenter(), transform);
            }
        }

        /// <summary>
        ///     Poner la camara en top view respecto de un conjunto de primitivas
        /// </summary>
        public static void setCameraTopView(MeshCreatorCamera camera, List<EditPolyPrimitive> primitives,
            TGCMatrix transform)
        {
            var aabb = getSelectionBoundingBox(primitives);
            TGCVector3 lookAt;
            if (aabb != null)
            {
                lookAt = TGCVector3.TransformCoordinate(aabb.calculateBoxCenter(), transform);
            }
            else
            {
                lookAt = TGCVector3.Empty;
            }
            camera.setFixedView(lookAt, -FastMath.PI_HALF, 0, camera.CameraDistance);
        }

        /// <summary>
        ///     Poner la camara en left view respecto de un conjunto de primitivas
        /// </summary>
        public static void setCameraLeftView(MeshCreatorCamera camera, List<EditPolyPrimitive> primitives,
            TGCMatrix transform)
        {
            var aabb = getSelectionBoundingBox(primitives);
            TGCVector3 lookAt;
            if (aabb != null)
            {
                lookAt = TGCVector3.TransformCoordinate(aabb.calculateBoxCenter(), transform);
            }
            else
            {
                lookAt = TGCVector3.Empty;
            }
            camera.setFixedView(lookAt, 0, FastMath.PI_HALF, camera.CameraDistance);
        }

        /// <summary>
        ///     Poner la camara en front view respecto de un conjunto de primitivas
        /// </summary>
        public static void setCameraFrontView(MeshCreatorCamera camera, List<EditPolyPrimitive> primitives,
            TGCMatrix transform)
        {
            var aabb = getSelectionBoundingBox(primitives);
            TGCVector3 lookAt;
            if (aabb != null)
            {
                lookAt = TGCVector3.TransformCoordinate(aabb.calculateBoxCenter(), transform);
            }
            else
            {
                lookAt = TGCVector3.Empty;
            }
            camera.setFixedView(lookAt, 0, 0, camera.CameraDistance);
        }
    }
}