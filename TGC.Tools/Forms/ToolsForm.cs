using System;
using System.Windows.Forms;
using TGC.Tools.Example;
using TGC.Tools.MeshCreator;
using TGC.Tools.Model;
using TGC.Tools.Properties;
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
        /// Modelo del Viewer.
        /// </summary>
        private ToolsModel Model { get; set; }

        //Archivo de configuracion
        private Settings Settings { get; set; }

        /// <summary>
        ///     Constructor principal de la aplicacion
        /// </summary>
        public ToolsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Inicializacion de los componentes principales y carga de ejemplos.
        /// </summary>
        private void InitAplication()
        {
            //Archivo de configuracion
            Settings = Settings.Default;

            //Titulo de la ventana principal
            Text = Settings.Title;

            //Herramientas basicas.
            fpsToolStripMenuItem.Checked = true;
            axisToolStripMenuItem.Checked = true;
            tickConstanteToolStripMenuItem.Checked = true;

            //Modelo de la aplicacion
            Model = ToolsModel.Instance;

            //Verificamos la carpeta Media y la de TGC shaders basicos
            if (Model.CheckFolder(Settings.MediaDirectory) || Model.CheckFolder(Settings.ShadersDirectory) || Model.CheckFolder(Settings.CommonShaders))
            {
                if (OpenOption() == DialogResult.Cancel)
                {
                    //Fuerzo el cierre de la aplicacion.
                    Environment.Exit(0);
                }
            }

            //Iniciar graficos
            try
            {
                Model.InitGraphics(this, panel3D);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocurrio un problema al inicializar DX", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Model.InitShaders(Settings.CommonShaders);

            try
            {
                //Cargar ejemplo default
                Model.ExecuteExample(new TgcMeshCreator(Settings.MediaDirectory, Settings.ShadersDirectory, splitContainerPrincipal.Panel2));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No se pudo cargar el MeshCreator.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Model.InitRenderLoop();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error en RenderLoop de la herramienta.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            panel3D.Focus();
        }

        /// <summary>
        /// Indica si la aplicacion esta activa.
        /// Busca si la ventana principal tiene foco o si alguna de sus hijas tiene.
        /// </summary>
        public bool ApplicationActive()
        {
            if (ContainsFocus)
            {
                return true;
            }

            foreach (var form in OwnedForms)
            {
                if (form.ContainsFocus)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Dialogo de confirmacion para cerrar la aplicacion.
        /// </summary>
        /// <returns> Si hay o no la aplicacion.</returns>
        public bool CloseAplication()
        {
            var result = MessageBox.Show("¿Esta seguro que desea cerrar la aplicación?", Text, MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                if (Model.ApplicationRunning)
                {
                    Model.Dispose();
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// Vuelve al estado inicial los valores del menu.
        /// </summary>
        private void ResetMenuValues()
        {
            wireframeToolStripMenuItem.Checked = false;
            fpsToolStripMenuItem.Checked = true;
            axisToolStripMenuItem.Checked = true;
            //fullExampleToolStripMenuItem.Checked = false;
            //splitContainerIzquierda.Visible = true;
            //splitContainerDerecha.Visible = true;
            statusStrip.Visible = true;

            Model.UpdateAspectRatio(panel3D);
            Model.Wireframe(wireframeToolStripMenuItem.Checked);
            Model.ContadorFPS(fpsToolStripMenuItem.Checked);
            Model.AxisLines(axisToolStripMenuItem.Checked);
        }

        /// <summary>
        /// Abre un dialogo con informacion de la aplicacion.
        /// </summary>
        private void OpenAbout()
        {
            new AboutForm().ShowDialog(this);
        }

        /// <summary>
        /// Abre las opciones de la aplicacion.
        /// </summary>
        private DialogResult OpenOption()
        {
            return new OptionForm().ShowDialog(this);
        }

        /// <summary>
        /// Activa o desactiva la opcion de wireframe en el ejemplo.
        /// </summary>
        private void Wireframe()
        {
            Model.Wireframe(wireframeToolStripMenuItem.Checked);
        }

        /// <summary>
        /// Activa o desactiva la opcion del contador de fps.
        /// </summary>
        private void ContadorFPS()
        {
            Model.ContadorFPS(fpsToolStripMenuItem.Checked);
        }

        /// <summary>
        /// Activa o desactiva la opcion de correr a update constante.
        /// </summary>
        private void FixedTick()
        {
            Model.FixedTick(tickConstanteToolStripMenuItem.Checked);
        }

        /// <summary>
        /// Activa o desactiva la opcion de los ejes cartesianos.
        /// </summary>
        private void AxisLines()
        {
            Model.AxisLines(axisToolStripMenuItem.Checked);
        }

        /// <summary>
        /// Ejecuta un ejemplo particular.
        /// </summary>
        /// <param name="example">Ejemplo a ejecutar.</param>
        private void ExcecuteExample(TGCExampleTools example)
        {
            var result = MessageBox.Show("¿Seguro que desea cambiar de herramienta?", "Confirmación", MessageBoxButtons.YesNo);

            if (result.Equals(DialogResult.Yes))
            {
                try
                {
                    Model.ExecuteExample(example);
                    AxisLines();
                    ContadorFPS();
                    FixedTick();
                    Wireframe();

                    toolStripStatusCurrentExample.Text = "Ejemplo actual: " + example.Name;
                    panel3D.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "No se pudo cargar el ejemplo " + example.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    Model.ClearCurrentExample();
                }
            }
        }

        /// <summary>
        ///     Texto de ToolStripStatusPosition
        /// </summary>
        public void setStatusPosition(string text)
        {
            toolStripStatusPosition.Text = text;
        }

        #region Eventos del form

        private void ToolsForm_Load(object sender, EventArgs e)
        {
            InitAplication();
        }

        private void ToolsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = CloseAplication();
        }

        private void fpsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ContadorFPS();
        }

        private void tickConstanteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FixedTick();
        }

        private void axisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AxisLines();
        }

        private void wireframeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Wireframe();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void acercaDeTgcViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenAbout();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetMenuValues();
        }

        private void opcionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenOption();
        }

        private void meshCreatorToolStripButton_Click(object sender, EventArgs e)
        {
            ExcecuteExample(new TgcMeshCreator(Settings.MediaDirectory, Settings.ShadersDirectory, splitContainerPrincipal.Panel2));
        }

        private void roomEditorToolStripButton_Click(object sender, EventArgs e)
        {
            ExcecuteExample(new TgcRoomsEditor(Settings.MediaDirectory, Settings.ShadersDirectory, splitContainerPrincipal.Panel2));
        }

        private void sceneEditorToolStripButton_Click(object sender, EventArgs e)
        {
            ExcecuteExample(new TgcSceneEditor(Settings.MediaDirectory, Settings.ShadersDirectory, splitContainerPrincipal.Panel2));
        }

        private void terrainEditorSoolStripButton_Click(object sender, EventArgs e)
        {
            ExcecuteExample(new TgcTerrainEditor(Settings.MediaDirectory, Settings.ShadersDirectory, splitContainerPrincipal.Panel2));
        }

        private void toolStripButtonSceneLoader_Click(object sender, EventArgs e)
        {
            ExcecuteExample(new SceneLoader.SceneLoader(Settings.MediaDirectory, Settings.ShadersDirectory, splitContainerPrincipal.Panel2));
        }

        private void mostrarPosiciónDeCámaraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!mostrarPosicionDeCamaraToolStripMenuItem.Checked)
            {
                toolStripStatusPosition.Text = "";
            }
        }

        #endregion Eventos del form
    }
}