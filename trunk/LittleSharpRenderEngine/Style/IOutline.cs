using System;
namespace LittleSharpRenderEngine.Style
{
    public interface IOutline : IColoredItem
    {
        System.Drawing.Drawing2D.DashStyle DashStyle { get; set; }
        System.Drawing.Image Pattern { get; set; }
        int Width { get; set; }
    }
}
