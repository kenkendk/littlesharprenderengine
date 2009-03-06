using System;
using System.Collections.Generic;
using System.Text;
using LittleSharpRenderEngine;
using Topology.Geometries;
using Topology.Index.Quadtree;
using System.Drawing;
using System.Data;

namespace LSRETester
{
	/// <summary>
	/// I just couldn't think of a longer name. HA HA!
	/// </summary>
	public class GenericWKBDatabaseWithBoundingBoxColumns : IProvider, IDisposable
	{
		private IDbConnection m_conn;
		private string m_tablename = "";
		private string m_WKBColumn = "MapObject";
		private string m_X1Column = "X1";
		private string m_Y1Column = "Y1";
		private string m_X2Column = "X2";
		private string m_Y2Column = "Y2";
		private string m_PrimaryColumn = "";
		private Quadtree m_objects = new Quadtree();
		private Quadtree m_cachedareas = new Quadtree();
		private LittleSharpRenderEngine.Style.IStyle m_areastyle;
		private Dictionary<int, IFeature> m_PrimaryKeyIndex = new Dictionary<int, IFeature>();

		public string Tablename { get { return m_tablename; } set { m_tablename = value; } }
		public string WKBColumn { get { return m_WKBColumn; } set { m_WKBColumn = value; } }
		public string X1Column { get { return m_X1Column; } set { m_X1Column = value; } }
		public string Y1Column { get { return m_Y1Column; } set { m_Y1Column = value; } }
		public string X2Column { get { return m_X2Column; } set { m_X2Column = value; } }
		public string Y2Column { get { return m_Y2Column; } set { m_Y2Column = value; } }
		public string PrimaryColumn { get { return m_PrimaryColumn; } set { m_PrimaryColumn = value; } }

		public GenericWKBDatabaseWithBoundingBoxColumns(IDbConnection connection, string tablename, string primarycolumn)
		{
			m_conn = connection;
			m_tablename = tablename;
			m_PrimaryColumn = primarycolumn;

			//default area style
			LittleSharpRenderEngine.Style.Area a = new LittleSharpRenderEngine.Style.Area();
			a.Fill.ForegroundColor = Color.Blue;
			a.Outline.ForegroundColor = Color.Black;
			m_areastyle = a;
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

		#region IProvider Members

		public IEnumerable<IFeature> GetFeatures(IEnvelope bbox, string filter, double scale)
		{
			//are we cached?
			Polygon p = null;
			foreach (IEnvelope area in m_cachedareas.Query(bbox))
			{
				if (p == null) p = ConvertEnvelopeToPolygon(area);
				else p.Union(ConvertEnvelopeToPolygon(area));
			}
			if(p != null && p.Covers(ConvertEnvelopeToPolygon(bbox)))
			{
				//load from cache
				LinkedList<IFeature> ret = new LinkedList<IFeature>();
				m_objects.Query(bbox, new FeatureLinkedListVisitor(ret));
				return ret;
			}

			OpenConnection();

			//load from DB
			IDbCommand cmd = m_conn.CreateCommand();
			cmd.CommandText = "SELECT " + m_PrimaryColumn + ", " + m_WKBColumn + " FROM " + m_tablename + " WHERE "
				+ m_X1Column + " >= " + bbox.MinX.ToString(System.Globalization.CultureInfo.InvariantCulture) + " AND " + m_X1Column + " <= " + bbox.MaxX.ToString(System.Globalization.CultureInfo.InvariantCulture) + " AND " + m_Y1Column + " >= " + bbox.MinY.ToString(System.Globalization.CultureInfo.InvariantCulture) + " AND " + m_Y1Column + " <= " + bbox.MaxY.ToString(System.Globalization.CultureInfo.InvariantCulture) + " OR "
				+ m_X2Column + " >= " + bbox.MinX.ToString(System.Globalization.CultureInfo.InvariantCulture) + " AND " + m_X2Column + " <= " + bbox.MaxX.ToString(System.Globalization.CultureInfo.InvariantCulture) + " AND " + m_Y1Column + " >= " + bbox.MinY.ToString(System.Globalization.CultureInfo.InvariantCulture) + " AND " + m_Y1Column + " <= " + bbox.MaxY.ToString(System.Globalization.CultureInfo.InvariantCulture) + " OR "
				+ m_X2Column + " >= " + bbox.MinX.ToString(System.Globalization.CultureInfo.InvariantCulture) + " AND " + m_X2Column + " <= " + bbox.MaxX.ToString(System.Globalization.CultureInfo.InvariantCulture) + " AND " + m_Y2Column + " >= " + bbox.MinY.ToString(System.Globalization.CultureInfo.InvariantCulture) + " AND " + m_Y2Column + " <= " + bbox.MaxY.ToString(System.Globalization.CultureInfo.InvariantCulture) + " OR "
				+ m_X1Column + " >= " + bbox.MinX.ToString(System.Globalization.CultureInfo.InvariantCulture) + " AND " + m_X1Column + " <= " + bbox.MaxX.ToString(System.Globalization.CultureInfo.InvariantCulture) + " AND " + m_Y2Column + " >= " + bbox.MinY.ToString(System.Globalization.CultureInfo.InvariantCulture) + " AND " + m_Y2Column + " <= " + bbox.MaxY.ToString(System.Globalization.CultureInfo.InvariantCulture);

			IDataReader dr = null;
			LinkedList<IFeature> fromdb = new LinkedList<IFeature>();
			try
			{
				dr = cmd.ExecuteReader();
				Topology.IO.WKBReader wkbreader = new Topology.IO.WKBReader();
				int nmap = dr.GetOrdinal(m_WKBColumn);
				int npk = dr.GetOrdinal(m_PrimaryColumn);
				while (dr.Read())
				{
					IGeometry g = wkbreader.Read((byte[])dr.GetValue(nmap));
					int pk = (int)dr.GetValue(npk);
					SimpleFeature f = new SimpleFeature(g, m_areastyle, pk);
					if (!m_PrimaryKeyIndex.ContainsKey(pk))
					{
						fromdb.AddLast(f);
						m_PrimaryKeyIndex.Add(pk, f);
						m_objects.Insert(g.EnvelopeInternal, f);
					}
					else fromdb.AddLast(m_PrimaryKeyIndex[pk]);
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Couldn't load features\nError: " + ex.Message);
			}
			finally
			{
				if (dr != null) dr.Close();
			}

			//add to cache
			m_cachedareas.Insert(bbox, bbox);

			return fromdb;
				
		}

		public static Polygon ConvertEnvelopeToPolygon(IEnvelope bbox)
		{
			return new Topology.Geometries.Polygon(new LinearRing(new Coordinate[] { new Coordinate(bbox.MinX, bbox.MinY), new Coordinate(bbox.MaxX, bbox.MinY), new Coordinate(bbox.MaxX, bbox.MaxY), new Coordinate(bbox.MinX, bbox.MaxY), new Coordinate(bbox.MinX, bbox.MinY)}));
		}

		public IEnumerable<IFeature> GetFeatures(IGeometry geom, string filter, double scale)
		{
			return GetFeatures(geom.EnvelopeInternal, filter, scale);
		}

		public string ProviderName
		{
			get { return "GenericWKBDatabaseWithBoundingBoxColumns"; }
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

				IDbCommand cmd = m_conn.CreateCommand();
				cmd.CommandText = "SELECT MIN(" + m_X1Column + "), MIN(" + m_Y1Column + "), MAX("+ m_X2Column + "), MAX(" + m_Y2Column + ") FROM " + m_tablename;

				IDataReader dr = null;
				try
				{
					dr = cmd.ExecuteReader();
					if (dr.Read()) return new Envelope(dr.GetDouble(0), dr.GetDouble(2), dr.GetDouble(1), dr.GetDouble(3));
					return null;
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
