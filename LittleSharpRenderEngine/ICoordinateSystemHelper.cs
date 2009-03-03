using System;
using System.Collections.Generic;
using System.Text;
using Topology.Geometries;
using System.Drawing;

namespace LittleSharpRenderEngine
{
    public interface ICoordinateSystemHelper
    {
        /// <summary>
        /// Calculates the scale of the map, given the bounding box and image size
        /// </summary>
        /// <param name="bbox">The map bounding box</param>
        /// <param name="size">The size of the image</param>
        /// <returns>The scale</returns>
        double CalculateScale(IEnvelope bbox, Size size);

        /// <summary>
        /// Adjusts the boundingbox to equal proportions 
        /// </summary>
        /// <param name="bbox">The actual bounding box</param>
        /// <param name="scale">The scale to fit</param>
        /// <param name="size">The size to fit to</param>
        /// <returns>A bounding box with the correct ratio</returns>
        IEnvelope AdjustBoundingBox(IEnvelope bbox, double scale, Size size);

        /// <summary>
        /// Calculates the distance from one point to another, in meters
        /// </summary>
        /// <param name="p1">One point</param>
        /// <param name="p2">Another point</param>
        /// <returns>The distance in meters</returns>
        double DistanceInMeters(IPoint p1, IPoint p2);

        /// <summary>
        /// The actual underlying coordinate system
        /// </summary>
        Topology.CoordinateSystems.ICoordinateSystem CoordinateSystem { get; }


    }
}
