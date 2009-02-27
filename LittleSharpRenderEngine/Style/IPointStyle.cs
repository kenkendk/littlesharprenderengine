using System;
using System.Drawing;

namespace LittleSharpRenderEngine.Style
{
    public interface IPointStyle : IStyle
    {
        IFill Fill { get; }
        IOutline Outline { get; }
        System.Drawing.Point Center { get; set; }
        double Rotation { get; set; }
        Size Size { get; set; }
        Image Symbol { get; set; }
        Point.PointType Type { get; set; }
    }
}
