using System;
using System.Collections.Generic;
using System.Text;
using OSGeo.MapGuide.MaestroAPI;
using Topology.Geometries;

namespace MapGuideProvider
{
    public static class MapGuideUtil
    {

        public static ServerConnectionI CreateConnection(string url, string username, string password)
        {
            return new HttpServerConnection(new Uri(url), username, password, null, true);
        }

        public static List<string> EnumerateLayers(ServerConnectionI connection, string mapdef)
        {
            List<string> l = new List<string>();
            MapDefinition mdef = connection.GetMapDefinition(mapdef);
            foreach (MapLayerType lt in mdef.Layers)
                l.Add(lt.ResourceId);

            if (mdef.BaseMapDefinition != null && mdef.BaseMapDefinition.BaseMapLayerGroup != null)
                foreach (BaseMapLayerType blt in mdef.BaseMapDefinition.BaseMapLayerGroup)
                    l.Add(blt.ResourceId);

            return l;
        }

        public static IEnvelope GetMapExtent(ServerConnectionI connection, string mapdef)
        {
            MapDefinition mdef = connection.GetMapDefinition(mapdef);
            return new Topology.Geometries.Envelope(mdef.Extents.MaxX, mdef.Extents.MinX, mdef.Extents.MaxY, mdef.Extents.MinY);
        }
    }
}
