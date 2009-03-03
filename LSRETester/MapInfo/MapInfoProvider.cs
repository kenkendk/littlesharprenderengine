using System;
using System.Collections.Generic;
using System.Text;
using LittleSharpRenderEngine;
using Topology.Geometries;
using System.Drawing;
using EBop.MapObjects.MapInfo;

namespace LSRETester
{
	public class MapInfoProvider : IProvider
	{
		private Layer m_layer;

		public MapInfoProvider(string tabpath)
		{
			try
			{
				m_layer = new Layer(tabpath);
			}
			catch (Exception ex)
			{
				throw new Exception("Couldn't load file " + tabpath + "\nError: " + ex.Message);
			}
		}

		public IEnvelope MaxBounds
		{
			get
			{
				IEnvelope env = null;
				foreach (Feature f in m_layer.Features)
				{
					if (env == null) env = f.Geometry.EnvelopeInternal;
					else env.ExpandToInclude(f.Geometry.EnvelopeInternal);
				}
				return env;
			}
		}

		public IEnumerable<IFeature> GetFeatures(IEnvelope bbox, string filter, float scale)
		{
			LinkedList<IFeature> ret = new LinkedList<IFeature>();
			foreach (Feature f in m_layer.Features)
			{
				//linear cull
				if(bbox.Intersects(f.Geometry.EnvelopeInternal)) ret.AddLast(f);
			}
			return ret;
		}
	}
}
