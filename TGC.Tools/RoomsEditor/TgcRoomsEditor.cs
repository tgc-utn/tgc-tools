using System.Windows.Forms;
using TGC.Core.Mathematica;
using TGC.Tools.Camara;
using TGC.Tools.Example;
using TGC.Tools.UserControls;

namespace TGC.Tools.RoomsEditor
{
    /// <summary>
    ///     Ejemplo Caja
    ///     Unidades Involucradas:
    ///     # Unidad 3 - Conceptos Básicos de 3D - Mesh
    ///     # Unidad 7 - Técnicas de Optimización - Indoor
    ///     Herramienta para crear escenarios Indoor compuestos por cuartos rectangulares
    ///     que se comunican entre sí.
    ///     Permite crear cuartos rectangulares en un plano 2D con vista superior y, a partir
    ///     de este plano, genera el escenario 3D.
    ///     Calcula automáticamente los lados de los rectángulos que tocan entre sí y genera
    ///     las aberturas necesarias que permien comunicar ambos cuartos (simulando puertas o ventanas)
    ///     Las instrucciones se muestran al hacer clic en el botón "Help" de este Modifier.
    ///     Autor: Matías Leone, Leandro Barbagallo
    /// </summary>
    public class TgcRoomsEditor : TGCExampleTools
    {
        private RoomsEditorModifier modifier;

        public TgcRoomsEditor(string mediaDir, string shadersDir, Panel modifiersPanel) : base(mediaDir, shadersDir, modifiersPanel)
        {
            Category = "Utils";
            Name = "RoomsEditor";
            Description = "Herramienta para crear escenarios Indoor compuestos por cuartos rectangulares que se comunican entre sí.";
        }

        public override void Init()
        {
            var fpsCamara = new TgcFpsCamera(Input);
            fpsCamara.MovementSpeed = 200f;
            fpsCamara.JumpSpeed = 200f;
            fpsCamara.SetCamera(new TGCVector3(133.0014f, 264.8258f, -119.0311f), new TGCVector3(498.1584f, -299.4199f, 621.433f));
            Camera = fpsCamara;

            //Crear Modifier especial para este editor
            modifier = AddRoomsEditorModifier(this);
        }

        public override void Update()
        {
            PreUpdate();
            PostUpdate();
        }

        public override void Render()
        {
            PreRender();

            foreach (var room in modifier.Rooms)
            {
                foreach (var wall in room.Walls)
                {
                    wall.render();
                }
            }

            PostRender();
        }

        public override void Dispose()
        {
            modifier.Dispose();
        }

        /// <summary>
        ///     Método que se llama cuando se quiere exportar la informacion de la escena a un XML,
        ///     a través del botón "Custom Export"
        ///     MODIFICAR ESTA SECCION PARA ADAPTARSE A LAS NECESIDADES DEL ALUMNO
        /// </summary>
        internal void customExport(string savePath)
        {
        }
    }
}