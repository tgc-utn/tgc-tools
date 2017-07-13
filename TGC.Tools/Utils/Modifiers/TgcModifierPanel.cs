using System;
using System.Drawing;
using System.Windows.Forms;
using TGC.Tools.Model;

namespace TGC.Tools.Utils.Modifiers
{
    /// <summary>
    ///     Panel generico para un Modifier
    /// </summary>
    public abstract class TgcModifierPanel
    {
        protected Panel contentPanel;
        protected Panel mainPanel;

        protected Label title;
        protected Panel titleBar;

        public TgcModifierPanel(string varName)
        {
            VarName = varName;

            title = new Label();
            title.AutoSize = true;
            title.Text = varName;
            title.TextAlign = ContentAlignment.MiddleLeft;
            title.Name = "title";
            title.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);

            titleBar = new Panel();
            titleBar.Padding = new Padding(3);
            titleBar.AutoSize = true;
            titleBar.Dock = DockStyle.Top;
            titleBar.Name = "titleBar";
            titleBar.BackColor = SystemColors.ButtonShadow;
            titleBar.BorderStyle = BorderStyle.FixedSingle;

            titleBar.Controls.Add(title);

            contentPanel = new Panel();
            contentPanel.Padding = new Padding(3);
            contentPanel.AutoSize = true;
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.Name = "contentPanel";

            mainPanel = new Panel();
            mainPanel.Padding = new Padding(3);
            mainPanel.AutoSize = true;
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.BorderStyle = BorderStyle.FixedSingle;

            
            mainPanel.Controls.Add(contentPanel);
            mainPanel.Controls.Add(titleBar);
        }

        /// <summary>
        ///     Control gráfico principal del Modifier
        /// </summary>
        public Panel MainPanel
        {
            get { return mainPanel; }
        }

        /// <summary>
        ///     Nombre de la variable del Modifier
        /// </summary>
        public string VarName { get; }

        /// <summary>
        ///     Devuelve el valor del variable del modificador.
        ///     Se debe castear al tipo que corresponda.
        /// </summary>
        public abstract object getValue();
    }
}