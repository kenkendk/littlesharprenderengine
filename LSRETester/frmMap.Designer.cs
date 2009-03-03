namespace LSRETester
{
    partial class frmMap
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMap));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ZoomInButton = new System.Windows.Forms.ToolStripButton();
            this.ZoomOutButton = new System.Windows.Forms.ToolStripButton();
            this.PanButton = new System.Windows.Forms.ToolStripButton();
            this.UnzoomButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.DisableScalesButton = new System.Windows.Forms.ToolStripButton();
            this.ToggleLayerListButton = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.CursorPosition = new System.Windows.Forms.ToolStripStatusLabel();
            this.CurrentScale = new System.Windows.Forms.ToolStripStatusLabel();
            this.SpeedMeasure = new System.Windows.Forms.ToolStripStatusLabel();
            this.LayerListBox = new System.Windows.Forms.CheckedListBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 25);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(243, 302);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ZoomInButton,
            this.ZoomOutButton,
            this.PanButton,
            this.UnzoomButton,
            this.toolStripSeparator1,
            this.DisableScalesButton,
            this.ToggleLayerListButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(363, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // ZoomInButton
            // 
            this.ZoomInButton.Checked = true;
            this.ZoomInButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ZoomInButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ZoomInButton.Image = ((System.Drawing.Image)(resources.GetObject("ZoomInButton.Image")));
            this.ZoomInButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ZoomInButton.Name = "ZoomInButton";
            this.ZoomInButton.Size = new System.Drawing.Size(23, 22);
            this.ZoomInButton.ToolTipText = "Zoom in";
            this.ZoomInButton.Click += new System.EventHandler(this.ZoomInButton_Click);
            // 
            // ZoomOutButton
            // 
            this.ZoomOutButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ZoomOutButton.Image = ((System.Drawing.Image)(resources.GetObject("ZoomOutButton.Image")));
            this.ZoomOutButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ZoomOutButton.Name = "ZoomOutButton";
            this.ZoomOutButton.Size = new System.Drawing.Size(23, 22);
            this.ZoomOutButton.ToolTipText = "Zoom out";
            this.ZoomOutButton.Click += new System.EventHandler(this.ZoomOutButton_Click);
            // 
            // PanButton
            // 
            this.PanButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.PanButton.Image = ((System.Drawing.Image)(resources.GetObject("PanButton.Image")));
            this.PanButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.PanButton.Name = "PanButton";
            this.PanButton.Size = new System.Drawing.Size(23, 22);
            this.PanButton.ToolTipText = "Pan the map";
            this.PanButton.Click += new System.EventHandler(this.PanButton_Click);
            // 
            // UnzoomButton
            // 
            this.UnzoomButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.UnzoomButton.Image = ((System.Drawing.Image)(resources.GetObject("UnzoomButton.Image")));
            this.UnzoomButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.UnzoomButton.Name = "UnzoomButton";
            this.UnzoomButton.Size = new System.Drawing.Size(23, 22);
            this.UnzoomButton.ToolTipText = "Unzoom";
            this.UnzoomButton.Click += new System.EventHandler(this.UnzoomButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // DisableScalesButton
            // 
            this.DisableScalesButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DisableScalesButton.Image = ((System.Drawing.Image)(resources.GetObject("DisableScalesButton.Image")));
            this.DisableScalesButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DisableScalesButton.Name = "DisableScalesButton";
            this.DisableScalesButton.Size = new System.Drawing.Size(23, 22);
            this.DisableScalesButton.ToolTipText = "Disable scale filters (eg. render all features)";
            this.DisableScalesButton.Click += new System.EventHandler(this.DisableScalesButton_Click);
            // 
            // ToggleLayerListButton
            // 
            this.ToggleLayerListButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToggleLayerListButton.Image = ((System.Drawing.Image)(resources.GetObject("ToggleLayerListButton.Image")));
            this.ToggleLayerListButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToggleLayerListButton.Name = "ToggleLayerListButton";
            this.ToggleLayerListButton.Size = new System.Drawing.Size(23, 22);
            this.ToggleLayerListButton.ToolTipText = "Toggle Layer list";
            this.ToggleLayerListButton.Click += new System.EventHandler(this.ToggleLayerListButton_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CursorPosition,
            this.CurrentScale,
            this.SpeedMeasure});
            this.statusStrip1.Location = new System.Drawing.Point(0, 327);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(363, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // CursorPosition
            // 
            this.CursorPosition.Name = "CursorPosition";
            this.CursorPosition.Size = new System.Drawing.Size(109, 17);
            this.CursorPosition.Text = "toolStripStatusLabel1";
            this.CursorPosition.ToolTipText = "The current mouse position";
            // 
            // CurrentScale
            // 
            this.CurrentScale.Name = "CurrentScale";
            this.CurrentScale.Size = new System.Drawing.Size(109, 17);
            this.CurrentScale.Text = "toolStripStatusLabel2";
            this.CurrentScale.ToolTipText = "The current map scale";
            // 
            // SpeedMeasure
            // 
            this.SpeedMeasure.Name = "SpeedMeasure";
            this.SpeedMeasure.Size = new System.Drawing.Size(109, 17);
            this.SpeedMeasure.Text = "toolStripStatusLabel1";
            this.SpeedMeasure.ToolTipText = "The time to render the last map";
            // 
            // LayerListBox
            // 
            this.LayerListBox.CheckOnClick = true;
            this.LayerListBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.LayerListBox.FormattingEnabled = true;
            this.LayerListBox.IntegralHeight = false;
            this.LayerListBox.Location = new System.Drawing.Point(243, 25);
            this.LayerListBox.Name = "LayerListBox";
            this.LayerListBox.Size = new System.Drawing.Size(120, 302);
            this.LayerListBox.TabIndex = 3;
            this.LayerListBox.Visible = false;
            this.LayerListBox.SelectedIndexChanged += new System.EventHandler(this.LayerListBox_SelectedIndexChanged);
            this.LayerListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.LayerListBox_ItemCheck);
            // 
            // frmMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 349);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.LayerListBox);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "frmMap";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.SizeChanged += new System.EventHandler(this.Form2_SizeChanged);
            this.Resize += new System.EventHandler(this.Form2_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton ZoomInButton;
        private System.Windows.Forms.ToolStripButton ZoomOutButton;
        private System.Windows.Forms.ToolStripButton PanButton;
        private System.Windows.Forms.ToolStripButton UnzoomButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel CursorPosition;
        private System.Windows.Forms.ToolStripStatusLabel CurrentScale;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton DisableScalesButton;
        private System.Windows.Forms.ToolStripStatusLabel SpeedMeasure;
        private System.Windows.Forms.ToolStripButton ToggleLayerListButton;
        private System.Windows.Forms.CheckedListBox LayerListBox;

    }
}