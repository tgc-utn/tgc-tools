namespace TGC.Tools.Forms
{
    partial class ToolsForm
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolsForm));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.archivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tickConstanteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.herramientasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ayudaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusCurrentExample = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusPosition = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel3D = new System.Windows.Forms.Panel();
            this.splitContainerPrincipal = new System.Windows.Forms.SplitContainer();
            this.meshCreatorToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.roomEditorToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.sceneEditorToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.terrainEditorSoolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSceneLoader = new System.Windows.Forms.ToolStripButton();
            this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wireframeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fpsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.axisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mostrarPosicionDeCamaraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reiniciarVisualizaciónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.opcionesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.acercaDeTgcViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerPrincipal)).BeginInit();
            this.splitContainerPrincipal.Panel1.SuspendLayout();
            this.splitContainerPrincipal.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.archivoToolStripMenuItem,
            this.verToolStripMenuItem,
            this.herramientasToolStripMenuItem,
            this.ayudaToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(784, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip";
            // 
            // archivoToolStripMenuItem
            // 
            this.archivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.salirToolStripMenuItem});
            this.archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            this.archivoToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.archivoToolStripMenuItem.Text = "Archivo";
            // 
            // verToolStripMenuItem
            // 
            this.verToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wireframeToolStripMenuItem,
            this.fpsToolStripMenuItem,
            this.axisToolStripMenuItem,
            this.mostrarPosicionDeCamaraToolStripMenuItem,
            this.tickConstanteToolStripMenuItem,
            this.toolStripSeparator1,
            this.reiniciarVisualizaciónToolStripMenuItem});
            this.verToolStripMenuItem.Name = "verToolStripMenuItem";
            this.verToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.verToolStripMenuItem.Text = "Ver";
            // 
            // tickConstanteToolStripMenuItem
            // 
            this.tickConstanteToolStripMenuItem.CheckOnClick = true;
            this.tickConstanteToolStripMenuItem.Image = global::TGC.Tools.Properties.Resources.view_refresh;
            this.tickConstanteToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tickConstanteToolStripMenuItem.Name = "tickConstanteToolStripMenuItem";
            this.tickConstanteToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.tickConstanteToolStripMenuItem.Text = "Render a intervalo constante";
            this.tickConstanteToolStripMenuItem.Click += new System.EventHandler(this.tickConstanteToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(255, 6);
            // 
            // herramientasToolStripMenuItem
            // 
            this.herramientasToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.opcionesToolStripMenuItem});
            this.herramientasToolStripMenuItem.Name = "herramientasToolStripMenuItem";
            this.herramientasToolStripMenuItem.Size = new System.Drawing.Size(90, 20);
            this.herramientasToolStripMenuItem.Text = "Herramientas";
            // 
            // ayudaToolStripMenuItem
            // 
            this.ayudaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.acercaDeTgcViewerToolStripMenuItem});
            this.ayudaToolStripMenuItem.Name = "ayudaToolStripMenuItem";
            this.ayudaToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.ayudaToolStripMenuItem.Text = "Ayuda";
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.meshCreatorToolStripButton,
            this.roomEditorToolStripButton,
            this.sceneEditorToolStripButton,
            this.terrainEditorSoolStripButton,
            this.toolStripButtonSceneLoader});
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(784, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip1";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusCurrentExample,
            this.toolStripStatusPosition});
            this.statusStrip.Location = new System.Drawing.Point(0, 539);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(784, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusCurrentExample
            // 
            this.toolStripStatusCurrentExample.BackColor = System.Drawing.Color.GreenYellow;
            this.toolStripStatusCurrentExample.Name = "toolStripStatusCurrentExample";
            this.toolStripStatusCurrentExample.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusPosition
            // 
            this.toolStripStatusPosition.Name = "toolStripStatusPosition";
            this.toolStripStatusPosition.Size = new System.Drawing.Size(0, 17);
            // 
            // panel3D
            // 
            this.panel3D.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel3D.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3D.Location = new System.Drawing.Point(0, 0);
            this.panel3D.Name = "panel3D";
            this.panel3D.Size = new System.Drawing.Size(602, 490);
            this.panel3D.TabIndex = 5;
            // 
            // splitContainerPrincipal
            // 
            this.splitContainerPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerPrincipal.Location = new System.Drawing.Point(0, 49);
            this.splitContainerPrincipal.Name = "splitContainerPrincipal";
            // 
            // splitContainerPrincipal.Panel1
            // 
            this.splitContainerPrincipal.Panel1.Controls.Add(this.panel3D);
            this.splitContainerPrincipal.Panel1MinSize = 500;
            this.splitContainerPrincipal.Panel2MinSize = 100;
            this.splitContainerPrincipal.Size = new System.Drawing.Size(784, 490);
            this.splitContainerPrincipal.SplitterDistance = 602;
            this.splitContainerPrincipal.TabIndex = 6;
            // 
            // meshCreatorToolStripButton
            // 
            this.meshCreatorToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.meshCreatorToolStripButton.Image = global::TGC.Tools.Properties.Resources.ICON_GROUP;
            this.meshCreatorToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.meshCreatorToolStripButton.Name = "meshCreatorToolStripButton";
            this.meshCreatorToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.meshCreatorToolStripButton.Text = "Mesh creator";
            this.meshCreatorToolStripButton.Click += new System.EventHandler(this.meshCreatorToolStripButton_Click);
            // 
            // roomEditorToolStripButton
            // 
            this.roomEditorToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.roomEditorToolStripButton.Image = global::TGC.Tools.Properties.Resources.ICON_MOD_BUILD;
            this.roomEditorToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.roomEditorToolStripButton.Name = "roomEditorToolStripButton";
            this.roomEditorToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.roomEditorToolStripButton.Text = "Room editor";
            this.roomEditorToolStripButton.Click += new System.EventHandler(this.roomEditorToolStripButton_Click);
            // 
            // sceneEditorToolStripButton
            // 
            this.sceneEditorToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.sceneEditorToolStripButton.Image = global::TGC.Tools.Properties.Resources.ICON_MOD_MIRROR;
            this.sceneEditorToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.sceneEditorToolStripButton.Name = "sceneEditorToolStripButton";
            this.sceneEditorToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.sceneEditorToolStripButton.Text = "Scene editor";
            this.sceneEditorToolStripButton.Click += new System.EventHandler(this.sceneEditorToolStripButton_Click);
            // 
            // terrainEditorSoolStripButton
            // 
            this.terrainEditorSoolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.terrainEditorSoolStripButton.Image = global::TGC.Tools.Properties.Resources.ICON_RNDCURVE;
            this.terrainEditorSoolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.terrainEditorSoolStripButton.Name = "terrainEditorSoolStripButton";
            this.terrainEditorSoolStripButton.Size = new System.Drawing.Size(23, 22);
            this.terrainEditorSoolStripButton.Text = "Terrain editor";
            this.terrainEditorSoolStripButton.Click += new System.EventHandler(this.terrainEditorSoolStripButton_Click);
            // 
            // toolStripButtonSceneLoader
            // 
            this.toolStripButtonSceneLoader.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSceneLoader.Image = global::TGC.Tools.Properties.Resources.ICON_MONKEY;
            this.toolStripButtonSceneLoader.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSceneLoader.Name = "toolStripButtonSceneLoader";
            this.toolStripButtonSceneLoader.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSceneLoader.Text = "Scene Loader";
            this.toolStripButtonSceneLoader.Click += new System.EventHandler(this.toolStripButtonSceneLoader_Click);
            // 
            // salirToolStripMenuItem
            // 
            this.salirToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("salirToolStripMenuItem.Image")));
            this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            this.salirToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
            this.salirToolStripMenuItem.Text = "Salir";
            this.salirToolStripMenuItem.Click += new System.EventHandler(this.salirToolStripMenuItem_Click);
            // 
            // wireframeToolStripMenuItem
            // 
            this.wireframeToolStripMenuItem.CheckOnClick = true;
            this.wireframeToolStripMenuItem.Image = global::TGC.Tools.Properties.Resources.ICON_WIRE;
            this.wireframeToolStripMenuItem.Name = "wireframeToolStripMenuItem";
            this.wireframeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.W)));
            this.wireframeToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.wireframeToolStripMenuItem.Text = "Wireframe";
            this.wireframeToolStripMenuItem.Click += new System.EventHandler(this.wireframeToolStripMenuItem_Click);
            // 
            // fpsToolStripMenuItem
            // 
            this.fpsToolStripMenuItem.Checked = true;
            this.fpsToolStripMenuItem.CheckOnClick = true;
            this.fpsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.fpsToolStripMenuItem.Image = global::TGC.Tools.Properties.Resources.ICON_TIME;
            this.fpsToolStripMenuItem.Name = "fpsToolStripMenuItem";
            this.fpsToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.fpsToolStripMenuItem.Text = "Contador FPS";
            this.fpsToolStripMenuItem.Click += new System.EventHandler(this.fpsToolStripMenuItem_Click);
            // 
            // axisToolStripMenuItem
            // 
            this.axisToolStripMenuItem.Checked = true;
            this.axisToolStripMenuItem.CheckOnClick = true;
            this.axisToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.axisToolStripMenuItem.Image = global::TGC.Tools.Properties.Resources.ICON_MANIPUL;
            this.axisToolStripMenuItem.Name = "axisToolStripMenuItem";
            this.axisToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.axisToolStripMenuItem.Text = "Ejes cartesianos";
            this.axisToolStripMenuItem.Click += new System.EventHandler(this.axisToolStripMenuItem_Click);
            // 
            // mostrarPosicionDeCamaraToolStripMenuItem
            // 
            this.mostrarPosicionDeCamaraToolStripMenuItem.Checked = true;
            this.mostrarPosicionDeCamaraToolStripMenuItem.CheckOnClick = true;
            this.mostrarPosicionDeCamaraToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mostrarPosicionDeCamaraToolStripMenuItem.Image = global::TGC.Tools.Properties.Resources.ICON_CAMERA_DATA;
            this.mostrarPosicionDeCamaraToolStripMenuItem.Name = "mostrarPosicionDeCamaraToolStripMenuItem";
            this.mostrarPosicionDeCamaraToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.P)));
            this.mostrarPosicionDeCamaraToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.mostrarPosicionDeCamaraToolStripMenuItem.Text = "Mostrar posición de cámara";
            this.mostrarPosicionDeCamaraToolStripMenuItem.Click += new System.EventHandler(this.mostrarPosiciónDeCámaraToolStripMenuItem_Click);
            // 
            // reiniciarVisualizaciónToolStripMenuItem
            // 
            this.reiniciarVisualizaciónToolStripMenuItem.Image = global::TGC.Tools.Properties.Resources.edit_clear_all;
            this.reiniciarVisualizaciónToolStripMenuItem.Name = "reiniciarVisualizaciónToolStripMenuItem";
            this.reiniciarVisualizaciónToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.reiniciarVisualizaciónToolStripMenuItem.Text = "Reiniciar Visualización";
            this.reiniciarVisualizaciónToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
            // 
            // opcionesToolStripMenuItem
            // 
            this.opcionesToolStripMenuItem.Image = global::TGC.Tools.Properties.Resources.preferences_desktop;
            this.opcionesToolStripMenuItem.Name = "opcionesToolStripMenuItem";
            this.opcionesToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.opcionesToolStripMenuItem.Text = "Opciones...";
            this.opcionesToolStripMenuItem.Click += new System.EventHandler(this.opcionesToolStripMenuItem_Click);
            // 
            // acercaDeTgcViewerToolStripMenuItem
            // 
            this.acercaDeTgcViewerToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("acercaDeTgcViewerToolStripMenuItem.Image")));
            this.acercaDeTgcViewerToolStripMenuItem.Name = "acercaDeTgcViewerToolStripMenuItem";
            this.acercaDeTgcViewerToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.acercaDeTgcViewerToolStripMenuItem.Text = "Acerca de TGC Tools";
            this.acercaDeTgcViewerToolStripMenuItem.Click += new System.EventHandler(this.acercaDeTgcViewerToolStripMenuItem_Click);
            // 
            // ToolsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.splitContainerPrincipal);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "ToolsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ToolsForm_FormClosing);
            this.Load += new System.EventHandler(this.ToolsForm_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.splitContainerPrincipal.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerPrincipal)).EndInit();
            this.splitContainerPrincipal.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem archivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ayudaToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusPosition;
        private System.Windows.Forms.ToolStripMenuItem verToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wireframeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fpsToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusCurrentExample;
        private System.Windows.Forms.ToolStripMenuItem axisToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem acercaDeTgcViewerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mostrarPosicionDeCamaraToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton meshCreatorToolStripButton;
        private System.Windows.Forms.ToolStripButton roomEditorToolStripButton;
        private System.Windows.Forms.ToolStripButton sceneEditorToolStripButton;
        private System.Windows.Forms.ToolStripButton terrainEditorSoolStripButton;
        private System.Windows.Forms.Panel panel3D;
        private System.Windows.Forms.SplitContainer splitContainerPrincipal;
        private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem reiniciarVisualizaciónToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem herramientasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem opcionesToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonSceneLoader;
        private System.Windows.Forms.ToolStripMenuItem tickConstanteToolStripMenuItem;
    }
}

