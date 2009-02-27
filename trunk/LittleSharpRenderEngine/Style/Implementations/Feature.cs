using System;
using System.Collections.Generic;
using System.Text;

namespace LittleSharpRenderEngine.Style
{
    public class Feature : IStyle
    {
        protected IOutline m_outline;
        protected IFill m_fill;

        public IOutline Outline
        {
            get { return m_outline; }
            set { m_outline = value; }
        }

        public IFill Fill
        {
            get { return m_fill; }
            set { m_fill = value; }
        }
    }
}
