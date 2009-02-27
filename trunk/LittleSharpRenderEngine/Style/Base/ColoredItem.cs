using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace LittleSharpRenderEngine.Style.Base
{
    public class ColoredItem : IColoredItem
    {
        protected Color m_backgroundColor;
        protected Color m_foregroundColor;

        public Color BackgroundColor
        {
            get { return m_backgroundColor; }
            set { m_backgroundColor = value; }
        }

        public Color ForegroundColor
        {
            get { return m_foregroundColor; }
            set { m_foregroundColor = value; }
        }
    }
}
