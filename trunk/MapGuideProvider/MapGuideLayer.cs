using System;
using System.Collections.Generic;
using System.Text;
using Topology.Geometries;
using OSGeo.MapGuide.MaestroAPI;
using LittleSharpRenderEngine;
using LittleSharpRenderEngine.Style;
using System.Data.LightDatamodel.QueryModel;

namespace MapGuideProvider
{
    public class MapGuideLayer : IProvider
    {
        private ServerConnectionI m_con;

        private class ScaleRange
        {
            public ScaleRange(double min, double max)
            {
                this.Min = min;
                this.Max = max;
            }

            public double Min = double.MinValue;
            public double Max = double.MaxValue;

            public List<KeyValuePair<OperationOrParameter, IPointStyle>> PointRules = new List<KeyValuePair<OperationOrParameter, IPointStyle>>();
            public List<KeyValuePair<OperationOrParameter, ILineStyle>> LineRules = new List<KeyValuePair<OperationOrParameter, ILineStyle>>();
            public List<KeyValuePair<OperationOrParameter, IAreaStyle>> AreaRules = new List<KeyValuePair<OperationOrParameter, IAreaStyle>>();
        }

        private List<ScaleRange> m_scaleRanges = new List<ScaleRange>();
        private Dictionary<string, string> m_columnnames = new Dictionary<string, string>();
        private LayerDefinition m_layerDef;

        private List<IFeature> m_featureCache = null;
        private Topology.CoordinateSystems.ICoordinateSystem m_coordSys;

        public MapGuideLayer(ServerConnectionI con, string layername)
        {
            m_con = con;

            m_layerDef = m_con.GetLayerDefinition(layername);
            if (!(m_layerDef.Item is VectorLayerDefinitionType))
                throw new Exception("The resource " + layername + " is not a vector layer");

            VectorLayerDefinitionType vldef = m_layerDef.Item as VectorLayerDefinitionType;

            m_columnnames = new Dictionary<string, string>();

            m_columnnames[vldef.Geometry] = null;

            ExtractColumnNames(vldef.Url, m_columnnames);
            ExtractColumnNames(vldef.ToolTip, m_columnnames);

            try
            {
                FeatureSource fs = m_con.GetFeatureSource(vldef.ResourceId);
                FdoSpatialContextList lst = fs.GetSpatialInfo();
                if (lst != null && lst.SpatialContext != null && lst.SpatialContext.Count > 0)
                {
                    Topology.CoordinateSystems.CoordinateSystemFactory cf = new Topology.CoordinateSystems.CoordinateSystemFactory();
                    m_coordSys = cf.CreateFromWkt(lst.SpatialContext[0].CoordinateSystemWkt);
                }
            }
            catch
            {
            }

            foreach (VectorScaleRangeType vsr in vldef.VectorScaleRange)
            {
                ScaleRange sr = new ScaleRange(vsr.MinScaleSpecified ? vsr.MinScale : 0, vsr.MaxScaleSpecified ? vsr.MaxScale : double.MaxValue);

                foreach (object style in vsr.Items)
                    if (style is PointTypeStyleType)
                    {
                        if (((PointTypeStyleType)style).PointRule != null)
                            foreach (PointRuleType rule in ((PointTypeStyleType)style).PointRule)
                            {
                                if (rule.Item != null && rule.Item.Item != null)
                                {
                                    OperationOrParameter op = ExtractColumnNames(rule.Filter, m_columnnames);
                                    if (rule.Label != null)
                                        ExtractColumnNames(rule.Label.Text, m_columnnames);

                                    LittleSharpRenderEngine.Style.Point p = new LittleSharpRenderEngine.Style.Point();
                                    p.Center = new System.Drawing.Point(
                                        (int)(rule.Item.Item.InsertionPointXSpecified ? rule.Item.Item.InsertionPointX : 0.5),
                                        (int)(rule.Item.Item.InsertionPointYSpecified ? rule.Item.Item.InsertionPointY : 0.5));

                                    p.Rotation = double.Parse(rule.Item.Item.Rotation, System.Globalization.CultureInfo.InvariantCulture);
                                    p.Size = new System.Drawing.Size(
                                        (int)double.Parse(rule.Item.Item.SizeX, System.Globalization.CultureInfo.InvariantCulture),
                                        (int)double.Parse(rule.Item.Item.SizeY, System.Globalization.CultureInfo.InvariantCulture));

                                    if (rule.Item.Item is MarkSymbolType)
                                    {
                                        MarkSymbolType mark = rule.Item.Item as MarkSymbolType;
                                        switch (mark.Shape)
                                        {
                                            case ShapeType.Circle:
                                                p.Type = LittleSharpRenderEngine.Style.Point.PointType.Circle;
                                                break;
                                            case ShapeType.Square:
                                                p.Type = LittleSharpRenderEngine.Style.Point.PointType.Square;
                                                break;
                                            case ShapeType.Triangle:
                                                p.Type = LittleSharpRenderEngine.Style.Point.PointType.Triangle;
                                                break;
                                            default:
                                                p.Type = LittleSharpRenderEngine.Style.Point.PointType.Circle;
                                                break;
                                        }

                                        if (mark.Fill != null)
                                        {
                                            p.Fill = new LittleSharpRenderEngine.Style.Base.Fill();
                                            p.Fill.BackgroundColor = mark.Fill.BackgroundColor;
                                            p.Fill.ForegroundColor = mark.Fill.ForegroundColor;
                                            //p.Fill.Pattern = mark.Fill.FillPattern;
                                            //TODO: Deal with unit/sizecontext
                                        }

                                        if (mark.Edge != null)
                                        {
                                            p.Outline = new LittleSharpRenderEngine.Style.Base.Outline();
                                            p.Outline.ForegroundColor = mark.Edge.Color;
                                            //p.Outline.DashStyle = mark.Edge.LineStyle;
                                            //p.Outline.Pattern = mark.Edge.LineStyle;
                                            p.Outline.Width = (int)double.Parse(mark.Edge.Thickness, System.Globalization.CultureInfo.InvariantCulture);
                                            //TODO: Deal with unit/sizecontext
                                        }
                                    }
                                    else
                                    {
                                        p.Type = LittleSharpRenderEngine.Style.Point.PointType.Circle;
                                        p.Outline = new LittleSharpRenderEngine.Style.Base.Outline();
                                        p.Fill = new LittleSharpRenderEngine.Style.Base.Fill();
                                        p.Fill.BackgroundColor = System.Drawing.Color.Red;
                                        p.Fill.ForegroundColor = System.Drawing.Color.Black;

                                        p.Outline.ForegroundColor = System.Drawing.Color.Black;
                                    }

                                    sr.PointRules.Add(new KeyValuePair<OperationOrParameter, IPointStyle>(op, p));
                                }
                            }
                    }
                    else if (style is LineTypeStyleType)
                    {
                        if (((LineTypeStyleType)style).LineRule != null)
                            foreach (LineRuleType rule in ((LineTypeStyleType)style).LineRule)
                            {
                                if (rule.Items != null && rule.Items.Count > 0)
                                {
                                    List<IOutline> lines = new List<IOutline>();
                                    OperationOrParameter op = ExtractColumnNames(rule.Filter, m_columnnames);
                                    if (rule.Label != null)
                                        ExtractColumnNames(rule.Label.Text, m_columnnames);

                                    foreach (StrokeType st in rule.Items)
                                    {
                                        LittleSharpRenderEngine.Style.Base.Outline outline = new LittleSharpRenderEngine.Style.Base.Outline();
                                        outline.ForegroundColor = st.Color;
                                        outline.Width = (int)double.Parse(st.Thickness, System.Globalization.CultureInfo.InvariantCulture);
                                        //outline.Pattern = st.LineStyle;
                                        //outline.DashStyle = st.LineStyle;
                                        //TODO: Deal with unit/sizecontext

                                        lines.Add(outline);
                                    }

                                    sr.LineRules.Add(new KeyValuePair<OperationOrParameter, ILineStyle>(op, new LittleSharpRenderEngine.Style.Line(lines)));
                                }

                            }
                    }
                    else if (style is AreaTypeStyleType)
                    {
                        if (((AreaTypeStyleType)style).AreaRule != null)
                            foreach (AreaRuleType rule in ((AreaTypeStyleType)style).AreaRule)
                            {
                                OperationOrParameter op = ExtractColumnNames(rule.Filter, m_columnnames);
                                if (rule.Label != null)
                                    ExtractColumnNames(rule.Label.Text, m_columnnames);

                                LittleSharpRenderEngine.Style.Area a = new Area();
                                if (rule.Item != null)
                                {
                                    if (rule.Item.Fill != null)
                                    {
                                        a.Fill = new LittleSharpRenderEngine.Style.Base.Fill();
                                        a.Fill.BackgroundColor = rule.Item.Fill.BackgroundColor;
                                        a.Fill.ForegroundColor = rule.Item.Fill.ForegroundColor;
                                        //p.Fill.Pattern = rule.Item.Fill.FillPattern;
                                        //TODO: Deal with unit/sizecontext
                                    }

                                    if (rule.Item.Stroke != null)
                                    {
                                        a.Outline = new LittleSharpRenderEngine.Style.Base.Outline();
                                        a.Outline.ForegroundColor = rule.Item.Stroke.Color;
                                        //p.Outline.DashStyle = rule.Item.Stroke.LineStyle;
                                        //p.Outline.Pattern = rule.Item.Stroke.LineStyle;
                                        a.Outline.Width = (int)double.Parse(rule.Item.Stroke.Thickness, System.Globalization.CultureInfo.InvariantCulture);
                                        //TODO: Deal with unit/sizecontext
                                    }

                                    sr.AreaRules.Add(new KeyValuePair<OperationOrParameter, IAreaStyle>(op, a));
                                }
                            }
                    }

                if (sr.PointRules.Count + sr.LineRules.Count + sr.AreaRules.Count > 0)
                    m_scaleRanges.Add(sr);
            }
        }

		public IEnvelope MaxBounds
		{
			get
			{
				throw new NotImplementedException();
			}
		}

        private OperationOrParameter ExtractColumnNames(string filter, Dictionary<string, string> columns)
        {
            //TODO: The parser does not support all the stuff that the MapGuide parser supports, eg. TIMESTAMP, INTERSECTS and GEOMFROMTEXT
            if (string.IsNullOrEmpty(filter))
                return System.Data.LightDatamodel.Query.NOP();

            Queue<OperationOrParameter> tokens = new Queue<OperationOrParameter>();
            OperationOrParameter rootop = Parser.ParseQuery(filter);
            tokens.Enqueue(rootop);

            while (tokens.Count > 0)
            {
                OperationOrParameter op = tokens.Dequeue();
                if (op is Operation)
                {
                    foreach (OperationOrParameter opm in (op as Operation).Parameters)
                        tokens.Enqueue(opm);
                }
                else
                {
                    System.Data.LightDatamodel.QueryModel.Parameter pm = op as System.Data.LightDatamodel.QueryModel.Parameter;
                    if (pm.IsFunction)
                    {
                        foreach (OperationOrParameter opm in pm.FunctionArguments)
                            tokens.Enqueue(opm);
                    }
                    else
                    {
                        //TODO: Does not correctly identify the column names if they are quoted
                        if (pm.IsColumn)
                            columns[(string)pm.Value] = null;
                        else if (pm.Value is String && ((string)pm.Value).StartsWith("'") && ((string)pm.Value).EndsWith("'"))
                            columns[((string)pm.Value).Substring(1, ((string)pm.Value).Length - 2)] = null;
                    }
                }

            }

            return rootop;
        }

        public MapGuideLayer(string url, string username, string password, string layername)
            : this(new HttpServerConnection(new Uri(url), username, password, null, true), layername)
        { }

        #region IProvider Members

        public IEnumerable<IFeature> GetFeatures(IEnvelope bbox, string filter, double scale)
        {
            ScaleRange sr = null;
            foreach (ScaleRange sx in m_scaleRanges)
                if (scale >= sx.Min && scale < sx.Max)
                {
                    sr = sx;
                    break;
                }

            //Hack to test renderings speed in the test application 
            if (sr == null && double.IsNaN(scale) && m_scaleRanges.Count > 0)
                sr = m_scaleRanges[0];

            if (sr == null)
                return new List<IFeature>();

            if (m_featureCache == null)
            {
                VectorLayerDefinitionType vldef = m_layerDef.Item as VectorLayerDefinitionType;

                string local_filter = ""; //TODO: Apply filter, from layer and passed in
                List<string> columns = new List<string>(m_columnnames.Keys);

                List<IFeature> cache = new List<IFeature>();

                try
                {
                    using (FeatureSetReader fsr = m_layerDef.CurrentConnection.QueryFeatureSource(vldef.ResourceId, vldef.FeatureName, local_filter, columns.ToArray()))
                        while (fsr.Read())
                        {
                            IGeometry geom = fsr.Row[vldef.Geometry] as IGeometry;

                            if (geom == null)
                                continue;

                            //TODO: Evaluate rules and pick the correct one
                            if ((geom is IPoint || geom is IMultiPoint) && sr.PointRules.Count > 0)
                            {
                                cache.Add(new FeatureImpl(geom, sr.PointRules[0].Value));
                            }
                            else if ((geom is ILineString || geom is IMultiLineString) && sr.LineRules.Count > 0)
                            {
                                cache.Add(new FeatureImpl(geom, sr.LineRules[0].Value));
                            }
                            else if ((geom is IPolygon || geom is IMultiPolygon) && sr.AreaRules.Count > 0)
                            {
                                cache.Add(new FeatureImpl(geom, sr.AreaRules[0].Value));
                            }
                        }

                }
                catch
                {
                    cache = new List<IFeature>();
                }

                m_featureCache = cache;
            }

            return m_featureCache;
        }

        public string ProviderName
        {
            get { return "WKT Provider"; }
        }

        public string DatasetName
        {
            get { return new ResourceIdentifier(m_layerDef.ResourceId).Name; }
        }

        public Topology.CoordinateSystems.ICoordinateSystem CoordinateSystem { get { return m_coordSys; } }

        #endregion
    }
}
