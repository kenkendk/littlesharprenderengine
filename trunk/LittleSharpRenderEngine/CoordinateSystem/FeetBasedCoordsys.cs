using System;
using System.Collections.Generic;
using System.Text;

namespace LittleSharpRenderEngine.CoordinateSystem
{
    public class FeetBasedCoordsys : MeterBasedCoordsys
    {
        public FeetBasedCoordsys()
            : base(0.3048, 0.3048)
        {
        }
    }
}
