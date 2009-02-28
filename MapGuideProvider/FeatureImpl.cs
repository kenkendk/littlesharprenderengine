using System;
using System.Collections.Generic;
using System.Text;
using LittleSharpRenderEngine;
using LittleSharpRenderEngine.Style;
using Topology.Geometries;

namespace MapGuideProvider
{
    class FeatureImpl : IFeature 
    {
        private IGeometry m_geom;
        private IStyle m_style;

        public FeatureImpl(IGeometry geom, IStyle style)
        {
            m_geom = geom;
            m_style = style;
        }

        #region IFeature Members

        public IGeometry Geometry
        {
            get { return m_geom; }
        }

        public IStyle Style
        {
            get { return m_style; }
        }

        #endregion
    }
}
