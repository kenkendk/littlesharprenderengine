namespace LSRETester
{
    partial class frmConnect
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
			this.BoundWKT = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.m_RenderWKTButton = new System.Windows.Forms.Button();
			this.WKTFile = new System.Windows.Forms.TextBox();
			this.MapGuidePassword = new System.Windows.Forms.TextBox();
			this.m_RenderMapGuideButton = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.MapGuideURL = new System.Windows.Forms.TextBox();
			this.MapGuideUsername = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.MapGuideResource = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.m_mapinfofiletext = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.m_MapInfoFileButton = new System.Windows.Forms.Button();
			this.m_RenderMapInfoButton = new System.Windows.Forms.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.m_sqlserverconnectiontext = new System.Windows.Forms.TextBox();
			this.m_rendersqlserverbutton = new System.Windows.Forms.Button();
			this.m_sqlservertablecombo = new System.Windows.Forms.ComboBox();
			this.label9 = new System.Windows.Forms.Label();
			this.m_rendermifbutton = new System.Windows.Forms.Button();
			this.m_mifpathtext = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// BoundWKT
			// 
			this.BoundWKT.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.BoundWKT.Location = new System.Drawing.Point(88, 16);
			this.BoundWKT.Name = "BoundWKT";
			this.BoundWKT.Size = new System.Drawing.Size(496, 20);
			this.BoundWKT.TabIndex = 0;
			this.BoundWKT.Text = "Multipoint(528339 6317817, 579780 6277741)";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(24, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(43, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Bounds";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(24, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Features";
			// 
			// m_RenderWKTButton
			// 
			this.m_RenderWKTButton.Location = new System.Drawing.Point(240, 66);
			this.m_RenderWKTButton.Name = "m_RenderWKTButton";
			this.m_RenderWKTButton.Size = new System.Drawing.Size(104, 23);
			this.m_RenderWKTButton.TabIndex = 6;
			this.m_RenderWKTButton.Text = "Render file";
			this.m_RenderWKTButton.UseVisualStyleBackColor = true;
			this.m_RenderWKTButton.Click += new System.EventHandler(this.m_RenderWKTButton_Click);
			// 
			// WKTFile
			// 
			this.WKTFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.WKTFile.Location = new System.Drawing.Point(88, 40);
			this.WKTFile.Name = "WKTFile";
			this.WKTFile.Size = new System.Drawing.Size(496, 20);
			this.WKTFile.TabIndex = 7;
			this.WKTFile.Text = "..\\..\\sampledata.wkt";
			// 
			// MapGuidePassword
			// 
			this.MapGuidePassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.MapGuidePassword.Location = new System.Drawing.Point(88, 178);
			this.MapGuidePassword.Name = "MapGuidePassword";
			this.MapGuidePassword.Size = new System.Drawing.Size(496, 20);
			this.MapGuidePassword.TabIndex = 14;
			this.MapGuidePassword.Text = "admin";
			// 
			// m_RenderMapGuideButton
			// 
			this.m_RenderMapGuideButton.Location = new System.Drawing.Point(240, 232);
			this.m_RenderMapGuideButton.Name = "m_RenderMapGuideButton";
			this.m_RenderMapGuideButton.Size = new System.Drawing.Size(112, 23);
			this.m_RenderMapGuideButton.TabIndex = 13;
			this.m_RenderMapGuideButton.Text = "Render MapGuide";
			this.m_RenderMapGuideButton.UseVisualStyleBackColor = true;
			this.m_RenderMapGuideButton.Click += new System.EventHandler(this.m_RenderMapGuideButton_Click);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(24, 178);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(53, 13);
			this.label5.TabIndex = 11;
			this.label5.Text = "Password";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(24, 130);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(38, 13);
			this.label6.TabIndex = 10;
			this.label6.Text = "Server";
			// 
			// MapGuideURL
			// 
			this.MapGuideURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.MapGuideURL.Location = new System.Drawing.Point(88, 130);
			this.MapGuideURL.Name = "MapGuideURL";
			this.MapGuideURL.Size = new System.Drawing.Size(496, 20);
			this.MapGuideURL.TabIndex = 8;
			this.MapGuideURL.Text = "http://geomgos/mapguide/";
			// 
			// MapGuideUsername
			// 
			this.MapGuideUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.MapGuideUsername.Location = new System.Drawing.Point(88, 154);
			this.MapGuideUsername.Name = "MapGuideUsername";
			this.MapGuideUsername.Size = new System.Drawing.Size(496, 20);
			this.MapGuideUsername.TabIndex = 9;
			this.MapGuideUsername.Text = "Administrator";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(24, 154);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(55, 13);
			this.label4.TabIndex = 12;
			this.label4.Text = "Username";
			// 
			// MapGuideResource
			// 
			this.MapGuideResource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.MapGuideResource.Location = new System.Drawing.Point(88, 208);
			this.MapGuideResource.Name = "MapGuideResource";
			this.MapGuideResource.Size = new System.Drawing.Size(496, 20);
			this.MapGuideResource.TabIndex = 16;
			this.MapGuideResource.Text = "Library://Samples/Sheboygan/Maps/Sheboygan.MapDefinition";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(24, 208);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(53, 13);
			this.label3.TabIndex = 15;
			this.label3.Text = "Resource";
			// 
			// m_mapinfofiletext
			// 
			this.m_mapinfofiletext.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_mapinfofiletext.Location = new System.Drawing.Point(88, 272);
			this.m_mapinfofiletext.Name = "m_mapinfofiletext";
			this.m_mapinfofiletext.Size = new System.Drawing.Size(464, 20);
			this.m_mapinfofiletext.TabIndex = 18;
			this.m_mapinfofiletext.Text = "D:\\workspace\\MI_Byggesag\\MapInfo-modul\\Test\\Grundkort\\MAT_REG.TAB";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(24, 272);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(23, 13);
			this.label7.TabIndex = 17;
			this.label7.Text = "File";
			// 
			// m_MapInfoFileButton
			// 
			this.m_MapInfoFileButton.Location = new System.Drawing.Point(552, 272);
			this.m_MapInfoFileButton.Name = "m_MapInfoFileButton";
			this.m_MapInfoFileButton.Size = new System.Drawing.Size(32, 20);
			this.m_MapInfoFileButton.TabIndex = 19;
			this.m_MapInfoFileButton.Text = "...";
			this.m_MapInfoFileButton.UseVisualStyleBackColor = true;
			this.m_MapInfoFileButton.Click += new System.EventHandler(this.m_MapInfoFileButton_Click);
			// 
			// m_RenderMapInfoButton
			// 
			this.m_RenderMapInfoButton.Location = new System.Drawing.Point(240, 296);
			this.m_RenderMapInfoButton.Name = "m_RenderMapInfoButton";
			this.m_RenderMapInfoButton.Size = new System.Drawing.Size(112, 24);
			this.m_RenderMapInfoButton.TabIndex = 20;
			this.m_RenderMapInfoButton.Text = "Render MapInfo";
			this.m_RenderMapInfoButton.UseVisualStyleBackColor = true;
			this.m_RenderMapInfoButton.Click += new System.EventHandler(this.m_RenderMapInfoButton_Click);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(24, 336);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(61, 13);
			this.label8.TabIndex = 22;
			this.label8.Text = "Connection";
			// 
			// m_sqlserverconnectiontext
			// 
			this.m_sqlserverconnectiontext.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_sqlserverconnectiontext.Location = new System.Drawing.Point(88, 336);
			this.m_sqlserverconnectiontext.Name = "m_sqlserverconnectiontext";
			this.m_sqlserverconnectiontext.Size = new System.Drawing.Size(496, 20);
			this.m_sqlserverconnectiontext.TabIndex = 21;
			this.m_sqlserverconnectiontext.Text = "Provider=sqloledb;Server=GeografSQL;Database=Testdatabase;Uid=mkv;Pwd=kvist;";
			// 
			// m_rendersqlserverbutton
			// 
			this.m_rendersqlserverbutton.Location = new System.Drawing.Point(240, 392);
			this.m_rendersqlserverbutton.Name = "m_rendersqlserverbutton";
			this.m_rendersqlserverbutton.Size = new System.Drawing.Size(112, 24);
			this.m_rendersqlserverbutton.TabIndex = 23;
			this.m_rendersqlserverbutton.Text = "Render SQL server";
			this.m_rendersqlserverbutton.UseVisualStyleBackColor = true;
			this.m_rendersqlserverbutton.Click += new System.EventHandler(this.m_rendersqlserverbutton_Click);
			// 
			// m_sqlservertablecombo
			// 
			this.m_sqlservertablecombo.FormattingEnabled = true;
			this.m_sqlservertablecombo.Location = new System.Drawing.Point(88, 360);
			this.m_sqlservertablecombo.Name = "m_sqlservertablecombo";
			this.m_sqlservertablecombo.Size = new System.Drawing.Size(176, 21);
			this.m_sqlservertablecombo.TabIndex = 24;
			this.m_sqlservertablecombo.Text = "MAT_REG";
			this.m_sqlservertablecombo.GotFocus += new System.EventHandler(this.m_sqlservertablecombo_GotFocus);
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(24, 360);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(34, 13);
			this.label9.TabIndex = 25;
			this.label9.Text = "Table";
			// 
			// m_rendermifbutton
			// 
			this.m_rendermifbutton.Location = new System.Drawing.Point(240, 456);
			this.m_rendermifbutton.Name = "m_rendermifbutton";
			this.m_rendermifbutton.Size = new System.Drawing.Size(112, 24);
			this.m_rendermifbutton.TabIndex = 29;
			this.m_rendermifbutton.Text = "Render MIF";
			this.m_rendermifbutton.UseVisualStyleBackColor = true;
			this.m_rendermifbutton.Click += new System.EventHandler(this.m_rendermifbutton_Click);
			// 
			// m_mifpathtext
			// 
			this.m_mifpathtext.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_mifpathtext.Location = new System.Drawing.Point(88, 432);
			this.m_mifpathtext.Name = "m_mifpathtext";
			this.m_mifpathtext.Size = new System.Drawing.Size(496, 20);
			this.m_mifpathtext.TabIndex = 27;
			this.m_mifpathtext.Text = "..\\..\\Testdata\\testpolykom.mif";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(24, 432);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(23, 13);
			this.label10.TabIndex = 26;
			this.label10.Text = "File";
			// 
			// frmConnect
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(595, 560);
			this.Controls.Add(this.m_rendermifbutton);
			this.Controls.Add(this.m_mifpathtext);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.m_sqlservertablecombo);
			this.Controls.Add(this.m_rendersqlserverbutton);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.m_sqlserverconnectiontext);
			this.Controls.Add(this.m_RenderMapInfoButton);
			this.Controls.Add(this.m_MapInfoFileButton);
			this.Controls.Add(this.m_mapinfofiletext);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.MapGuideResource);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.MapGuidePassword);
			this.Controls.Add(this.m_RenderMapGuideButton);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.MapGuideUsername);
			this.Controls.Add(this.MapGuideURL);
			this.Controls.Add(this.WKTFile);
			this.Controls.Add(this.m_RenderWKTButton);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.BoundWKT);
			this.Name = "frmConnect";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox BoundWKT;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button m_RenderWKTButton;
        private System.Windows.Forms.TextBox WKTFile;
        private System.Windows.Forms.TextBox MapGuidePassword;
        private System.Windows.Forms.Button m_RenderMapGuideButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox MapGuideURL;
        private System.Windows.Forms.TextBox MapGuideUsername;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox MapGuideResource;
        private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox m_mapinfofiletext;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Button m_MapInfoFileButton;
		private System.Windows.Forms.Button m_RenderMapInfoButton;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox m_sqlserverconnectiontext;
		private System.Windows.Forms.Button m_rendersqlserverbutton;
		private System.Windows.Forms.ComboBox m_sqlservertablecombo;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Button m_rendermifbutton;
		private System.Windows.Forms.TextBox m_mifpathtext;
		private System.Windows.Forms.Label label10;
    }
}

