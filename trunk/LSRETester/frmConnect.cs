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
            frmMap dlg = new frmMap(bounds, null, provider);
            dlg.ShowDialog();
        }

        private void m_RenderMapGuideButton_Click(object sender, EventArgs e)
        {
            OSGeo.MapGuide.MaestroAPI.ServerConnectionI con = MapGuideProvider.MapGuideUtil.CreateConnection(
                MapGuideURL.Text,
                MapGuideUsername.Text,
                MapGuidePassword.Text);

            IEnvelope env = MapGuideProvider.MapGuideUtil.GetMapExtent(con, MapGuideResource.Text);
            Topology.CoordinateSystems.ICoordinateSystem coordSys = null;

            try
            {
                coordSys = MapGuideProvider.MapGuideUtil.GetCoordinateSystem(con, MapGuideResource.Text);
            }
            catch { }

            List<MapGuideProvider.MapGuideLayer> layers = new List<MapGuideProvider.MapGuideLayer>();

            foreach(string s in MapGuideProvider.MapGuideUtil.EnumerateLayers(con, MapGuideResource.Text, true))
                try
                {
                    layers.Add(new MapGuideProvider.MapGuideLayer(con, s));
                }
                catch
                {
                    //Don't care about broken layers
                }

            layers.Reverse();

            frmMap dlg = new frmMap(env, coordSys, layers.ToArray());
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
				MessageBox.Show(this, "Could not open file\nError: " + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			//show map
			frmMap dlg = new frmMap(layer.MaxBounds, layer.CoordinateSystem, layer);
			dlg.ShowDialog();
		}
    }
}