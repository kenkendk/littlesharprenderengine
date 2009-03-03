using System;
using System.Collections.Generic;
using System.Text;

namespace LittleSharpRenderEngine.CoordinateSystem
{
    public class ActualCoordinateSystem : MeterBasedCoordsys, ICoordinateSystemHelper 
    {
        private Topology.CoordinateSystems.Transformations.ICoordinateTransformation m_transform;
        private const string XY_M = "LOCAL_CS[\"Non-Earth (Meter)\",LOCAL_DATUM[\"Local Datum\",0],UNIT[\"Meter\", 1],AXIS[\"X\",EAST],AXIS[\"Y\",NORTH]]";

        public ActualCoordinateSystem(Topology.CoordinateSystems.ICoordinateSystem coordinateSystem)
        {
            if (coordinateSystem == null)
                throw new ArgumentNullException("coordinateSystem");

            Topology.CoordinateSystems.Transformations.CoordinateTransformationFactory f = new Topology.CoordinateSystems.Transformations.CoordinateTransformationFactory();
            Topology.CoordinateSystems.CoordinateSystemFactory cf = new Topology.CoordinateSystems.CoordinateSystemFactory();

            /*Topology.CoordinateSystems.ICoordinateSystem local = cf.CreateLocalCoordinateSystem(
                "Non-Earth (Meter)",
                cf.CreateLocalDatum("Local Datum", Topology.CoordinateSystems.DatumType.VD_Normal),
                new Topology.CoordinateSystems.LinearUnit(1.0, "Meter", "", 0, "", "", ""),
                new List<Topology.CoordinateSystems.AxisInfo>(new Topology.CoordinateSystems.AxisInfo[] {
                    new Topology.CoordinateSystems.AxisInfo("X", Topology.CoordinateSystems.AxisOrientationEnum.East),
                    new Topology.CoordinateSystems.AxisInfo("Y", Topology.CoordinateSystems.AxisOrientationEnum.North)
                })
            );

            string s = cf.ToString();*/

            m_transform = f.CreateFromCoordinateSystems(coordinateSystem, cf.CreateFromWkt(XY_M));
        }

        #region ICoordinateSystem Members

        public override double CalculateScale(Topology.Geometries.IEnvelope bbox, System.Drawing.Size size)
        {
            double[] points = m_transform.MathTransform.Transform(new double[] { bbox.MinX, bbox.MinY, bbox.MaxX, bbox.MaxY });
            Topology.Geometries.IEnvelope localEnv = new Topology.Geometries.Envelope(points[0], points[2], points[1], points[3]);
            return base.CalculateScale(localEnv, size);
        }

        public override Topology.Geometries.IEnvelope AdjustBoundingBox(Topology.Geometries.IEnvelope bbox, double scale, System.Drawing.Size size)
        {
            double[] points = m_transform.MathTransform.Transform(new double[] { bbox.MinX, bbox.MinY, bbox.MaxX, bbox.MaxY });
            Topology.Geometries.IEnvelope localEnv = new Topology.Geometries.Envelope(points[0], points[2], points[1], points[3]);
            localEnv = base.AdjustBoundingBox(localEnv, scale, size);
            points = m_transform.MathTransform.Inverse().Transform(new double[] { localEnv.MinX, localEnv.MinY, localEnv.MaxX, localEnv.MaxY });
            return new Topology.Geometries.Envelope(points[0], points[2], points[1], points[3]);
        }

        public override double DistanceInMeters(Topology.Geometries.IPoint p1, Topology.Geometries.IPoint p2)
        {
            double[] points = m_transform.MathTransform.Transform(new double[] { p1.X, p1.Y, p2.X, p2.Y });
            return base.DistanceInMeters(new Topology.Geometries.Point(points[0], points[1]), new Topology.Geometries.Point(points[2], points[3]));
        }

        public override Topology.CoordinateSystems.ICoordinateSystem CoordinateSystem { get { return m_transform.SourceCS; } }

        #endregion
    }
}
