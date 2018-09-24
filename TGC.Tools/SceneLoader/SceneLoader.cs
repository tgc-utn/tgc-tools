using System.Drawing;
using System.Windows.Forms;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.SceneLoader.Model;
using TGC.Tools.Camara;
using TGC.Tools.Example;
using TGC.Tools.UserControls;

namespace TGC.Tools.SceneLoader
{
    public class SceneLoader : TGCExampleTools
    {
        private TGCFileModifier MeshModifier { get; set; }

        private string CurrentPath { get; set; }

        //Importador de mesh en formato obj
        private TGCObjLoader TgcObjLoader { get; set; } = new TGCObjLoader();

        private TgcMesh MeshDelObj { get; set; }

        public SceneLoader(string mediaDir, string shadersDir, Panel modifiersPanel) : base(mediaDir, shadersDir, modifiersPanel)
        {
            Category = "Mesh Examples";
            Name = "Scene loader";
            Description = "Ejemplo de como cargar una escena estatica en formato OBJ + MTL.";
        }

        public override void Init()
        {
            //CurrentPath = MediaDir + @"Obj\cubotexturacaja.obj";
            CurrentPath = MediaDir + @"Obj\bb8\bb8.obj";
            //CurrentPath = MediaDir + @"Obj\tgcito\tgcito con textura.obj";
            //CurrentPath = MediaDir + @"Obj\tgcito\tgcito color solo.obj";

            MeshModifier = AddFile("Mesh", CurrentPath, "*.obj | *.obj");

            MeshDelObj = TgcObjLoader.LoadTgcMeshFromObj(CurrentPath, 0);

            MeshDelObj.AutoTransform = true;
            MeshDelObj.Scale = new TGCVector3(5f, 5f, 5f);
            MeshDelObj.Position = new TGCVector3(-25, 0, 0);
            MeshDelObj.BoundingBox.move(new TGCVector3(25, 0, 0));

            Camara = new TgcRotationalCamera(MeshDelObj.BoundingBox.calculateBoxCenter(), MeshDelObj.BoundingBox.calculateBoxRadius() * 2, Input);
        }

        public override void Update()
        {
            PreUpdate();
            PostUpdate();
        }

        public override void Render()
        {
            PreRender();

            DrawText.drawText("Buscar y abrir un archivo obj solo o uno con mtl.", 0, 20, Color.OrangeRed);

            //Ver si cambio el mesh
            var selectedPath = MeshModifier.Value;
            if (CurrentPath != selectedPath)
            {
                //TODO si utilizo el mismo Builder falla porque no limpia el mtl viejo.
                TgcObjLoader = new TGCObjLoader();
                MeshDelObj = TgcObjLoader.LoadTgcMeshFromObj(selectedPath, 0);
                CurrentPath = selectedPath;
            }

            MeshDelObj.Render();

            PostRender();
        }

        public override void Dispose()
        {
            MeshDelObj.Dispose();
        }
    }
}