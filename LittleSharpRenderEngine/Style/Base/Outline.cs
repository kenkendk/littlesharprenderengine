using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace LittleSharpRenderEngine.Style.Base
{
    public class Outline : ColoredItem, IOutline
    {
        protected Image m_pattern;
        protected System.Drawing.Drawing2D.DashStyle m_dash;
        protected int m_width;
        protected Pen m_pen;

        public Image Pattern
        {
            get { return m_pattern; }
            set { m_pattern = value; }
        }

        public System.Drawing.Drawing2D.DashStyle DashStyle
        {
            get { return m_dash; }
            set { m_dash = value; }
        }

        public int Width
        {
            get { return m_width; }
            set { m_width = value; }
        }

    }
}
