using System;
using System.Collections.Generic;
using System.Text;
using Topology.Geometries;
using System.Drawing;
using LittleSharpRenderEngine.Style;

namespace LittleSharpRenderEngine.Render
{
    public class Area
    {
        public Area()
        {
        }

        public void Render(LittleSharpRenderEngine engine, Graphics graphics, IPolygon polygon, IAreaStyle style)
        {
            if (polygon == null || style == null) return;

            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();

            gp.AddPolygon(RenderUtil.CoordToPoint(polygon.Shell.Coordinates));
            foreach (ILinearRing l in polygon.Holes)
                gp.AddPolygon(RenderUtil.CoordToPoint(l.Coordinates));
            gp.CloseFigure();

            if (style.Fill != null) RenderUtil.RenderFill(engine, graphics, gp, style.Fill);

            if (style.Outline != null) RenderUtil.RenderOutline(engine, graphics, gp, style.Outline);
        }
    }
}
