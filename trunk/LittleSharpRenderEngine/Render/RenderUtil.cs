using System;
using System.Collections.Generic;
using System.Text;
using Topology.Geometries;
using System.Drawing;
using LittleSharpRenderEngine.Style;

namespace LittleSharpRenderEngine.Render
{
    class RenderUtil
    {
        public static System.Drawing.Point[] CoordToPoint(ICoordinate[] source)
        {
            System.Drawing.Point[] px = new System.Drawing.Point[source.Length];
            for (int i = 0; i < px.Length; i++)
                px[i] = new System.Drawing.Point((int)source[i].X, (int)source[i].Y);

            return px;
        }

        public static void RenderFill(LittleSharpRenderEngine engine, Graphics graphics, System.Drawing.Drawing2D.GraphicsPath path, IFill fill)
        {
            Brush brush;
            if (fill.Pattern != null)
            {
                //TODO: Make this
                return;
            }
            else
            {
                //TODO: Hatch style
                brush = new System.Drawing.SolidBrush(fill.ForegroundColor);
            }

            using(brush)
                if (fill.Pattern == null) graphics.FillPath(brush, path);

        }

        public static void RenderOutline(LittleSharpRenderEngine engine, Graphics graphics, System.Drawing.Drawing2D.GraphicsPath path, IOutline line)
        {
            Pen pen;
            if (line.Pattern != null)
            {
                //TODO: Fix this
                return;
            }
            else
            {
                //TODO: Dash style
                pen = new Pen(line.ForegroundColor, line.Width);
            }

            if (line.Pattern == null)
                graphics.DrawPath(pen, path);


        }


    }
}
