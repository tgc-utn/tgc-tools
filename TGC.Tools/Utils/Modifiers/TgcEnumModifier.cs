using System;
using System.Windows.Forms;

namespace TgcViewer.Utils.Modifiers
{
    /// <summary>
    /// Modificador para un intervalo discreto de valores creados con una estructura Enum
    /// </summary>
    public class TgcEnumModifier : TgcModifierPanel
    {
        private ComboBox comboBox;
        private Type enumType;
        private object selectValue;

        public TgcEnumModifier(string varName, Type enumType, object defaultValue)
            : base(varName)
        {
            this.enumType = enumType;
            this.selectValue = defaultValue;

            //Buscar indice del defaultValue dentro del enum
            string[] enumNames = Enum.GetNames(enumType);
            string currentName = Enum.GetName(enumType, defaultValue);
            int selectedIndex = -1;
            for (int i = 0; i < enumNames.Length; i++)
            {
                if (currentName.Equals(enumNames[i]))
                {
                    selectedIndex = i;
                    break;
                }
            }

            comboBox = new ComboBox();
            comboBox.Margin = new Padding(0);
            comboBox.Size = new System.Drawing.Size(100, 20);
            comboBox.Items.AddRange(enumNames);
            comboBox.SelectedIndex = selectedIndex;
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox.SelectionChangeCommitted += new EventHandler(comboBox_SelectionChangeCommitted);

            contentPanel.Controls.Add(comboBox);
        }

        /// <summary>
        /// Cuando el valor del combo cambia y fue confirmado realmente
        /// </summary>
        private void comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int selectedIndex = comboBox.SelectedIndex;
            string itemString = (string)comboBox.Items[selectedIndex];
            selectValue = Enum.Parse(enumType, itemString);
        }

        public override object getValue()
        {
            return selectValue;
        }
    }
}