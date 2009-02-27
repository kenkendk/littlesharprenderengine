using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace LittleSharpRenderEngine.Style
{
    public class Point : Feature, IPointStyle
    {
        public enum PointType
        {
            Symbol,
            Circle,
            Triangle,
            Square
        }

        protected PointType m_pointType;
        protected Image m_symbol;
        protected Size m_size;
        protected double m_rotation;
        protected System.Drawing.Point m_center;

        public PointType Type
        {
            get { return m_pointType; }
            set { m_pointType = value; }
        }

        public Image Symbol
        {
            get { return m_symbol; }
            set { m_symbol = value; }
        }

        public Size Size
        {
            get { return m_size; }
            set { m_size = value; }
        }

        public double Rotation
        {
            get { return m_rotation; }
            set { m_rotation = value; }
        }

        public System.Drawing.Point Center
        {
            get { return m_center; }
            set { m_center = value; }
        }
    }
}
