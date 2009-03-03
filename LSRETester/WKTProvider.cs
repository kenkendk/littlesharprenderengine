using System;
using System.Collections.Generic;
using System.Text;
using LittleSharpRenderEngine;
using Topology.Geometries;
using System.Drawing;

namespace LSRETester
{
    public class WKTProvider: IProvider
    {
        private List<IGeometry> m_geometries;
        private LittleSharpRenderEngine.Style.IPointStyle m_pointStyle;
        private LittleSharpRenderEngine.Style.IAreaStyle m_areaStyle;
        private LittleSharpRenderEngine.Style.ILineStyle m_lineStyle;
        private string m_name;

        public WKTProvider(string filename)
        {
            m_geometries = new List<IGeometry>();
            SetDefaultStyles();

            Topology.IO.WKTReader rd = new Topology.IO.WKTReader();
            foreach (string x in System.IO.File.ReadAllLines(filename))
                if (x.Trim().Length > 0)
                    m_geometries.Add(rd.Read(x));

            m_name = System.IO.Path.GetFileNameWithoutExtension(filename);
        }

		public IEnvelope MaxBounds
		{
			get
			{
				throw new NotImplementedException();
			}
		}

        private void SetDefaultStyles()
        {
            LittleSharpRenderEngine.Style.Point p = new LittleSharpRenderEngine.Style.Point();
            LittleSharpRenderEngine.Style.Line l = new LittleSharpRenderEngine.Style.Line();
            LittleSharpRenderEngine.Style.Area a = new LittleSharpRenderEngine.Style.Area();

            p.Center = new System.Drawing.Point(5, 5);
            p.Size = new Size(10, 10);
            p.Type = LittleSharpRenderEngine.Style.Point.PointType.Triangle;
            p.Fill = new LittleSharpRenderEngine.Style.Base.Fill();
            p.Fill.BackgroundColor = Color.Red;
            p.Fill.ForegroundColor = Color.Red;
            p.Outline = new LittleSharpRenderEngine.Style.Base.Outline();
            p.Outline.BackgroundColor = Color.Yellow;
            p.Outline.ForegroundColor = Color.Black;
            p.Outline.Width = 3;

            LittleSharpRenderEngine.Style.Base.Outline o = new LittleSharpRenderEngine.Style.Base.Outline();
            o.BackgroundColor = Color.Blue;
            o.ForegroundColor = Color.Blue;
            o.Width = 1;
            ((IList<LittleSharpRenderEngine.Style.IOutline>)l.Outlines).Add(o);

            a.Fill = new LittleSharpRenderEngine.Style.Base.Fill();
            a.Fill.BackgroundColor = Color.LightGreen;
            a.Fill.ForegroundColor = Color.LightGreen;
            a.Outline = new LittleSharpRenderEngine.Style.Base.Outline();
            a.Outline.BackgroundColor = Color.Green;
            a.Outline.ForegroundColor = Color.Green;
            a.Outline.Width = 1;

            m_pointStyle = p;
            m_lineStyle = l;
            m_areaStyle = a;
        }

        #region IProvider Members

        public IEnumerable<IFeature> GetFeatures(Topology.Geometries.IEnvelope bbox, string filter, double scale)
        {
            List<IFeature> feats = new List<IFeature>();
			foreach (IGeometry geom in m_geometries)
			{
				if (bbox != null || geom.EnvelopeInternal.Intersects(bbox))
					if (geom is IPoint)
						feats.Add(new SimpleFeature(geom, m_pointStyle));
					else if (geom is ILineString)
						feats.Add(new SimpleFeature(geom, m_lineStyle));
					else if (geom is IPolygon)
						feats.Add(new SimpleFeature(geom, m_areaStyle));
			}

            return feats;
        }

        public string ProviderName
        {
            get { return "WKT Provider"; }
        }

        public string DatasetName
        {
            get { return m_name; }
        }

        public Topology.CoordinateSystems.ICoordinateSystem CoordinateSystem { get { return null; } }

        #endregion

        public LittleSharpRenderEngine.Style.IPointStyle PointStyle
        {
            get { return m_pointStyle; }
            set { m_pointStyle = value; }
        }

        public LittleSharpRenderEngine.Style.ILineStyle LineStyle
        {
            get { return m_lineStyle; }
            set { m_lineStyle = value; }
        }

        public LittleSharpRenderEngine.Style.IAreaStyle AreaStyle
        {
            get { return m_areaStyle; }
            set { m_areaStyle = value; }
        }
    }
}
