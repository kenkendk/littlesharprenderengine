using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Topology.Geometries;
using System.Data.OleDb;

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

		private void m_sqlservertablecombo_GotFocus(object sender, System.EventArgs e)
		{
			try
			{
				if (m_sqlservertablecombo.DataSource != null) return;
				SQLServerProvider conn = new SQLServerProvider(m_sqlserverconnectiontext.Text);
				string[] tables = conn.GetTablenames();
				m_sqlservertablecombo.DataSource = tables;
				if (tables == null) m_sqlservertablecombo.DataSource = new string[] { };	//don't load again
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "Fejl under forbindelse til SQL Server\nFejl: " + ex.Message, "Fejl", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void m_rendersqlserverbutton_Click(object sender, EventArgs e)
		{
			SQLServerProvider layer = new SQLServerProvider(m_sqlserverconnectiontext.Text, m_sqlservertablecombo.Text, null);

			//test connection
			try
			{
				layer.GeoColumn = layer.GetGeoColumn();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "Fejl under forbindelse til SQL Server\nFejl: " + ex.Message, "Fejl", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			//show map
			frmMap dlg = new frmMap(layer.MaxBounds, layer.CoordinateSystem, layer);
			dlg.ShowDialog();
		}

		private void m_rendermifbutton_Click(object sender, EventArgs e)
		{
			MIFProvider layer = null;
			try
			{
				layer = new MIFProvider(m_mifpathtext.Text);
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

		private void m_genericdbbutton_Click(object sender, EventArgs e)
		{
			GenericWKBDatabaseWithBoundingBoxColumns layer = null;
			try
			{
				layer = new GenericWKBDatabaseWithBoundingBoxColumns(new OleDbConnection( m_genericdbconnectiontext.Text), "testpolykom", "ID");
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "Could not open connection\nError: " + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			//show map
			frmMap dlg = new frmMap(layer.MaxBounds, layer.CoordinateSystem, layer);
			dlg.ShowDialog();
		}

		private void m_uploadmiftodbbutton_Click(object sender, EventArgs e)
		{
			OleDbConnection conn = new OleDbConnection(m_genericdbconnectiontext.Text);
			conn.Open();

			//load mif
			MapInfo.MapInfoLayer layer = MapInfo.MIFParser.LoadFile(m_mifpathtext.Text);

			//delete existing table
			try
			{
				OleDbCommand cmd = conn.CreateCommand();
				cmd.CommandText	= "DROP TABLE " + layer.Name;
				cmd.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
			}

			//create
			string sql = "CREATE TABLE " + layer.Name + "(ID AUTOINCREMENT, ";
			for(int i = 0; i < layer.ColumnNames.Length; i++)
			{
				sql += layer.ColumnNames[i] + " " + layer.ColumnTypes[i] + ", ";
			}
			sql += "X1 FLOAT, Y1 FLOAT, X2 FLOAT, Y2 FLOAT, MapObject OLEOBJECT)";
			try
			{
				OleDbCommand cmd = conn.CreateCommand();
				cmd.CommandText= sql;
				cmd.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				throw new Exception("BAH! " + ex.Message);
			}

			//fill
			foreach (MapInfo.MapInfoObject obj in layer)
			{
				sql = "INSERT INTO " + layer.Name + "(";
				for (int i = 0; i < layer.ColumnNames.Length; i++)
				{
					sql += layer.ColumnNames[i] + ", ";
				}
				sql += "X1, Y1, X2, Y2, MapObject) VALUES(";
				for (int i = 0; i < layer.ColumnNames.Length; i++)
				{
					sql += "?, ";
				}

				IEnvelope env = obj.Geometry.EnvelopeInternal;
				sql += env.MinX.ToString(System.Globalization.CultureInfo.InvariantCulture) + ", ";
				sql += env.MinY.ToString(System.Globalization.CultureInfo.InvariantCulture) + ", ";
				sql += env.MaxX.ToString(System.Globalization.CultureInfo.InvariantCulture) + ", ";
				sql += env.MaxY.ToString(System.Globalization.CultureInfo.InvariantCulture) + ", ";
				sql += "?)";

				try
				{
					OleDbCommand cmd = conn.CreateCommand();
					cmd.CommandText = sql;
					for (int i = 0; i < layer.ColumnNames.Length; i++)
					{
						OleDbParameter p = cmd.CreateParameter();
						p.Value = obj.GetColumnValue(i);
						cmd.Parameters.Add(p);
					}
					OleDbParameter blob = cmd.CreateParameter();
					blob.Value = obj.Geometry.AsBinary();
					cmd.Parameters.Add(blob);
					cmd.ExecuteNonQuery();
				}
				catch(Exception ex)
				{
					throw new Exception("BAH! " + ex.Message);
				}

			}
		}
    }
}