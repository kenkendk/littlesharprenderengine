using System;
using System.Collections.Generic;
using System.Text;
using LittleSharpRenderEngine;
using Topology.Geometries;
using System.Drawing;
using System.Data.OleDb;
using System.Data;

namespace LSRETester
{
	public class SQLServerProvider : IProvider, IDisposable
	{
		private OleDbConnection m_conn;
		private string m_tablename;
		private string m_geocolumnname;
		private LittleSharpRenderEngine.Style.IStyle m_areastyle;

		public SQLServerProvider(string connectionstring, string tablename, string geocolumn)
		{
			m_conn = new OleDbConnection(connectionstring);
			m_tablename = tablename;
			m_geocolumnname = geocolumn;

			//default area style
			LittleSharpRenderEngine.Style.Area a = new LittleSharpRenderEngine.Style.Area();
			a.Fill.ForegroundColor = Color.Blue;
			a.Outline.ForegroundColor = Color.Black;
			m_areastyle = a;
		}

		public SQLServerProvider(string connectionstring) : this(connectionstring, null, null)
		{
		}

		public string Table
		{
			get { return m_tablename; }
			set { m_tablename = value; }
		}

		public string GeoColumn
		{
			get { return m_geocolumnname; }
			set { m_geocolumnname = value; }
		}

		public string GetGeoColumn()
		{
			OpenConnection();
			try
			{
				DataTable schema = m_conn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, m_tablename });
				DataRow[] georows = schema.Select("SS_UDT_NAME = 'geometry'");

				return (georows != null && georows.Length > 0 ? georows[0]["COLUMN_NAME"].ToString() : null);
			}
			catch (Exception ex)
			{
				throw new Exception("Couldn't load geo column from table " + m_tablename + "\nError: " + ex.Message);
			}
		}

		private void OpenConnection()
		{
			try
			{
				if (m_conn.State != ConnectionState.Closed && m_conn.State != ConnectionState.Broken) return;
				m_conn.Open();
			}
			catch (Exception ex)
			{
				throw new Exception("Couldn't open connection to sql server\nError: " + ex.Message);
			}
		}

		public string[] GetTablenames()
		{
			OpenConnection();
			try
			{
				DataTable tablesschema = m_conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
				string[] tablenames = new string[tablesschema.Rows.Count];
				for (int i = 0; i < tablenames.Length; i++)
					tablenames[i] = tablesschema.Rows[i][2].ToString();
				return tablenames;
			}
			catch (Exception ex)
			{
				throw new Exception("Couldn't load table schema\nError: " + ex.Message);
			}
		}

		#region IProvider Members

		public IEnumerable<IFeature> GetFeatures(IEnvelope bbox, string filter, double scale)
		{
			IGeometry geom = new Topology.Geometries.Polygon(new LinearRing(new Coordinate[] { new Coordinate(bbox.MinX, bbox.MinY), new Coordinate(bbox.MaxX, bbox.MinY), new Coordinate(bbox.MaxX, bbox.MaxY), new Coordinate(bbox.MinX, bbox.MaxY), new Coordinate(bbox.MinX, bbox.MinY) }));
			return GetFeatures(geom, filter, scale);
		}

		public IEnumerable<IFeature> GetFeatures(IGeometry bbox, string filter, double scale)
		{
			OpenConnection();
			OleDbDataReader dr = null;
			Topology.IO.WKBReader wkb = new Topology.IO.WKBReader();
			LinkedList<IFeature> ret = new LinkedList<IFeature>();
			try
			{
				OleDbCommand cmd = m_conn.CreateCommand();
				//cmd.CommandText = "SELECT [" + m_geocolumnname + "].STAsBinary() FROM [" + m_tablename + "] WHERE geometry::STGeomFromWKB(?, 0).STIntersects([" + m_geocolumnname + "]) = 1";
				cmd.CommandText = "SELECT [" + m_geocolumnname + "].STAsBinary() FROM [" + m_tablename + "]";
				//cmd.Parameters.Add(bbox.AsBinary());
				dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					ret.AddLast( new SimpleFeature( wkb.Read((byte[])dr.GetValue(0)), m_areastyle));
				}

				return ret;
			}
			catch (Exception ex)
			{
				throw new Exception("Couldn't load features\nError: " + ex.Message);
			}
			finally
			{
				if (dr != null) dr.Close();
			}
		}

		public string ProviderName
		{
			get { return "SQL Server"; }
		}

		public string DatasetName
		{
			get { return m_tablename; }
		}

		public Topology.CoordinateSystems.ICoordinateSystem CoordinateSystem
		{
			get { return null; }
		}

		public IEnvelope MaxBounds
		{
			get 
			{
				OpenConnection();
				OleDbDataReader dr = null;
				try
				{
					//this is really slow I'm afraid
					OleDbCommand cmd = m_conn.CreateCommand();
					cmd.CommandText = "SELECT MIN([" + m_geocolumnname + "].STEnvelope().STPointN(1).STX), MIN([" + m_geocolumnname + "].STEnvelope().STPointN(1).STY), MAX([" + m_geocolumnname + "].STEnvelope().STPointN(3).STX), MAX([" + m_geocolumnname + "].STEnvelope().STPointN(3).STY) FROM [" + m_tablename + "]";
					dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
					if (!dr.Read()) throw new Exception("No bounds to load");
					Envelope e = new Envelope((double)dr.GetValue(0), (double)dr.GetValue(2), (double)dr.GetValue(1), (double)dr.GetValue(3));
					return e;
				}
				catch (Exception ex)
				{
					throw new Exception("Couldn't load max bounds\nError: " + ex.Message);
				}
				finally
				{
					if (dr != null) dr.Close();
				}
			}
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			try
			{
				if (m_conn != null) m_conn.Close();
			}
			catch
			{ }
		}

		#endregion
	}
}
