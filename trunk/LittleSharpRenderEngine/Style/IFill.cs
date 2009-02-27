using System;
namespace LittleSharpRenderEngine.Style
{
    public interface IFill : IColoredItem
    {
        System.Drawing.Drawing2D.HatchStyle HatchStyle { get; set; }
        System.Drawing.Image Pattern { get; set; }
    }
}
