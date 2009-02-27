using System;
namespace LittleSharpRenderEngine.Style
{
    public interface IColoredItem
    {
        System.Drawing.Color BackgroundColor { get; set; }
        System.Drawing.Color ForegroundColor { get; set; }
    }
}
