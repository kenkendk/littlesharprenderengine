using System;
using System.Collections.Generic;
using System.Text;
using LittleSharpRenderEngine;
using Topology.Geometries;
using System.Drawing;
using MapInfo;

namespace LSRETester
{
	public class MIFProvider : IProvider
	{
		private MapInfoLayer m_miffile;

		public MIFProvider(string mifpath)
		{
			try
			{
				m_miffile = MIFParser.LoadFile(mifpath);
			}
			catch (Exception ex)
			{
				throw new Exception("Couldn't read MIF file " + mifpath + "\nError: " + ex.Message);
			}
		}

		#region IProvider Members

		public IEnumerable<IFeature> GetFeatures(IGeometry geom, string filter, double scale)
		{
			LinkedList<IFeature> ret = new LinkedList<IFeature>();
			foreach (MapInfoObject f in m_miffile)
			{
				//linear cull
				if (geom.Intersects(f.Geometry)) ret.AddLast(f);
			}
			return ret;
		}

		public IEnumerable<IFeature> GetFeatures(IEnvelope bbox, string filter, double scale)
		{
			LinkedList<IFeature> ret = new LinkedList<IFeature>();
			foreach (MapInfoObject f in m_miffile)
			{
				//linear cull
				if (bbox.Intersects(f.Geometry.EnvelopeInternal)) ret.AddLast(f);
			}
			return ret;
		}

		public string ProviderName
		{
			get { return "MIFProvider"; }
		}

		public string DatasetName
		{
			get { return m_miffile.Name; }
		}

		public Topology.CoordinateSystems.ICoordinateSystem CoordinateSystem
		{
			get
			{
				//the coordinate system is in m_miffile.CoordinateSystem
				return null;
			}
		}

		public IEnvelope MaxBounds
		{
			get
			{
				IEnvelope env = null;
				foreach (MapInfoObject obj in m_miffile)
				{
					if (env == null) env = obj.Geometry.EnvelopeInternal;
					else env.ExpandToInclude(obj.Geometry.EnvelopeInternal);
				}
				return env;
			}
		}

		#endregion
	}
}
