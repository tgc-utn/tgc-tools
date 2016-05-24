using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;
using TGC.Tools.MeshCreator;
using TGC.Tools.Model;
using TGC.Tools.RoomsEditor;
using TGC.Tools.SceneEditor;
using TGC.Tools.TerrainEditor;

namespace TGC.Tools.Forms
{
    /// <summary>
    ///     Formulario principal de la aplicación
    /// </summary>
    public partial class ToolsForm : Form
    {
        /// <summary>
        ///     Constructor principal de la aplicacion
        /// </summary>
        public ToolsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Obtener o parar el estado del RenderLoop.
        /// </summary>
        public static bool ApplicationRunning { get; set; }

        /// <summary>
        ///     Mostrar posicion de camara
        /// </summary>
        internal bool MostrarPosicionDeCamaraEnable
        {
            get { return mostrarPosicionDeCamaraToolStripMenuItem.Checked; }
            set { mostrarPosicionDeCamaraToolStripMenuItem.Checked = value; }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //Show the App before we init
            Show();
            panel3d.Focus();

            ApplicationRunning = true;
            GuiController.newInstance();
            var guiController = GuiController.Instance;
            guiController.initGraphics(this, panel3d);

            while (ApplicationRunning)
            {
                //Solo renderizamos si la aplicacion tiene foco, para no consumir recursos innecesarios
                if (applicationActive())
                {
                    guiController.render();
                }
                else
                {
                    //Si no tenemos el foco, dormir cada tanto para no consumir gran cantida de CPU
                    Thread.Sleep(100);
                }

                // Process application messages
                Application.DoEvents();
            }
        }

        /// <summary>
        ///     Finalizar aplicacion
        /// </summary>
        private void shutDown()
        {
            ApplicationRunning = false;
            //GuiController.Instance.shutDown();
            //Application.Exit();

            //Matar proceso principal a la fuerza
            var currentProcess = Process.GetCurrentProcess();
            currentProcess.Kill();
        }

        /// <summary>
        ///     Cerrando el formulario
        /// </summary>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            shutDown();
        }

        /// <summary>
        ///     Texto de ToolStripStatusPosition
        /// </summary>
        public void setStatusPosition(string text)
        {
            toolStripStatusPosition.Text = text;
        }

        /// <summary>
        ///     Texto de ToolStripStatusCurrentExample
        /// </summary>
        /// <param name="text"></param>
        public void setCurrentExampleStatus(string text)
        {
            toolStripStatusCurrentExample.Text = text;
        }

        /// <summary>
        ///     Panel de Modifiers
        /// </summary>
        /// <returns></returns>
        internal Panel getModifiersPanel()
        {
            return flowLayoutPanelModifiers;
        }

        private void wireframeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (wireframeToolStripMenuItem.Checked)
            {
                GuiController.Instance.D3dDevice.RenderState.FillMode = FillMode.WireFrame;
            }
            else
            {
                GuiController.Instance.D3dDevice.RenderState.FillMode = FillMode.Solid;
            }
        }

        private void contadorFPSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GuiController.Instance.FpsCounterEnable = contadorFPSToolStripMenuItem.Checked;
        }

        private void ejesCartesianosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GuiController.Instance.AxisLines.Enable = ejesCartesianosToolStripMenuItem.Checked;
        }

        /// <summary>
        ///     Setea los valores default de las opciones del menu
        /// </summary>
        internal void resetMenuOptions()
        {
            wireframeToolStripMenuItem.Checked = false;
            contadorFPSToolStripMenuItem.Checked = true;
            ejesCartesianosToolStripMenuItem.Checked = true;
        }

        private void mostrarPosiciónDeCámaraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!mostrarPosicionDeCamaraToolStripMenuItem.Checked)
            {
                toolStripStatusPosition.Text = "";
            }
        }

        /// <summary>
        ///     Indica si la aplicacion esta activa.
        ///     Busca si la ventana principal tiene foco o si alguna de sus hijas tiene.
        /// </summary>
        private bool applicationActive()
        {
            if (ContainsFocus) return true;
            foreach (var form in OwnedForms)
            {
                if (form.ContainsFocus) return true;
            }
            return false;
        }

        private void meshCreatorToolStripButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Seguro que desea cambiar de herramienta?", "Confirmación",
                MessageBoxButtons.YesNo);

            if (result.Equals(DialogResult.Yes))
            {
                try
                {
                    GuiController.Instance.executeSelectedExample(new TgcMeshCreator());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error en init() de Mesh Creator \n" + ex.Message, ProductName,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void roomEditorToolStripButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Seguro que desea cambiar de herramienta?", "Confirmación",
                MessageBoxButtons.YesNo);

            if (result.Equals(DialogResult.Yes))
            {
                try
                {
                    GuiController.Instance.executeSelectedExample(new TgcRoomsEditor());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error en init() de Room Editor \n" + ex.Message, ProductName,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void sceneEditorToolStripButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Seguro que desea cambiar de herramienta?", "Confirmación",
                MessageBoxButtons.YesNo);

            if (result.Equals(DialogResult.Yes))
            {
                try
                {
                    GuiController.Instance.executeSelectedExample(new TgcSceneEditor());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error en init() de Scene Editor \n " + ex.Message, ProductName, 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void terrainEditorSoolStripButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Seguro que desea cambiar de herramienta?", "Confirmación",
                MessageBoxButtons.YesNo);

            if (result.Equals(DialogResult.Yes))
            {
                try
                {
                    GuiController.Instance.executeSelectedExample(new TgcTerrainEditor());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error en init() de Terrain Editor \n" + ex.Message, ProductName, 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Seguro que desea cerrar la aplicación?", "Confirmación",
                MessageBoxButtons.YesNo);

            if (result.Equals(DialogResult.Yes))
            {
                Close();
            }
        }

        private void acercaDeTgcViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }
    }
}