namespace TGC.Tools
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.archivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wireframeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contadorFPSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ejesCartesianosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mostrarPosiciónDeCámaraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.groupBoxModifiers = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanelModifiers = new System.Windows.Forms.FlowLayoutPanel();
            this.panel3d = new System.Windows.Forms.Panel();
            this.splitContainerPrincipal = new System.Windows.Forms.SplitContainer();
            this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.groupBoxModifiers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerPrincipal)).BeginInit();
            this.splitContainerPrincipal.Panel1.SuspendLayout();
            this.splitContainerPrincipal.Panel2.SuspendLayout();
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
            // verToolStripMenuItem
            // 
            this.verToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wireframeToolStripMenuItem,
            this.contadorFPSToolStripMenuItem,
            this.ejesCartesianosToolStripMenuItem,
            this.mostrarPosiciónDeCámaraToolStripMenuItem});
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
            // mostrarPosiciónDeCámaraToolStripMenuItem
            // 
            this.mostrarPosiciónDeCámaraToolStripMenuItem.Checked = true;
            this.mostrarPosiciónDeCámaraToolStripMenuItem.CheckOnClick = true;
            this.mostrarPosiciónDeCámaraToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mostrarPosiciónDeCámaraToolStripMenuItem.Name = "mostrarPosiciónDeCámaraToolStripMenuItem";
            this.mostrarPosiciónDeCámaraToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.P)));
            this.mostrarPosiciónDeCámaraToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.mostrarPosiciónDeCámaraToolStripMenuItem.Text = "Mostrar posición de cámara";
            this.mostrarPosiciónDeCámaraToolStripMenuItem.Click += new System.EventHandler(this.mostrarPosiciónDeCámaraToolStripMenuItem_Click);
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
            this.toolStrip.Size = new System.Drawing.Size(784, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip1";
            // 
            // meshCreatorToolStripButton
            // 
            this.meshCreatorToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.meshCreatorToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("meshCreatorToolStripButton.Image")));
            this.meshCreatorToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.meshCreatorToolStripButton.Name = "meshCreatorToolStripButton";
            this.meshCreatorToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.meshCreatorToolStripButton.Text = "Mesh creator";
            this.meshCreatorToolStripButton.Click += new System.EventHandler(this.meshCreatorToolStripButton_Click);
            // 
            // roomEditorToolStripButton
            // 
            this.roomEditorToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.roomEditorToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("roomEditorToolStripButton.Image")));
            this.roomEditorToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.roomEditorToolStripButton.Name = "roomEditorToolStripButton";
            this.roomEditorToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.roomEditorToolStripButton.Text = "Room editor";
            this.roomEditorToolStripButton.Click += new System.EventHandler(this.roomEditorToolStripButton_Click);
            // 
            // sceneEditorToolStripButton
            // 
            this.sceneEditorToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.sceneEditorToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("sceneEditorToolStripButton.Image")));
            this.sceneEditorToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.sceneEditorToolStripButton.Name = "sceneEditorToolStripButton";
            this.sceneEditorToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.sceneEditorToolStripButton.Text = "Scene editor";
            this.sceneEditorToolStripButton.Click += new System.EventHandler(this.sceneEditorToolStripButton_Click);
            // 
            // terrainEditorSoolStripButton
            // 
            this.terrainEditorSoolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.terrainEditorSoolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("terrainEditorSoolStripButton.Image")));
            this.terrainEditorSoolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.terrainEditorSoolStripButton.Name = "terrainEditorSoolStripButton";
            this.terrainEditorSoolStripButton.Size = new System.Drawing.Size(23, 22);
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
            // groupBoxModifiers
            // 
            this.groupBoxModifiers.BackColor = System.Drawing.SystemColors.Control;
            this.groupBoxModifiers.Controls.Add(this.flowLayoutPanelModifiers);
            this.groupBoxModifiers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxModifiers.Location = new System.Drawing.Point(0, 0);
            this.groupBoxModifiers.Name = "groupBoxModifiers";
            this.groupBoxModifiers.Size = new System.Drawing.Size(178, 490);
            this.groupBoxModifiers.TabIndex = 0;
            this.groupBoxModifiers.TabStop = false;
            this.groupBoxModifiers.Text = "Modifiers";
            // 
            // flowLayoutPanelModifiers
            // 
            this.flowLayoutPanelModifiers.AutoScroll = true;
            this.flowLayoutPanelModifiers.AutoSize = true;
            this.flowLayoutPanelModifiers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelModifiers.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelModifiers.Location = new System.Drawing.Point(3, 16);
            this.flowLayoutPanelModifiers.Name = "flowLayoutPanelModifiers";
            this.flowLayoutPanelModifiers.Size = new System.Drawing.Size(172, 471);
            this.flowLayoutPanelModifiers.TabIndex = 0;
            this.flowLayoutPanelModifiers.WrapContents = false;
            // 
            // panel3d
            // 
            this.panel3d.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel3d.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3d.Location = new System.Drawing.Point(0, 0);
            this.panel3d.Name = "panel3d";
            this.panel3d.Size = new System.Drawing.Size(602, 490);
            this.panel3d.TabIndex = 5;
            // 
            // splitContainerPrincipal
            // 
            this.splitContainerPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerPrincipal.IsSplitterFixed = true;
            this.splitContainerPrincipal.Location = new System.Drawing.Point(0, 49);
            this.splitContainerPrincipal.Name = "splitContainerPrincipal";
            // 
            // splitContainerPrincipal.Panel1
            // 
            this.splitContainerPrincipal.Panel1.Controls.Add(this.panel3d);
            // 
            // splitContainerPrincipal.Panel2
            // 
            this.splitContainerPrincipal.Panel2.Controls.Add(this.groupBoxModifiers);
            this.splitContainerPrincipal.Size = new System.Drawing.Size(784, 490);
            this.splitContainerPrincipal.SplitterDistance = 602;
            this.splitContainerPrincipal.TabIndex = 6;
            // 
            // salirToolStripMenuItem
            // 
            this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            this.salirToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.salirToolStripMenuItem.Text = "Salir";
            this.salirToolStripMenuItem.Click += new System.EventHandler(this.salirToolStripMenuItem_Click);
            // 
            // MainForm
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
            this.Name = "MainForm";
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
            this.groupBoxModifiers.ResumeLayout(false);
            this.groupBoxModifiers.PerformLayout();
            this.splitContainerPrincipal.Panel1.ResumeLayout(false);
            this.splitContainerPrincipal.Panel2.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripMenuItem mostrarPosiciónDeCámaraToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton meshCreatorToolStripButton;
        private System.Windows.Forms.ToolStripButton roomEditorToolStripButton;
        private System.Windows.Forms.ToolStripButton sceneEditorToolStripButton;
        private System.Windows.Forms.ToolStripButton terrainEditorSoolStripButton;
        private System.Windows.Forms.GroupBox groupBoxModifiers;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelModifiers;
        private System.Windows.Forms.Panel panel3d;
        private System.Windows.Forms.SplitContainer splitContainerPrincipal;
        private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem;
    }
}

