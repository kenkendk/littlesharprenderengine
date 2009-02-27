using System;
using System.Collections.Generic;
using System.Text;
using Topology.Geometries;
using System.Drawing;
using LittleSharpRenderEngine.Style;

namespace LittleSharpRenderEngine.Render
{
    public class Line
    {
        public Line()
        {
        }

        public void Render(LittleSharpRenderEngine engine, Graphics graphics, ILineString line, ILineStyle style)
        {
            if (line == null || style == null)
                return;

            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp.AddLines(RenderUtil.CoordToPoint(line.Coordinates));
            gp.CloseFigure();

            if (style.Outlines != null)
                foreach(IOutline linestyle in style.Outlines)
                    RenderUtil.RenderOutline(engine, graphics, gp, linestyle);
        }
    }
}
