using System;
using System.Collections.Generic;
using System.Text;
using Topology.Geometries;
using System.Drawing;
using LittleSharpRenderEngine.Style;

namespace LittleSharpRenderEngine.Render
{
    public class Point
    {

        public Point()
        {
        }

        public void Render(LittleSharpRenderEngine engine, Graphics graphics, IPoint point, IPointStyle style)
        {
            if (point == null || style == null)
                return;

            if (style.Type == global::LittleSharpRenderEngine.Style.Point.PointType.Symbol)
            {
                return;
            }
            else
            {
                int w = style.Size.Width / 2;
                int h = style.Size.Height / 2;
                System.Drawing.Point[] points;

                switch (style.Type)
                {
                    case global::LittleSharpRenderEngine.Style.Point.PointType.Circle:
                        int pc = (int)Math.Max(8, Math.Log10(Math.Max(w, h)));
                        points = new System.Drawing.Point[pc + 1];

                        double fr = (2*Math.PI) / pc;

                        for (int i = 0; i < pc; i++)
                            points[i] = new System.Drawing.Point((int)(Math.Cos(fr * i) * w + point.X), (int)(Math.Sin(fr * i) * h + point.Y));
                        points[pc] = points[0];
                        break;
                    case global::LittleSharpRenderEngine.Style.Point.PointType.Square:
                        points = new System.Drawing.Point[] 
                                {
                                    new System.Drawing.Point((int)point.X - w, (int)point.Y - h),
                                    new System.Drawing.Point((int)point.X + w, (int)point.Y - h),
                                    new System.Drawing.Point((int)point.X + w, (int)point.Y + h),
                                    new System.Drawing.Point((int)point.X - w, (int)point.Y + h),
                                    new System.Drawing.Point((int)point.X - w, (int)point.Y - h),
                                };
                        break;
                    case global::LittleSharpRenderEngine.Style.Point.PointType.Triangle:
                        points = new System.Drawing.Point[] 
                                {
                                    new System.Drawing.Point((int)point.X - w, (int)point.Y + h),
                                    new System.Drawing.Point((int)point.X, (int)point.Y - h),
                                    new System.Drawing.Point((int)point.X + w, (int)point.Y + h),
                                    new System.Drawing.Point((int)point.X - w, (int)point.Y + h),
                                };
                        break;
                    default:
                        return;
                }

                //TODO: Apply rotation
                if (style.Type != global::LittleSharpRenderEngine.Style.Point.PointType.Circle)
                {
                }

                //Apply offset
                for (int i = 0; i < points.Length; i++)
                {
                    points[i].X += (w - style.Center.X);
                    points[i].Y += (h - style.Center.Y);
                }

                System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
                gp.AddPolygon(points);
                gp.CloseFigure();

                if (style.Fill != null)
                    RenderUtil.RenderFill(engine, graphics, gp, style.Fill);

                if (style.Outline != null)
                    RenderUtil.RenderOutline(engine, graphics, gp, style.Outline);
            }
        }
    }
}
