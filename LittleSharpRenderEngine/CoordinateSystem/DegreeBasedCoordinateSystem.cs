using System;
using System.Collections.Generic;
using System.Text;

namespace LittleSharpRenderEngine.CoordinateSystem
{
    public class DegreeBasedCoordinateSystem : MeterBasedCoordsys
    {
        public DegreeBasedCoordinateSystem()
            : base(10000000 / 90, 10000000 / 90)
        { 
        }
    }
}
