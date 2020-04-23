using System.Drawing;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;

namespace TGC.Tools.MeshCreator.EditablePoly
{
    /// <summary>
    /// Herramienta para renderizar las primitivas de Editable Poly
    /// </summary>
    public class PrimitiveRenderer
    {
        private readonly Color SELECTED_POLYGON_COLOR = Color.FromArgb(120, 255, 0, 0);
        private readonly TrianglesBatchRenderer batchRenderer;

        private readonly EditablePoly editablePoly;
        private readonly TGCBox selectedVertexBox;
        private readonly TGCBox vertexBox;

        public PrimitiveRenderer(EditablePoly editablePoly)
        {
            this.editablePoly = editablePoly;
            batchRenderer = new TrianglesBatchRenderer();

            vertexBox = TGCBox.fromSize(TGCVector3.One, Color.Blue);
            vertexBox.AutoTransformEnable = true;
            selectedVertexBox = TGCBox.fromSize(TGCVector3.One, Color.Red);
            selectedVertexBox.AutoTransformEnable = true;
        }

        /// <summary>
        /// Dibujar primitivas
        /// </summary>
        /// <param name="transform">Transform TGCMatrix del mesh</param>
        public void render(TGCMatrix transform)
        {
            switch (editablePoly.CurrentPrimitive)
            {
                case EditablePoly.PrimitiveType.Vertex:
                    renderVertices(transform);
                    break;

                case EditablePoly.PrimitiveType.Edge:
                    renderEdges(transform);
                    break;

                case EditablePoly.PrimitiveType.Polygon:
                    renderPolygons(transform);
                    break;
            }
        }

        /// <summary>
        /// Dibujar vertices
        /// </summary>
        private void renderVertices(TGCMatrix transform)
        {
            foreach (var v in editablePoly.Vertices)
            {
                var pos = TGCVector3.TransformCoordinate(v.position, transform);
                var box = v.Selected ? selectedVertexBox : vertexBox;
                box.Position = pos /*+ new TGCVector3(0.5f, 0.5f, 0.5f)*/;
                box.Render();
            }
        }

        /// <summary>
        /// Dibujar poligonos
        /// </summary>
        private void renderPolygons(TGCMatrix transform)
        {
            batchRenderer.reset();

            //Edges
            foreach (var e in editablePoly.Edges)
            {
                var a = TGCVector3.TransformCoordinate(e.a.position, transform);
                var b = TGCVector3.TransformCoordinate(e.b.position, transform);
                batchRenderer.addBoxLine(a, b, 0.06f, e.Selected ? Color.Red : Color.Blue);
            }

            //Selected polygons (as polygon meshes)
            foreach (var p in editablePoly.Polygons)
            {
                if (p.Selected)
                {
                    /*
                    TGCVector3 n = new TGCVector3(p.TGCPlane.A, p.TGCPlane.B, p.TGCPlane.C) * 0.1f;
                    TGCVector3 v0 = TGCVector3.TransformCoordinate(p.vertices[0].position, transform);
                    TGCVector3 v1 = TGCVector3.TransformCoordinate(p.vertices[1].position, transform);
                    for (int i = 2; i < p.vertices.Count; i++)
                    {
                        batchRenderer.checkAndFlush(6);
                        TGCVector3 v2 = TGCVector3.TransformCoordinate(p.vertices[i].position, transform);
                        batchRenderer.addTriangle(v0 + n, v1 + n, v2 + n, SELECTED_POLYGON_COLOR);
                        batchRenderer.addTriangle(v0 - n, v1 - n, v2 - n, SELECTED_POLYGON_COLOR);
                        v1 = v2;
                    }
                     */
                    var n = new TGCVector3(p.TGCPlane.A, p.TGCPlane.B, p.TGCPlane.C) * 0.1f;
                    for (var i = 0; i < p.vbTriangles.Count; i++)
                    {
                        var triIdx = p.vbTriangles[i];
                        var v0 =
                            TGCVector3.TransformCoordinate(
                                editablePoly.Vertices[editablePoly.IndexBuffer[triIdx]].position, transform);
                        var v1 =
                            TGCVector3.TransformCoordinate(
                                editablePoly.Vertices[editablePoly.IndexBuffer[triIdx + 1]].position, transform);
                        var v2 =
                            TGCVector3.TransformCoordinate(
                                editablePoly.Vertices[editablePoly.IndexBuffer[triIdx + 2]].position, transform);

                        batchRenderer.checkAndFlush(6);
                        batchRenderer.addTriangle(v0 + n, v1 + n, v2 + n, SELECTED_POLYGON_COLOR);
                        batchRenderer.addTriangle(v0 - n, v1 - n, v2 - n, SELECTED_POLYGON_COLOR);
                    }
                }
            }
            //Vaciar todo lo que haya
            batchRenderer.render();
        }

        /// <summary>
        /// Dibujar aristas
        /// </summary>
        private void renderEdges(TGCMatrix transform)
        {
            batchRenderer.reset();

            foreach (var e in editablePoly.Edges)
            {
                var a = TGCVector3.TransformCoordinate(e.a.position, transform);
                var b = TGCVector3.TransformCoordinate(e.b.position, transform);
                batchRenderer.addBoxLine(a, b, 0.12f, e.Selected ? Color.Red : Color.Blue);
            }
            //Vaciar todo lo que haya
            batchRenderer.render();
        }

        public void dispose()
        {
            vertexBox.Dispose();
            selectedVertexBox.Dispose();
            batchRenderer.dispose();
        }
    }
}