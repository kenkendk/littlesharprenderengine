using System;
using System.Collections.Generic;
using System.Text;
using Topology.Geometries;
using LittleSharpRenderEngine.Style;

namespace LittleSharpRenderEngine
{
    /// <summary>
    /// A feature that is able to be rendered
    /// </summary>
    public interface IFeature
    {
        /// <summary>
        /// The geometry of the feature
        /// </summary>
        IGeometry Geometry { get; }

        /// <summary>
        /// The style to render the feature with
        /// </summary>
        IStyle Style { get; }
    }
}
