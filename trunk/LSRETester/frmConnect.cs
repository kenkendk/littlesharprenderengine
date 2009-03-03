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
    public partial class frmConnect : Form
    {
        public frmConnect()
        {
            InitializeComponent();
        }

        private void m_RenderWKTButton_Click(object sender, EventArgs e)
        {
            Topology.IO.WKTReader rd = new Topology.IO.WKTReader();
            IEnvelope bounds = rd.Read(BoundWKT.Text).EnvelopeInternal;

            LittleSharpRenderEngine.IProvider provider = new WKTProvider(WKTFile.Text);

			//show map
            frmMap dlg = new frmMap(bounds, provider);
            dlg.ShowDialog();
        }

        private void m_RenderMapGuideButton_Click(object sender, EventArgs e)
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

            frmMap dlg = new frmMap(env, layers.ToArray());
            dlg.ShowDialog();
        }

		private void m_MapInfoFileButton_Click(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Title = "Select MapInfo-file";
			dlg.Filter = "TAB|*.tab";
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				m_mapinfofiletext.Text = dlg.FileName;
			}
		}

		private void m_RenderMapInfoButton_Click(object sender, EventArgs e)
		{
			MapInfoProvider layer = null;
			try
			{
				layer = new MapInfoProvider(m_mapinfofiletext.Text);
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "Kunne ikke åbne fil\nFejl: " + ex.Message, "Fejl", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			//show map
			frmMap dlg = new frmMap(layer.MaxBounds, layer);
			dlg.ShowDialog();
		}
    }
}