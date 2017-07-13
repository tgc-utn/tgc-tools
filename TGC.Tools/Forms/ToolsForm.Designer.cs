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
            this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wireframeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contadorFPSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ejesCartesianosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mostrarPosicionDeCamaraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ayudaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.acercaDeTgcViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.meshCreatorToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.roomEditorToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.sceneEditorToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.terrainEditorSoolStripButton = new System.Windows.Forms.ToolStripButton();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusCurrentExample = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusPosition = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel3d = new System.Windows.Forms.Panel();
            this.splitContainerPrincipal = new System.Windows.Forms.SplitContainer();
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
            // salirToolStripMenuItem
            // 
            this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            this.salirToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
            this.salirToolStripMenuItem.Text = "Salir";
            this.salirToolStripMenuItem.Click += new System.EventHandler(this.salirToolStripMenuItem_Click);
            // 
            // verToolStripMenuItem
            // 
            this.verToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wireframeToolStripMenuItem,
            this.contadorFPSToolStripMenuItem,
            this.ejesCartesianosToolStripMenuItem,
            this.mostrarPosicionDeCamaraToolStripMenuItem});
            this.verToolStripMenuItem.Name = "verToolStripMenuItem";
            this.verToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.verToolStripMenuItem.Text = "Ver";
            // 
            // wireframeToolStripMenuItem
            // 
            this.wireframeToolStripMenuItem.CheckOnClick = true;
            this.wireframeToolStripMenuItem.Name = "wireframeToolStripMenuItem";
            this.wireframeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.W)));
            this.wireframeToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.wireframeToolStripMenuItem.Text = "Wireframe";
            this.wireframeToolStripMenuItem.Click += new System.EventHandler(this.wireframeToolStripMenuItem_Click);
            // 
            // contadorFPSToolStripMenuItem
            // 
            this.contadorFPSToolStripMenuItem.Checked = true;
            this.contadorFPSToolStripMenuItem.CheckOnClick = true;
            this.contadorFPSToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.contadorFPSToolStripMenuItem.Name = "contadorFPSToolStripMenuItem";
            this.contadorFPSToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.contadorFPSToolStripMenuItem.Text = "Contador FPS";
            this.contadorFPSToolStripMenuItem.Click += new System.EventHandler(this.contadorFPSToolStripMenuItem_Click);
            // 
            // ejesCartesianosToolStripMenuItem
            // 
            this.ejesCartesianosToolStripMenuItem.Checked = true;
            this.ejesCartesianosToolStripMenuItem.CheckOnClick = true;
            this.ejesCartesianosToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ejesCartesianosToolStripMenuItem.Name = "ejesCartesianosToolStripMenuItem";
            this.ejesCartesianosToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.ejesCartesianosToolStripMenuItem.Text = "Ejes cartesianos";
            this.ejesCartesianosToolStripMenuItem.Click += new System.EventHandler(this.ejesCartesianosToolStripMenuItem_Click);
            // 
            // mostrarPosicionDeCamaraToolStripMenuItem
            // 
            this.mostrarPosicionDeCamaraToolStripMenuItem.Checked = true;
            this.mostrarPosicionDeCamaraToolStripMenuItem.CheckOnClick = true;
            this.mostrarPosicionDeCamaraToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mostrarPosicionDeCamaraToolStripMenuItem.Name = "mostrarPosicionDeCamaraToolStripMenuItem";
            this.mostrarPosicionDeCamaraToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.P)));
            this.mostrarPosicionDeCamaraToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.mostrarPosicionDeCamaraToolStripMenuItem.Text = "Mostrar posición de cámara";
            this.mostrarPosicionDeCamaraToolStripMenuItem.Click += new System.EventHandler(this.mostrarPosiciónDeCámaraToolStripMenuItem_Click);
            // 
            // ayudaToolStripMenuItem
            // 
            this.ayudaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.acercaDeTgcViewerToolStripMenuItem});
            this.ayudaToolStripMenuItem.Name = "ayudaToolStripMenuItem";
            this.ayudaToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.ayudaToolStripMenuItem.Text = "Ayuda";
            // 
            // acercaDeTgcViewerToolStripMenuItem
            // 
            this.acercaDeTgcViewerToolStripMenuItem.Name = "acercaDeTgcViewerToolStripMenuItem";
            this.acercaDeTgcViewerToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.acercaDeTgcViewerToolStripMenuItem.Text = "Acerca de TGC Tools";
            this.acercaDeTgcViewerToolStripMenuItem.Click += new System.EventHandler(this.acercaDeTgcViewerToolStripMenuItem_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.meshCreatorToolStripButton,
            this.roomEditorToolStripButton,
            this.sceneEditorToolStripButton,
            this.terrainEditorSoolStripButton});
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(784, 29);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip1";
            // 
            // meshCreatorToolStripButton
            // 
            this.meshCreatorToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.meshCreatorToolStripButton.Image = global::TGC.Tools.Properties.Resources.mesh_creator;
            this.meshCreatorToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.meshCreatorToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.meshCreatorToolStripButton.Name = "meshCreatorToolStripButton";
            this.meshCreatorToolStripButton.Size = new System.Drawing.Size(26, 26);
            this.meshCreatorToolStripButton.Text = "Mesh creator";
            this.meshCreatorToolStripButton.Click += new System.EventHandler(this.meshCreatorToolStripButton_Click);
            // 
            // roomEditorToolStripButton
            // 
            this.roomEditorToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.roomEditorToolStripButton.Image = global::TGC.Tools.Properties.Resources.room_editor;
            this.roomEditorToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.roomEditorToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.roomEditorToolStripButton.Name = "roomEditorToolStripButton";
            this.roomEditorToolStripButton.Size = new System.Drawing.Size(26, 26);
            this.roomEditorToolStripButton.Text = "Room editor";
            this.roomEditorToolStripButton.Click += new System.EventHandler(this.roomEditorToolStripButton_Click);
            // 
            // sceneEditorToolStripButton
            // 
            this.sceneEditorToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.sceneEditorToolStripButton.Image = global::TGC.Tools.Properties.Resources.scene_editor;
            this.sceneEditorToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.sceneEditorToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.sceneEditorToolStripButton.Name = "sceneEditorToolStripButton";
            this.sceneEditorToolStripButton.Size = new System.Drawing.Size(26, 26);
            this.sceneEditorToolStripButton.Text = "Scene editor";
            this.sceneEditorToolStripButton.Click += new System.EventHandler(this.sceneEditorToolStripButton_Click);
            // 
            // terrainEditorSoolStripButton
            // 
            this.terrainEditorSoolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.terrainEditorSoolStripButton.Image = global::TGC.Tools.Properties.Resources.terrain_editor;
            this.terrainEditorSoolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.terrainEditorSoolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.terrainEditorSoolStripButton.Name = "terrainEditorSoolStripButton";
            this.terrainEditorSoolStripButton.Size = new System.Drawing.Size(26, 26);
            this.terrainEditorSoolStripButton.Text = "Terrain editor";
            this.terrainEditorSoolStripButton.Click += new System.EventHandler(this.terrainEditorSoolStripButton_Click);
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
            // panel3d
            // 
            this.panel3d.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel3d.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3d.Location = new System.Drawing.Point(0, 0);
            this.panel3d.Name = "panel3d";
            this.panel3d.Size = new System.Drawing.Size(602, 486);
            this.panel3d.TabIndex = 5;
            // 
            // splitContainerPrincipal
            // 
            this.splitContainerPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerPrincipal.Location = new System.Drawing.Point(0, 53);
            this.splitContainerPrincipal.Name = "splitContainerPrincipal";
            // 
            // splitContainerPrincipal.Panel1
            // 
            this.splitContainerPrincipal.Panel1.Controls.Add(this.panel3d);
            this.splitContainerPrincipal.Panel1MinSize = 500;
            this.splitContainerPrincipal.Panel2MinSize = 100;
            this.splitContainerPrincipal.Size = new System.Drawing.Size(784, 486);
            this.splitContainerPrincipal.SplitterDistance = 602;
            this.splitContainerPrincipal.TabIndex = 6;
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
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
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
        private System.Windows.Forms.ToolStripMenuItem contadorFPSToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusCurrentExample;
        private System.Windows.Forms.ToolStripMenuItem ejesCartesianosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem acercaDeTgcViewerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mostrarPosicionDeCamaraToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton meshCreatorToolStripButton;
        private System.Windows.Forms.ToolStripButton roomEditorToolStripButton;
        private System.Windows.Forms.ToolStripButton sceneEditorToolStripButton;
        private System.Windows.Forms.ToolStripButton terrainEditorSoolStripButton;
        private System.Windows.Forms.Panel panel3d;
        private System.Windows.Forms.SplitContainer splitContainerPrincipal;
        private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem;
    }
}

