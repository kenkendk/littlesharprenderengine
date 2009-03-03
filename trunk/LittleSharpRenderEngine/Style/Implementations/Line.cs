using System;
using System.Collections.Generic;
using System.Text;
using LittleSharpRenderEngine.Style.Base;

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

		public Line(IOutline outline) : this(new IOutline[]{outline})
		{
		}

		public Line(System.Drawing.Color color, int width)
		{
			Outline ol = new Outline();
			ol.ForegroundColor = color;
			ol.Width = width;
			m_outlines = new IOutline[] {ol };
		}

        public IEnumerable<IOutline> Outlines
        {
            get { return m_outlines; }
            set { m_outlines = value; }
        }

    }
}
