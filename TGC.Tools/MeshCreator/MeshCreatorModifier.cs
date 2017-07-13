using System.Windows.Forms;
using TGC.Tools.Utils.Modifiers;

namespace TGC.Tools.MeshCreator
{
    /// <summary>
    ///     Modifier customizado que se utiliza en el MeshCreator
    /// </summary>
    public class MeshCreatorModifier : TgcModifierPanel
    {
        public MeshCreatorModifier(string varName, TgcMeshCreator creator)
            : base(varName)
        {
            Control = new MeshCreatorControl(creator);
            Control.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(Control);
        }

        public MeshCreatorControl Control { get; }

        public override object getValue()
        {
            return null;
        }

        public void dispose()
        {
            Control.close();
        }
    }
}