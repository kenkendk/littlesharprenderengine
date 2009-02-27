using System;
using System.Collections.Generic;
using System.Text;

namespace LittleSharpRenderEngine.Style
{
    public class Line : ILineStyle
    {
        private IEnumerable<IOutline> m_outlines;

        public Line()
        {
            m_outlines = new List<IOutline>();
        }

        public Line(IEnumerable<IOutline> outlines)
        {
            m_outlines = outlines;
        }

        public IEnumerable<IOutline> Outlines
        {
            get { return m_outlines; }
            set { m_outlines = value; }
        }

    }
}
