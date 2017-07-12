using TGC.Tools.Utils.Modifiers;

namespace TGC.Tools.SceneEditor
{
    /// <summary>
    ///     Modifier customizado que se utiliza en el SceneEditor
    /// </summary>
    public class SceneEditorModifier : TgcModifierPanel
    {
        public SceneEditorModifier(string varName, TgcSceneEditor editor)
            : base(varName)
        {
            EditorControl = new SceneEditorControl(editor);
            contentPanel.Controls.Add(EditorControl);
        }

        public SceneEditorControl EditorControl { get; }

        public override object getValue()
        {
            return null;
        }
    }
}