using System.Windows.Forms;
using System.Xml;
using TGC.Core.Mathematica;
using TGC.Tools.Example;
using TGC.Tools.UserControls;

namespace TGC.Tools.SceneEditor
{
    /// <summary>
    ///     Ejemplo TgcSceneEditor:
    ///     Unidades Involucradas:
    ///     # Unidad 3 - Conceptos B�sicos de 3D - Mesh, Transformaciones, GameEngine
    ///     # Unidad 6 - Detecci�n de Colisiones - BoundingBox, Picking
    ///     # Unidad 7 - T�cnicas de Optimizaci�n - Frustum Culling
    ///     Ejemplo que muestra como crear un editor de escenarios.
    ///     Permite cargar varias mallas estaticas, moverlas, rotarlas y escalarlas.
    ///     Tambi�n permite crear un terreno.
    ///     El ejemplo crea su propio Modifier con todos los controles visuales de .NET que necesita.
    ///     Luego toda la informaci�n puede ser exportada a un archivo para su posterior uso.
    ///     Se utiliza el m�todo "exportScene()" para grabar la informaci�n de la escena en un XML de formato TgcScene.
    ///     Las instrucciones se muestran al hacer clic en el bot�n "Help" de este Modifier.
    ///     Autor: Mat�as Leone, Leandro Barbagallo
    /// </summary>
    public class TgcSceneEditor : TGCExampleTools
    {
        private SceneEditorModifier Modifier { get; set; }

        public TgcSceneEditor(string mediaDir, string shadersDir, Panel modifiersPanel) : base(mediaDir, shadersDir, modifiersPanel)
        {
            Category = "Utils";
            Name = "SceneEditor";
            Description = "Editor de escena. Permite abrir modelos en formato TGC y posicionarlos dentro de un escenario." +
                          "Luego esa informaci�n se puede exportar a un archivo XML para su posterior uso.";
        }

        public override void Init()
        {
            //Crear Modifier especial para este editor
            Modifier = AddSceneEditorModifier(this);
        }

        public override void Update()
        {
            PreUpdate();
            PostUpdate();
        }

        public override void Render()
        {
            PreRender();

            //Delegar render al control
            Modifier.render();

            PostRender();
        }

        public override void Dispose()
        {
            //Delegar al control
            Modifier.close();
        }

        /// <summary>
        ///     M�todo que se llama cuando se quiere exportar la informacion de la escena a un XML.
        ///     MODIFICAR ESTA SECCION PARA ADAPTARSE A LAS NECESIDADES DEL ALUMNO
        /// </summary>
        internal void exportScene(string savePath)
        {
            //Crea XML
            var doc = new XmlDocument();
            XmlNode root = doc.CreateElement("SceneEditor-Export");

            //Guardar info del terreno
            var terrainNode = doc.CreateElement("Terrain");
            var terrain = Modifier.TgcTerrain;
            if (terrain != null)
            {
                terrainNode.SetAttribute("enable", true.ToString());
                terrainNode.SetAttribute("heightmap", Modifier.heighmap.Text);
                terrainNode.SetAttribute("texture", Modifier.terrainTexture.Text);
                terrainNode.SetAttribute("xzScale", Modifier.terrainXZscale.Value.ToString());
                terrainNode.SetAttribute("yScale", Modifier.terrainYscale.Value.ToString());
                terrainNode.SetAttribute("center", TGCVector3.PrintVector3FromString(
                    Modifier.terrainCenterX.Text,
                    Modifier.terrainCenterY.Text,
                    Modifier.terrainCenterZ.Text));
            }
            else
            {
                terrainNode.SetAttribute("enable", false.ToString());
            }
            root.AppendChild(terrainNode);

            //Recorrer Meshes del escenario, ordenadas por grupo
            var meshObjects = Modifier.getMeshObjectsOrderByGroup();
            var meshesNode = doc.CreateElement("Meshes");
            var groupIndex = -1;
            XmlElement lastGroupNode = null;
            foreach (var meshObject in meshObjects)
            {
                //Crear grupo con corte de control
                if (meshObject.groupIndex > groupIndex)
                {
                    groupIndex = meshObject.groupIndex;
                    lastGroupNode = doc.CreateElement("MeshGroup");
                    lastGroupNode.SetAttribute("groupIndex", groupIndex.ToString());
                    meshesNode.AppendChild(lastGroupNode);
                }

                //Guardar info de mesh
                var mesh = meshObject.mesh;
                var meshNode = doc.CreateElement("Mesh");
                meshNode.SetAttribute("name", meshObject.name);
                meshNode.SetAttribute("index", meshObject.index.ToString());
                meshNode.SetAttribute("file", meshObject.fileName);
                meshNode.SetAttribute("folder", meshObject.folderName);
                meshNode.SetAttribute("position", TGCVector3.PrintVector3(mesh.Position));
                meshNode.SetAttribute("rotation", TGCVector3.PrintVector3(mesh.Rotation));
                meshNode.SetAttribute("scale", TGCVector3.PrintVector3(mesh.Scale));
                meshNode.SetAttribute("userInfo", meshObject.userInfo);

                lastGroupNode.AppendChild(meshNode);
            }
            root.AppendChild(meshesNode);

            //Guardar XML
            doc.AppendChild(root);
            doc.Save(savePath);
        }
    }
}