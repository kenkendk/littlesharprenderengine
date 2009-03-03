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

        public static List<string> EnumerateLayers(ServerConnectionI connection, string mapdef, bool onlyVisible)
        {
            List<string> l = new List<string>();
            MapDefinition mdef = connection.GetMapDefinition(mapdef);
            foreach (MapLayerType lt in mdef.Layers)
                if (!onlyVisible || lt.Visible)
                    l.Add(lt.ResourceId);

            if (mdef.BaseMapDefinition != null && mdef.BaseMapDefinition.BaseMapLayerGroup != null)
                foreach (BaseMapLayerGroupCommonType bgt in mdef.BaseMapDefinition.BaseMapLayerGroup)
                    if (!onlyVisible || bgt.Visible)
                        if (bgt.BaseMapLayer != null)
                            foreach(BaseMapLayerType blt in bgt.BaseMapLayer)
                                l.Add(blt.ResourceId);

            return l;
        }

        public static IEnvelope GetMapExtent(ServerConnectionI connection, string mapdef)
        {
            MapDefinition mdef = connection.GetMapDefinition(mapdef);
            return new Topology.Geometries.Envelope(mdef.Extents.MaxX, mdef.Extents.MinX, mdef.Extents.MaxY, mdef.Extents.MinY);
        }

        public static Topology.CoordinateSystems.ICoordinateSystem GetCoordinateSystem(ServerConnectionI connection, string mapdef)
        {
            MapDefinition mdef = connection.GetMapDefinition(mapdef);
            Topology.CoordinateSystems.CoordinateSystemFactory cf = new Topology.CoordinateSystems.CoordinateSystemFactory();
            return cf.CreateFromWkt(mdef.CoordinateSystem);
        }
    }
}
