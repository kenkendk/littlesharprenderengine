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
        /// <summary>
        /// Returns all features in a given area, matching a filter, and visible at a given scale
        /// </summary>
		/// <param name="bbox">The bounding geometry that limits the number of features.</param>
        /// <param name="filter">An optional filter applied to the features</param>
        /// <param name="scale">The scale to display the features at</param>
        /// <returns>all features in the given area, matching the filter, and visible at the given scale</returns>
        IEnumerable<IFeature> GetFeatures(IEnvelope bbox, string filter, double scale);

		/// <summary>
		/// Returns all features in a given area, matching a filter, and visible at a given scale
		/// </summary>
		/// <param name="geom">The bounding geometry that limits the number of features. Notice that this can be a point</param>
		/// <param name="filter">An optional filter applied to the features</param>
		/// <param name="scale">The scale to display the features at</param>
		/// <returns>all features in the given area, matching the filter, and visible at the given scale</returns>
		IEnumerable<IFeature> GetFeatures(IGeometry geom, string filter, double scale);

        /// <summary>
        /// The name of the provider, for internal use
        /// </summary>
        string ProviderName { get; }
        
        /// <summary>
        /// The name of the dataset, for display to the user
        /// </summary>
        string DatasetName { get; }

        /// <summary>
        /// The projection the data originates from
        /// </summary>
        Topology.CoordinateSystems.ICoordinateSystem CoordinateSystem { get; }

		IEnvelope MaxBounds { get; }
    }
}
