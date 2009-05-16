using System;
using System.Collections.Generic;
using System.Text;
using Topology;
using Topology.CoordinateSystems;
using Topology.Geometries;
using System.Drawing;
using System.Drawing.Imaging;
using LittleSharpRenderEngine.Style;

namespace LittleSharpRenderEngine
{
    public delegate IGeometry GeometryConverter(LittleSharpRenderEngine engine, IGeometry geometry);
    public delegate void PolygonRender(LittleSharpRenderEngine engine, Graphics graphics, IPolygon polygon, IAreaStyle style);
    public delegate void LineStringRender(LittleSharpRenderEngine engine, Graphics graphics, ILineString line, ILineStyle style);
    public delegate void PointRender(LittleSharpRenderEngine engine, Graphics graphics, IPoint point, IPointStyle style);

    public class LittleSharpRenderEngine : IDisposable
    {
        public GeometryConverter ProjectionConverter;
        public GeometryConverter Generalizer;
        public GeometryConverter CoordinateMapper;

        public PolygonRender PolygonRender;
        public LineStringRender LineStringRender;
        public PointRender PointRender;

        private IEnvelope m_boundingBox;
        private Bitmap m_canvas;
        private ICoordinateSystem m_targetCoordsys;
        private ICoordinateSystem m_sourceCoordsys;
        private Graphics m_graphics;

        //private double m_scale;

        public LittleSharpRenderEngine(IEnvelope bbox, ICoordinateSystem coordinateSystem, Size size, Color background)
        {
            m_boundingBox = bbox;
            m_targetCoordsys = coordinateSystem;
            m_canvas = new Bitmap(size.Width, size.Height);
            m_graphics = Graphics.FromImage(m_canvas);
            m_graphics.Clear(background);
            m_graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            
            //m_graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            
            //m_graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //m_graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            //m_graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            PointRender = new PointRender(new Render.Point().Render);
            LineStringRender = new LineStringRender(new Render.Line().Render);
            PolygonRender = new PolygonRender(new Render.Area().Render);
        }

        public IEnvelope BoundingBox { get { return m_boundingBox; } }
        public ICoordinateSystem CoordinateSystem { get { return m_targetCoordsys; } }
        public Size Size { get { return m_canvas.Size; } }

        public ICoordinateSystem SourceCoordinateSystem { get { return m_sourceCoordsys; } }

        public void RenderFeatures(ICoordinateSystem coordsys, IEnumerable<IFeature> items)
        {
            if (items == null) return;

            m_sourceCoordsys = coordsys;

            foreach (IFeature g in items)
            {
                IGeometry geom = g.Geometry;
                if (ProjectionConverter != null)
                {
                    geom = ProjectionConverter(this, geom);
                    if (geom == null) continue;
                }

                if (Generalizer != null)
                {
                    geom = Generalizer(this, geom);
                    if (geom == null) continue;
                }

                if (CoordinateMapper != null)
                    geom = CoordinateMapper(this, geom);
                else
                    geom = PrimitiveCoordinateMapper(geom);

                if (geom == null) continue;

                Draw(geom, g.Style);
            }

            m_sourceCoordsys = null;
        }

        /*private IGeometry SimpleProjectionConverter(LittleSharpRenderEngine engine, IGeometry geometry)
        {
            if (engine.m_sourceCoordsys == null || engine.m_targetCoordsys == null)
                return geometry;

            Topology.CoordinateSystems.Transformations.CoordinateTransformationFactory tf = new Topology.CoordinateSystems.Transformations.CoordinateTransformationFactory();
            tf.CreateFromCoordinateSystems(engine.m_sourceCoordsys, engine.m_targetCoordsys);
        }*/

        private ICoordinate[] PrimitiveCoordinateMapper(ICoordinate[] input)
        {
            double xres = m_canvas.Width / m_boundingBox.Width;
            double yres = m_canvas.Height / m_boundingBox.Height;
            double xoff = m_boundingBox.MinX;
            double yoff = m_boundingBox.MinY;

            ICoordinate[] output = new ICoordinate[input.Length];
            for(int i = 0; i < input.Length; i++)
                output[i] = new Coordinate((input[i].X - xoff) * xres, m_canvas.Height - ((input[i].Y - yoff) * yres));

            return output;
        }

        private IGeometry PrimitiveCoordinateMapper(IGeometry geom)
        {
            if (geom is IPoint)
                return new Topology.Geometries.Point(PrimitiveCoordinateMapper(geom.Coordinates)[0]);
            else if (geom is IMultiPoint)
            {
                IGeometry[] input = (geom as IMultiLineString).Geometries;
                IPoint[] output = new IPoint[input.Length];
                for (int i = 0; i < input.Length; i++)
                    output[i] = (IPoint)PrimitiveCoordinateMapper(input[i]);
                return new MultiPoint(output);
            }
            else if (geom is ILineString)
                return new LineString(PrimitiveCoordinateMapper(geom.Coordinates));
            else if (geom is ILinearRing)
                return new LinearRing(PrimitiveCoordinateMapper(geom.Coordinates));
            else if (geom is IMultiLineString)
            {
                IGeometry[] input = (geom as IMultiLineString).Geometries;
                ILineString[] output = new ILineString[input.Length];
                for (int i = 0; i < input.Length; i++)
                    output[i] = (ILineString)PrimitiveCoordinateMapper(input[i]);

                return new MultiLineString(output);
            }
            else if (geom is IPolygon)
            {
                ILineString shell = (ILineString)PrimitiveCoordinateMapper(((IPolygon)geom).Shell);

                ILineString[] input = (geom as IPolygon).Holes;
                ILinearRing[] output = new ILinearRing[input.Length];
                for (int i = 0; i < input.Length; i++)
                    output[i] = new LinearRing(((ILineString)PrimitiveCoordinateMapper(input[i])).Coordinates);

                return new Polygon(new LinearRing(shell.Coordinates), output);
            }
            else if (geom is IMultiPolygon)
            {
                IGeometry[] input = (geom as IMultiPolygon).Geometries;
                IPolygon[] output = new IPolygon[input.Length];
                for (int i = 0; i < input.Length; i++)
                    output[i] = (IPolygon)PrimitiveCoordinateMapper(input[i]);

                return new MultiPolygon(output);
            }
            else if (geom is IGeometryCollection)
            {
                IGeometry[] input = (geom as IGeometryCollection).Geometries;
                IGeometry[] output = new IGeometry[input.Length];
                for (int i = 0; i < input.Length; i++)
                    output[i] = PrimitiveCoordinateMapper(input[i]);

                return new GeometryCollection(output);
            }
            else
                return null;
        }

        private System.Drawing.Point[] CoordToPoint(ICoordinate[] source)
        {
            System.Drawing.Point[] px = new System.Drawing.Point[source.Length];
            for (int i = 0; i < px.Length; i++)
                px[i] = new System.Drawing.Point((int)source[i].X, (int)source[i].Y);

            return px;
        }

        private void Draw(IGeometry geom, IStyle style)
        {
            if (geom == null || style == null) return;

            if (geom is IPoint)
            {
                if (PointRender != null) PointRender(this, m_graphics, geom as IPoint, style as IPointStyle);
            }
            else if (geom is ILineString)
            {
                if (LineStringRender != null) LineStringRender(this, m_graphics, geom as ILineString, style as ILineStyle);
            }
            else if (geom is IPolygon)
            {
                if (PolygonRender != null) PolygonRender(this, m_graphics, geom as IPolygon, style as IAreaStyle);
            }
            else if (geom is IGeometryCollection)
            {
                foreach (IGeometry gx in (geom as IGeometryCollection).Geometries)
                    Draw(gx, style);
            }
        }

        public System.Drawing.Bitmap Bitmap { get { return m_canvas; } }

        #region IDisposable Members

        public void Dispose()
        {
            if (m_graphics != null)
            {
                m_graphics.Dispose();
                m_graphics = null;
            }

            if (m_canvas != null)
            {
                m_canvas.Dispose();
                m_canvas = null;
            }
        }

        #endregion
    }
}
