using System.Windows.Forms;
using TGC.Core.Example;
using TGC.Tools.MeshCreator;
using TGC.Tools.RoomsEditor;
using TGC.Tools.SceneEditor;
using TGC.Tools.TerrainEditor;
using TGC.Tools.UserControls;

namespace TGC.Tools.Example
{
    public abstract class TGCExampleTools : TgcExample
    {
        private readonly Panel modifiersPanel;

        public TGCExampleTools(string mediaDir, string shadersDir, Panel modifiersPanel) : base(mediaDir, shadersDir)
        {
            this.modifiersPanel = modifiersPanel;
        }

        private void AddModifier(UserControl modifier)
        {
            modifier.Dock = DockStyle.Top;
            modifiersPanel.Controls.Add(modifier);
        }

        #region Facilitadores de Modifiers

        public MeshCreatorModifier AddMeshCreatorModifier(TgcMeshCreator tool)
        {
            var modifier = new MeshCreatorModifier(tool);
            AddModifier(modifier);
            return modifier;
        }

        public RoomsEditorModifier AddRoomsEditorModifier(TgcRoomsEditor tool)
        {
            var modifier = new RoomsEditorModifier(tool);
            AddModifier(modifier);
            return modifier;
        }

        public SceneEditorModifier AddSceneEditorModifier(TgcSceneEditor tool)
        {
            var modifier = new SceneEditorModifier(tool);
            AddModifier(modifier);
            return modifier;
        }

        public TerrainEditorModifier AddTerrainEditorModifier(TgcTerrainEditor tool)
        {
            var modifier = new TerrainEditorModifier(tool);
            AddModifier(modifier);
            return modifier;
        }

        public TGCFileModifier AddFile(string varName, string defaultPath, string fileFilter)
        {
            var fileModifier = new TGCFileModifier(varName, defaultPath, fileFilter);
            AddModifier(fileModifier);
            return fileModifier;
        }

        #endregion Facilitadores de Modifiers

        public void ClearModifiers()
        {
            modifiersPanel.Controls.Clear();
        }

        /// <summary>
        ///     Vuelve la configuracion de Render y otras cosas a la configuracion inicial
        /// </summary>
        public override void ResetDefaultConfig()
        {
            base.ResetDefaultConfig();
            ClearModifiers();
        }
    }
}