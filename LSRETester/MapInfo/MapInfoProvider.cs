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
        private string m_filename;
		private Topology.Index.Quadtree.Quadtree m_tree = new Topology.Index.Quadtree.Quadtree();

		public MapInfoProvider(string tabpath)
		{
			try
			{
				m_layer = new Layer(tabpath);
				m_filename = tabpath;
				BuildTree();
			}
			catch (Exception ex)
			{
				throw new Exception("Couldn't load file " + tabpath + "\nError: " + ex.Message);
			}
		}

		private void BuildTree()
		{
			foreach (Feature f in m_layer.Features)
			{
				m_tree.Insert(f.Geometry.EnvelopeInternal, f);
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

		public IEnumerable<IFeature> GetFeatures(IGeometry geom, string filter, double scale)
		{
			if (geom is IPoint)
			{
				IPoint p = (IPoint)geom;
				Envelope e = new Envelope(p.X, p.X, p.Y, p.Y);
				LinkedList<IFeature> ret = new LinkedList<IFeature>();
				m_tree.Query(e, new FeatureLinkedListVisitor(ret));
				return ret;
			}
			else
			{
				LinkedList<IFeature> ret = new LinkedList<IFeature>();
				foreach (Feature f in m_layer.Features)
				{
					//linear cull
					if (geom.Intersects(f.Geometry)) ret.AddLast(f);
				}
				return ret;
			}
		}

		public IEnumerable<IFeature> GetFeatures(IEnvelope bbox, string filter, double scale)
		{
			LinkedList<IFeature> ret = new LinkedList<IFeature>();
			m_tree.Query(bbox, new FeatureLinkedListVisitor(ret));
			return ret;
		}

        public string ProviderName
        {
            get { return "MapInfo Provider"; }
        }

        public string DatasetName
        {
            get { return System.IO.Path.GetFileNameWithoutExtension(m_filename); }
        }

        public Topology.CoordinateSystems.ICoordinateSystem CoordinateSystem { get { return null; } }

    }
}
