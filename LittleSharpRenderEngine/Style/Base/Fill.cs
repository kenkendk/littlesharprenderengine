using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace LittleSharpRenderEngine.Style.Base
{
    public class Fill : ColoredItem, IFill
    {
        protected Image m_pattern;
        protected System.Drawing.Drawing2D.HatchStyle m_hatch;
        protected Brush m_brush;

        public Image Pattern
        {
            get { return m_pattern; }
            set { m_pattern = value; }
        }

        public System.Drawing.Drawing2D.HatchStyle HatchStyle
        {
            get { return m_hatch; }
            set { m_hatch = value; }
        }

    }
}
