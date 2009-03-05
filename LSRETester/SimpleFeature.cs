using System;
using System.Collections.Generic;
using System.Text;
using LittleSharpRenderEngine.Style;
using Topology.Geometries;

namespace LSRETester
{
    struct SimpleFeature : LittleSharpRenderEngine.IFeature
    {
        private IStyle m_style;
        private IGeometry m_geom;

        public SimpleFeature(IGeometry geom, IStyle style)
        {
            m_style = style;
            m_geom = geom;
        }

        #region IFeature Members

        public Topology.Geometries.IGeometry Geometry
        {
            get { return m_geom; }
        }

        public LittleSharpRenderEngine.Style.IStyle Style
        {
            get { return m_style; }
        }

        #endregion
    }

	struct FeatureLinkedListVisitor : Topology.Index.IItemVisitor
	{
		private LinkedList<LittleSharpRenderEngine.IFeature> m_list;

		public FeatureLinkedListVisitor(LinkedList<LittleSharpRenderEngine.IFeature> list)
		{
			m_list = list;
		}

		#region IItemVisitor Members

		public void VisitItem(object item)
		{
			m_list.AddLast((LittleSharpRenderEngine.IFeature)item);
		}

		#endregion
	}
}
