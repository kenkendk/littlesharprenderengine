using System;
using System.Collections.Generic;
using System.Text;
using Topology.Geometries;
using System.Drawing;

namespace LittleSharpRenderEngine.CoordinateSystem
{
    public class MeterBasedCoordsys : ICoordinateSystemHelper 
    {
        //Dots pr inch
        protected const double DPI = 96;

        //Inches pr meter
        protected const double IPM = 39.3700787;

        //Coordsys distance pr unit in meters, X/Y axis
        protected readonly double UDM_X = 1;
        protected readonly double UDM_Y = 1;

        #region ICoordinateSystem Members

        public MeterBasedCoordsys()
        { }

        public MeterBasedCoordsys(double meters_pr_x_unit, double meters_pr_y_unit)
        {
            UDM_X = meters_pr_x_unit;
            UDM_Y = meters_pr_y_unit;
        }

        public virtual double CalculateScale(IEnvelope bbox, Size size)
        {
            double picture_width_in_meters = (size.Width / DPI) / IPM;
            double picture_height_in_meters = (size.Height / DPI) / IPM;

            double map_width_in_meters = bbox.Width * UDM_X;
            double map_height_in_meters = bbox.Height * UDM_Y;

            double width_scale = map_width_in_meters / picture_width_in_meters;
            double height_scale = map_height_in_meters / picture_height_in_meters;

            return Math.Max(width_scale, height_scale);
        }

        public virtual Topology.Geometries.IEnvelope AdjustBoundingBox(IEnvelope bbox, double scale, Size size)
        {
            double picture_width_in_meters = ((size.Width / DPI) / IPM) * scale; 
            double picture_height_in_meters = ((size.Height / DPI) / IPM) * scale;

            double width_extent = picture_width_in_meters / UDM_X;
            double height_extent = picture_height_in_meters / UDM_Y;

            return new Envelope(bbox.Centre.X - (width_extent / 2), bbox.Centre.X + (width_extent / 2), bbox.Centre.Y - (height_extent / 2), bbox.Centre.Y + (height_extent / 2));
        }

        public virtual double DistanceInMeters(Topology.Geometries.IPoint p1, Topology.Geometries.IPoint p2)
        {
            double xdist = Math.Abs(p1.X - p2.X);
            double ydist = Math.Abs(p1.Y - p2.Y);

            return Math.Sqrt((xdist * xdist) + (ydist * ydist));            
        }

        public virtual Topology.CoordinateSystems.ICoordinateSystem CoordinateSystem { get { return null; } }

        #endregion
    }
}
