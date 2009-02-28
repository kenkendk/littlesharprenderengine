namespace LSRETester
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.WKTFile = new System.Windows.Forms.TextBox();
            this.MapGuidePassword = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.MapGuideURL = new System.Windows.Forms.TextBox();
            this.MapGuideUsername = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.MapGuideResource = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
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
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button1.Location = new System.Drawing.Point(240, 72);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Render file";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
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
            // button2
            // 
            this.button2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button2.Location = new System.Drawing.Point(240, 232);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(112, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "Render MapGuide";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
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
            this.MapGuideURL.Text = "http://localhost/mapguide/";
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(595, 362);
            this.Controls.Add(this.MapGuideResource);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.MapGuidePassword);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.MapGuideUsername);
            this.Controls.Add(this.MapGuideURL);
            this.Controls.Add(this.WKTFile);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BoundWKT);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox BoundWKT;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox WKTFile;
        private System.Windows.Forms.TextBox MapGuidePassword;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox MapGuideURL;
        private System.Windows.Forms.TextBox MapGuideUsername;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox MapGuideResource;
        private System.Windows.Forms.Label label3;
    }
}

