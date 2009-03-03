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
        ServerConnectionI m_con;

        /// <summary>
        /// TODO: Handle scale range
        /// </summary>
        List<KeyValuePair<OperationOrParameter, IPointStyle>> m_pointRules = new List<KeyValuePair<OperationOrParameter, IPointStyle>>();
        List<KeyValuePair<OperationOrParameter, ILineStyle>> m_lineRules = new List<KeyValuePair<OperationOrParameter, ILineStyle>>();
        List<KeyValuePair<OperationOrParameter, IAreaStyle>> m_areaRules = new List<KeyValuePair<OperationOrParameter, IAreaStyle>>();

        Dictionary<string, string> m_columnnames = new Dictionary<string, string>();
        LayerDefinition m_layerDef;

        List<IFeature> m_featureCache = null;

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

            foreach(VectorScaleRangeType vsr in vldef.VectorScaleRange)
                foreach(object style in vsr.Items)
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

                                    m_pointRules.Add(new KeyValuePair<OperationOrParameter, IPointStyle>(op, p));
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

                                    m_lineRules.Add(new KeyValuePair<OperationOrParameter, ILineStyle>(op, new LittleSharpRenderEngine.Style.Line(lines)));                                    
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

                                    m_areaRules.Add(new KeyValuePair<OperationOrParameter, IAreaStyle>(op, a));
                                }
                            }
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

        public IEnumerable<IFeature> GetFeatures(IEnvelope bbox, string filter, float scale)
        {
            if (m_featureCache == null)
            {
                VectorLayerDefinitionType vldef = m_layerDef.Item as VectorLayerDefinitionType;

                string local_filter = ""; //TODO: Apply filter, from layer and passed in
                List<string> columns = new List<string>(m_columnnames.Keys);

                List<IFeature> cache = new List<IFeature>();

                using (FeatureSetReader fsr = m_layerDef.CurrentConnection.QueryFeatureSource(vldef.ResourceId, vldef.FeatureName, local_filter, columns.ToArray()))
                while(fsr.Read())
                {
                    IGeometry geom = fsr.Row[vldef.Geometry] as IGeometry;

                    if (geom == null)
                        continue;

                    //TODO: Evaluate rules and pick the correct one
                    if ((geom is IPoint || geom is IMultiPoint) && m_pointRules.Count > 0)
                    {
                        cache.Add(new FeatureImpl(geom, m_pointRules[0].Value));
                    }
                    else if ((geom is ILineString || geom is IMultiLineString) && m_lineRules.Count > 0)
                    {
                        cache.Add(new FeatureImpl(geom, m_lineRules[0].Value));
                    }
                    else if ((geom is IPolygon || geom is IMultiPolygon) && m_areaRules.Count > 0)
                    {
                        cache.Add(new FeatureImpl(geom, m_areaRules[0].Value));
                    }
                }

                m_featureCache = cache;
            }

            return m_featureCache;
        }

        #endregion
    }
}
