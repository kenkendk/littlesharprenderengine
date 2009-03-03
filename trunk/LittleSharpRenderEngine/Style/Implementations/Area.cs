using System;
using System.Collections.Generic;
using System.Text;
using LittleSharpRenderEngine.Style.Base;

namespace LittleSharpRenderEngine.Style
{
    public class Area : Feature, IAreaStyle
    {
		public Area()
		{
			this.Fill = new Fill();
			this.Outline = new Outline();
		}
    }
}
