using System;
using System.Collections.Generic;
using System.Text;
using Topology.Geometries;

namespace LittleSharpRenderEngine
{
    /// <summary>
    /// A simple interface for retrieving features from a provider
    /// </summary>
    public interface IProvider
    {
        IEnumerable<IFeature> GetFeatures(IEnvelope bbox, string filter, float scale);
		IEnvelope MaxBounds { get; }
    }
}
