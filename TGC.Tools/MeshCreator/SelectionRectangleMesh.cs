using Microsoft.DirectX.Direct3D;
using System.Drawing;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using TGC.Tools.Model;

namespace TGC.Tools.MeshCreator
{
    /// <summary>
    ///     Mesh para dibujar el rectangulo de seleccion 2D en pantalla
    /// </summary>
    public class SelectionRectangleMesh
    {
        private static readonly int RECT_COLOR = Color.White.ToArgb();

        private CustomVertex.TransformedColored[] vertices;

        public SelectionRectangleMesh()
        {
            vertices = new CustomVertex.TransformedColored[8];
        }

        /// <summary>
        ///     Actualizar mesh del recuadro de seleccion
        /// </summary>
        public void updateMesh(TGCVector2 min, TGCVector2 max)
        {
            //Horizontal arriba
            vertices[0] = new CustomVertex.TransformedColored(min.X, min.Y, 0, 1, RECT_COLOR);
            vertices[1] = new CustomVertex.TransformedColored(max.X, min.Y, 0, 1, RECT_COLOR);

            //Horizontal abajo
            vertices[2] = new CustomVertex.TransformedColored(min.X, max.Y, 0, 1, RECT_COLOR);
            vertices[3] = new CustomVertex.TransformedColored(max.X, max.Y, 0, 1, RECT_COLOR);

            //Vertical izquierda
            vertices[4] = new CustomVertex.TransformedColored(min.X, min.Y, 0, 1, RECT_COLOR);
            vertices[5] = new CustomVertex.TransformedColored(min.X, max.Y, 0, 1, RECT_COLOR);

            //Vertical derecha
            vertices[6] = new CustomVertex.TransformedColored(max.X, min.Y, 0, 1, RECT_COLOR);
            vertices[7] = new CustomVertex.TransformedColored(max.X, max.Y, 0, 1, RECT_COLOR);
        }

        /// <summary>
        ///     Dibujar recuadro
        /// </summary>
        public void render()
        {
            var d3dDevice = ToolsModel.Instance.D3dDevice;
            var texturesManager = ToolsModel.Instance.TexturesManager;

            texturesManager.clear(0);
            texturesManager.clear(1);
            d3dDevice.Material = D3DDevice.DEFAULT_MATERIAL;
            d3dDevice.Transform.World = TGCMatrix.Identity;

            d3dDevice.VertexFormat = CustomVertex.TransformedColored.Format;
            d3dDevice.DrawUserPrimitives(PrimitiveType.LineList, 4, vertices);
        }

        public void dipose()
        {
            vertices = null;
        }
    }
}