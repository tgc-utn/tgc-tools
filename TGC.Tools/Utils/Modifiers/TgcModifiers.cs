using System.Collections.Generic;
using System.Windows.Forms;

namespace TGC.Tools.Utils.Modifiers
{
    public class TgcModifiers
    {
        private readonly Dictionary<string, TgcModifierPanel> modifiers;
        private readonly Panel modifiersPanel;

        public TgcModifiers(Panel modifiersPanel)
        {
            this.modifiersPanel = modifiersPanel;
            modifiers = new Dictionary<string, TgcModifierPanel>();
        }

        public object this[string varName]
        {
            get { return modifiers[varName].getValue(); }
        }

        public void add(TgcModifierPanel modifier)
        {
            if (modifiersPanel.Controls.Count > 0)
            {
                modifiersPanel.Controls.RemoveAt(modifiersPanel.Controls.Count - 1);
            }

            modifiers.Add(modifier.VarName, modifier);
            modifiersPanel.Controls.Add(modifier.MainPanel);
        }

        public object getValue(string varName)
        {
            return modifiers[varName].getValue();
        }

        public void clear()
        {
            modifiersPanel.Controls.Clear();
            modifiers.Clear();
        }
    }
}