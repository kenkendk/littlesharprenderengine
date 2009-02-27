using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Topology.Geometries;

namespace LSRETester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Topology.IO.WKTReader rd = new Topology.IO.WKTReader();
            IEnvelope bounds = rd.Read(BoundWKT.Text).EnvelopeInternal;

            LittleSharpRenderEngine.LittleSharpRenderEngine engine = new LittleSharpRenderEngine.LittleSharpRenderEngine(bounds, null, new Size(1024, 768), Color.Transparent);
            LittleSharpRenderEngine.IProvider provider = new WKTProvider(WKTFile.Text);

            engine.RenderFeatures(null, provider.GetFeatures(bounds, null));

            Form2 dlg = new Form2();
            dlg.pictureBox1.Image = engine.Bitmap;
            dlg.Size = dlg.pictureBox1.Image.Size;
            dlg.ShowDialog();
        }
    }
}