using Microsoft.DirectX.Direct3D;
using System.Drawing;
using System.Windows.Forms;
using TGC.Core.Direct3D;
using TGC.Tools.Example;
using TGC.Tools.UserControls;

namespace TGC.Tools.MeshCreator
{
    /// <summary>
    ///     Ejemplo TgcMeshCreator:
    ///     Unidades Involucradas:
    ///     # Unidad 3 - Conceptos Básicos de 3D - Mesh, Transformaciones, GameEngine
    ///     # Unidad 6 - Detección de Colisiones - BoundingBox, Picking
    ///     Herramienta para crear modelos 3D en base a primitivas simples.
    ///     Permite luego exportarlos a un un XML de formato TgcScene.
    ///     El ejemplo crea su propio Modifier con todos los controles visuales de .NET que necesita.
    ///     Autor: Matías Leone, Leandro Barbagallo
    /// </summary>
    public class TgcMeshCreator : TGCExampleTools
    {
        private MeshCreatorModifier Modifier { get; set; }

        public TgcMeshCreator(string mediaDir, string shadersDir, Panel modifiersPanel) : base(mediaDir, shadersDir, modifiersPanel)
        {
            Category = "Utils";
            Name = "MeshCreator";
            Description = "Creador de objetos 3D a paritir de primitivas simples. " +
                          "Luego se puede exportar a un archivo XML de formato TgcScene para su posterior uso.";
        }

        public override void Init()
        {
            //Crear Modifier especial para este editor
            Modifier = AddMeshCreatorModifier(this);
        }

        public override void Update()
        {
            PreUpdate();
            PostUpdate();
        }

        public override void Render()
        {
            PreRender();

            //Color de fondo, TGCExample deberia aceptar un BackgroundColor para evitar esto en cada Render, ya que en BeginRenderScene tambien se hace.
            D3DDevice.Instance.Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.FromArgb(38, 38, 38), 1.0f, 0);

            //Delegar Render al control
            Modifier.Render();

            PostRender();
        }

        public override void Dispose()
        {
            //Delegar al control
            Modifier.DisposeElements();
        }
    }
}