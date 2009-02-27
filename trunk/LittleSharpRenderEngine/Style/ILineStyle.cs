using System;
using System.Collections.Generic;
using System.Text;

namespace LittleSharpRenderEngine.Style
{
    public interface ILineStyle : IStyle
    {
        IEnumerable<IOutline> Outlines { get; }
    }
}
