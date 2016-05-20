using Examples.MeshCreator;
using Examples.RoomsEditor;
using Examples.SceneEditor;
using Examples.TerrainEditor;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace TgcViewer
{
    /// <summary>
    /// Formulario principal de la aplicación
    /// </summary>
    public partial class MainForm : Form
    {
        private static bool applicationRunning;

        /// <summary>
        /// Obtener o parar el estado del RenderLoop.
        /// </summary>
        public static bool ApplicationRunning
        {
            get { return MainForm.applicationRunning; }
            set { MainForm.applicationRunning = value; }
        }

        /// <summary>
        /// Constructor principal de la aplicacion
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //Show the App before we init
            this.Show();
            this.panel3d.Focus();

            MainForm.ApplicationRunning = true;
            GuiController.newInstance();
            GuiController guiController = GuiController.Instance;
            guiController.initGraphics(this, panel3d);

            while (MainForm.ApplicationRunning)
            {
                //Solo renderizamos si la aplicacion tiene foco, para no consumir recursos innecesarios
                if (this.applicationActive())
                {
                    guiController.render();
                }
                else
                {
                    //Si no tenemos el foco, dormir cada tanto para no consumir gran cantida de CPU
                    System.Threading.Thread.Sleep(100);
                }

                // Process application messages
                Application.DoEvents();
            }
        }

        /// <summary>
        /// Finalizar aplicacion
        /// </summary>
        private void shutDown()
        {
            MainForm.ApplicationRunning = false;
            //GuiController.Instance.shutDown();
            //Application.Exit();

            //Matar proceso principal a la fuerza
            Process currentProcess = Process.GetCurrentProcess();
            currentProcess.Kill();
        }

        /// <summary>
        /// Cerrando el formulario
        /// </summary>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            shutDown();
        }

        /// <summary>
        /// Texto de ToolStripStatusPosition
        /// </summary>
        public void setStatusPosition(string text)
        {
            toolStripStatusPosition.Text = text;
        }

        /// <summary>
        /// Texto de ToolStripStatusCurrentExample
        /// </summary>
        /// <param name="text"></param>
        public void setCurrentExampleStatus(string text)
        {
            toolStripStatusCurrentExample.Text = text;
        }

        /// <summary>
        /// Panel de Modifiers
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
                GuiController.Instance.D3dDevice.RenderState.FillMode = Microsoft.DirectX.Direct3D.FillMode.WireFrame;
            }
            else
            {
                GuiController.Instance.D3dDevice.RenderState.FillMode = Microsoft.DirectX.Direct3D.FillMode.Solid;
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
        /// Setea los valores default de las opciones del menu
        /// </summary>
        internal void resetMenuOptions()
        {
            wireframeToolStripMenuItem.Checked = false;
            contadorFPSToolStripMenuItem.Checked = true;
            ejesCartesianosToolStripMenuItem.Checked = true;
        }

        /// <summary>
        /// Mostrar posicion de camara
        /// </summary>
        internal bool MostrarPosicionDeCamaraEnable
        {
            get { return mostrarPosiciónDeCámaraToolStripMenuItem.Checked; }
            set { mostrarPosiciónDeCámaraToolStripMenuItem.Checked = value; }
        }

        private void mostrarPosiciónDeCámaraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!mostrarPosiciónDeCámaraToolStripMenuItem.Checked)
            {
                toolStripStatusPosition.Text = "";
            }
        }

        /// <summary>
        /// Indica si la aplicacion esta activa.
        /// Busca si la ventana principal tiene foco o si alguna de sus hijas tiene.
        /// </summary>
        private bool applicationActive()
        {
            if (this.ContainsFocus) return true;
            foreach (Form form in this.OwnedForms)
            {
                if (form.ContainsFocus) return true;
            }
            return false;
        }

        private void meshCreatorToolStripButton_Click(object sender, EventArgs e)
        {
            GuiController.Instance.executeSelectedExample(new TgcMeshCreator());
        }

        private void roomEditorToolStripButton_Click(object sender, EventArgs e)
        {
            GuiController.Instance.executeSelectedExample(new TgcRoomsEditor());
        }

        private void sceneEditorToolStripButton_Click(object sender, EventArgs e)
        {
            GuiController.Instance.executeSelectedExample(new TgcSceneEditor());
        }

        private void terrainEditorSoolStripButton_Click(object sender, EventArgs e)
        {
            GuiController.Instance.executeSelectedExample(new TgcTerrainEditor());
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void acercaDeTgcViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TGC Tools :)");
        }
    }
}