using System;
using System.Collections.Generic;
using System.Text;
using Topology.Geometries;
using OSGeo.MapGuide.MaestroAPI;
using LittleSharpRenderEngine;

namespace MapGuideProvider
{
    public class MapGuideLayer : IProvider
    {
        ServerConnectionI m_con;

        public MapGuideLayer(ServerConnectionI con, string layername)
        {
            m_con = con;

            LayerDefinition ldef = m_con.GetLayerDefinition(layername);
            if (!(ldef.Item is VectorLayerDefinitionType))
                throw new Exception("The resource " + layername + " is not a vector layer");
        }

        public MapGuideLayer(string url, string username, string password, string layername)
            : this(new HttpServerConnection(new Uri(url), username, password, null, true), layername)
        { }
    }
}
