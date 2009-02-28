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

            LittleSharpRenderEngine.IProvider provider = new WKTProvider(WKTFile.Text);

            Form2 dlg = new Form2(bounds, provider);
            dlg.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OSGeo.MapGuide.MaestroAPI.ServerConnectionI con = MapGuideProvider.MapGuideUtil.CreateConnection(
                MapGuideURL.Text,
                MapGuideUsername.Text,
                MapGuidePassword.Text);

            IEnvelope env = MapGuideProvider.MapGuideUtil.GetMapExtent(con, MapGuideResource.Text);

            List<MapGuideProvider.MapGuideLayer> layers = new List<MapGuideProvider.MapGuideLayer>();

            foreach(string s in MapGuideProvider.MapGuideUtil.EnumerateLayers(con, MapGuideResource.Text))
                try
                {
                    layers.Add(new MapGuideProvider.MapGuideLayer(con, s));
                }
                catch
                {
                    //Don't care about broken layers
                }

            layers.Reverse();

            Form2 dlg = new Form2(env, layers.ToArray());
            dlg.ShowDialog();
        }
    }
}