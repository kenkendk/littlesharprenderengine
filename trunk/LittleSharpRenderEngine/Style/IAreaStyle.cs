using System;
using System.Collections.Generic;
using System.Text;

namespace LittleSharpRenderEngine.Style
{
    public interface IAreaStyle : IStyle
    {
        IFill Fill { get; }
        IOutline Outline { get; }
    }
}
