using Microsoft.DirectX.Direct3D;
using System.Drawing;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using TGC.Core.Shaders;
using TGC.Core.Textures;

namespace TGC.Tools.MeshCreator.EditablePoly
{
    /// <summary>
    ///     Herramienta para acumular varios triangulos y dibujarlos luego todos juntos
    /// </summary>
    public class TrianglesBatchRenderer
    {
        private const int BATCH_SIZE = 1200;
        private readonly TGCVector3 BOX_LINE_ORIGINAL_DIR = new TGCVector3(0, 1, 0);
        private readonly CustomVertex.PositionColored[] vertices;
        private int idx;

        public TrianglesBatchRenderer()
        {
            vertices = new CustomVertex.PositionColored[BATCH_SIZE];
            reset();
        }

        /// <summary>
        ///     Reiniciar batch
        /// </summary>
        public void reset()
        {
            idx = 0;
        }

        /// <summary>
        ///     Agregar triangulo al batch
        /// </summary>
        public void addTriangle(CustomVertex.PositionColored a, CustomVertex.PositionColored b,
            CustomVertex.PositionColored c)
        {
            vertices[idx] = a;
            vertices[idx + 1] = b;
            vertices[idx + 2] = c;
            idx += 3;
        }

        /// <summary>
        ///     Agregar triangulo al batch
        /// </summary>
        public void addTriangle(TGCVector3 a, TGCVector3 b, TGCVector3 c, Color color)
        {
            var cInt = color.ToArgb();
            addTriangle(
                new CustomVertex.PositionColored(a, cInt),
                new CustomVertex.PositionColored(b, cInt),
                new CustomVertex.PositionColored(c, cInt));
        }

        /// <summary>
        ///     Indica si hay espacio suficiente para agregar la cantidad de vertices deseada
        /// </summary>
        public bool hasSpace(int vertexCount)
        {
            return idx + vertexCount < BATCH_SIZE;
        }

        /// <summary>
        ///     Dibujar batch de triangulos hasta donde se haya cargado
        /// </summary>
        public void render()
        {
            var d3dDevice = D3DDevice.Instance.Device;
            var texturesManager = TexturesManager.Instance;

            texturesManager.clear(0);
            texturesManager.clear(1);

            var effect = TgcShaders.Instance.VariosShader;
            TgcShaders.Instance.setShaderMatrixIdentity(effect);
            d3dDevice.VertexDeclaration = TgcShaders.Instance.VdecPositionColored;
            effect.Technique = TgcShaders.T_POSITION_COLORED;

            //Alpha blend on
            d3dDevice.RenderState.AlphaTestEnable = true;
            d3dDevice.RenderState.AlphaBlendEnable = true;

            //Render con shader
            effect.Begin(0);
            effect.BeginPass(0);
            d3dDevice.DrawUserPrimitives(PrimitiveType.TriangleList, idx / 3, vertices);
            effect.EndPass();
            effect.End();

            //Alpha blend off
            d3dDevice.RenderState.AlphaTestEnable = false;
            d3dDevice.RenderState.AlphaBlendEnable = false;
        }

        /// <summary>
        ///     Si no queda lugar para agregar los vertices que se quiere entonces se dibuja y vacia el buffer
        /// </summary>
        public void checkAndFlush(int vertexCount)
        {
            if (!hasSpace(vertexCount))
            {
                render();
                reset();
            }
        }

        /// <summary>
        ///     Add new BoxLine mesh
        /// </summary>
        public void addBoxLine(TGCVector3 pStart, TGCVector3 pEnd, float thickness, Color color)
        {
            const int vertexCount = 36;
            checkAndFlush(vertexCount);
            var initIdx = idx;

            var c = color.ToArgb();

            //Crear caja en vertical en Y con longitud igual al módulo de la recta.
            var lineVec = TGCVector3.Subtract(pEnd, pStart);
            var lineLength = lineVec.Length();
            var min = new TGCVector3(-thickness, 0, -thickness);
            var max = new TGCVector3(thickness, lineLength, thickness);

            //Vértices de la caja con forma de linea
            // Front face
            addTriangle(
                new CustomVertex.PositionColored(min.X, max.Y, max.Z, c),
                new CustomVertex.PositionColored(min.X, min.Y, max.Z, c),
                new CustomVertex.PositionColored(max.X, max.Y, max.Z, c)
                );
            addTriangle(
                new CustomVertex.PositionColored(min.X, min.Y, max.Z, c),
                new CustomVertex.PositionColored(max.X, min.Y, max.Z, c),
                new CustomVertex.PositionColored(max.X, max.Y, max.Z, c)
                );

            // Back face (remember this is facing *away* from the camera, so vertices should be clockwise order)
            addTriangle(
                new CustomVertex.PositionColored(min.X, max.Y, min.Z, c),
                new CustomVertex.PositionColored(max.X, max.Y, min.Z, c),
                new CustomVertex.PositionColored(min.X, min.Y, min.Z, c)
                );
            addTriangle(
                new CustomVertex.PositionColored(min.X, min.Y, min.Z, c),
                new CustomVertex.PositionColored(max.X, max.Y, min.Z, c),
                new CustomVertex.PositionColored(max.X, min.Y, min.Z, c)
                );

            // Top face
            addTriangle(
                new CustomVertex.PositionColored(min.X, max.Y, max.Z, c),
                new CustomVertex.PositionColored(max.X, max.Y, min.Z, c),
                new CustomVertex.PositionColored(min.X, max.Y, min.Z, c)
                );
            addTriangle(
                new CustomVertex.PositionColored(min.X, max.Y, max.Z, c),
                new CustomVertex.PositionColored(max.X, max.Y, max.Z, c),
                new CustomVertex.PositionColored(max.X, max.Y, min.Z, c)
                );

            // Bottom face (remember this is facing *away* from the camera, so vertices should be clockwise order)
            addTriangle(
                new CustomVertex.PositionColored(min.X, min.Y, max.Z, c),
                new CustomVertex.PositionColored(min.X, min.Y, min.Z, c),
                new CustomVertex.PositionColored(max.X, min.Y, min.Z, c)
                );
            addTriangle(
                new CustomVertex.PositionColored(min.X, min.Y, max.Z, c),
                new CustomVertex.PositionColored(max.X, min.Y, min.Z, c),
                new CustomVertex.PositionColored(max.X, min.Y, max.Z, c)
                );

            // Left face
            addTriangle(
                new CustomVertex.PositionColored(min.X, max.Y, max.Z, c),
                new CustomVertex.PositionColored(min.X, min.Y, min.Z, c),
                new CustomVertex.PositionColored(min.X, min.Y, max.Z, c)
                );
            addTriangle(
                new CustomVertex.PositionColored(min.X, max.Y, min.Z, c),
                new CustomVertex.PositionColored(min.X, min.Y, min.Z, c),
                new CustomVertex.PositionColored(min.X, max.Y, max.Z, c)
                );

            // Right face (remember this is facing *away* from the camera, so vertices should be clockwise order)
            addTriangle(
                new CustomVertex.PositionColored(max.X, max.Y, max.Z, c),
                new CustomVertex.PositionColored(max.X, min.Y, max.Z, c),
                new CustomVertex.PositionColored(max.X, min.Y, min.Z, c)
                );
            addTriangle(
                new CustomVertex.PositionColored(max.X, max.Y, min.Z, c),
                new CustomVertex.PositionColored(max.X, max.Y, max.Z, c),
                new CustomVertex.PositionColored(max.X, min.Y, min.Z, c)
                );

            //Obtener matriz de rotacion respecto del vector de la linea
            lineVec.Normalize();
            var angle = FastMath.Acos(TGCVector3.Dot(BOX_LINE_ORIGINAL_DIR, lineVec));
            var axisRotation = TGCVector3.Cross(BOX_LINE_ORIGINAL_DIR, lineVec);
            axisRotation.Normalize();
            var t = TGCMatrix.RotationAxis(axisRotation, angle) * TGCMatrix.Translation(pStart);

            //Transformar todos los puntos
            for (var i = initIdx; i < initIdx + vertexCount; i++)
            {
                vertices[i].Position = TGCVector3.TransformCoordinate(TGCVector3.FromVector3(vertices[i].Position), t);
            }
        }

        public void dispose()
        {
        }
    }
}