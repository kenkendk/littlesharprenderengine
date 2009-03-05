using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Collections;

namespace MapInfo
{
	public enum MapInfoObjectTypes
	{
		Unknown,
		Region,
		Line,
		Point,
		Ellipse,
		Roundrect,
		Arc,
		Text,
	}

	public class MapInfoObject : LittleSharpRenderEngine.IFeature
	{
		public MapInfoObjectTypes ObjectType = MapInfoObjectTypes.Unknown;
		public string[] Data;
		public double[][] SubObjects;
		public Color BorderColor = Color.Black;
		public Color FillColor = Color.Empty;
		public MapInfoLayer ParentLayer;
		private LittleSharpRenderEngine.Style.IStyle m_IStyle;
		private Topology.Geometries.IGeometry m_IGeometri;
		public MapInfoObject(MapInfoLayer layer)
		{
			ParentLayer = layer;
		}
		protected internal void LoadFromMIDLine(string dataline)
		{
			List<string> tmp = new List<string>();

			//parse elements
			int s = 0;
			bool insideword = false;
			for (int i = 0; i < dataline.Length; i++)
			{
				if (dataline[i] == ',' && !insideword)
				{
					tmp.Add(dataline.Substring(s, i - s).Trim('"').Replace("\"\"", "\""));
					s = i + 1;
				}
				else if (dataline[i] == '"')
				{
					insideword = !insideword;
				}
			}
			if(!dataline.EndsWith(","))
				tmp.Add(dataline.Substring(s, dataline.Length - s).Trim('\r').Trim('"').Replace("\"\"", "\""));

			Data = tmp.ToArray();
		}
		public string this[string columnname]
		{
			get
			{
				try
				{
					return Data[Array.IndexOf<string>(ParentLayer.ColumnNames, columnname)];
				}
				catch
				{
					throw new Exception("Kunne ikke hente kolonne (" + columnname + ")");
				}
			}
		}

		#region IFeature Members

		public Topology.Geometries.IGeometry Geometry
		{
			get 
			{
				if (m_IGeometri != null) return m_IGeometri;
				switch (this.ObjectType)
				{
					case MapInfoObjectTypes.Unknown:
						return null;
					case MapInfoObjectTypes.Point:
						m_IGeometri = new Topology.Geometries.Point(new Topology.Geometries.Coordinate(this.SubObjects[0][0], this.SubObjects[0][1]));
						break;
					case MapInfoObjectTypes.Line:
						Topology.Geometries.Coordinate[] arr = new Topology.Geometries.Coordinate[this.SubObjects[0].Length / 2];
						for (int cc = 0; cc < this.SubObjects[0].Length; cc += 2)
						{
							arr[cc / 2] = new Topology.Geometries.Coordinate(this.SubObjects[0][cc], this.SubObjects[0][cc + 1]);
						}
						m_IGeometri = new Topology.Geometries.LineString(arr);
						break;
					case MapInfoObjectTypes.Region:
						Topology.Geometries.Polygon[] polygons = new Topology.Geometries.Polygon[this.SubObjects.Length];
						int p = 0;
						foreach(double[] obj in this.SubObjects)
						{
							Topology.Geometries.Coordinate[] coords = new Topology.Geometries.Coordinate[obj.Length / 2];
							for (int cc = 0; cc < obj.Length; cc += 2)
							{
								coords[cc/2] = new Topology.Geometries.Coordinate(obj[cc], obj[cc + 1]);
							}
							polygons[p++] = new Topology.Geometries.Polygon(new Topology.Geometries.LinearRing(coords));
						}
						if (polygons.Length > 1) m_IGeometri = new Topology.Geometries.MultiPolygon(polygons);
						else m_IGeometri = polygons[0];
						break;
					default:
						throw new Exception("MapInfo type " + this.ObjectType.ToString() + " not yet supported");
				}
				return m_IGeometri;
			}
		}

		public LittleSharpRenderEngine.Style.IStyle Style
		{
			get 
			{
				if (m_IStyle != null) return m_IStyle;
				switch (this.ObjectType)
				{
					case MapInfoObjectTypes.Unknown:
						return null;
					case MapInfoObjectTypes.Point:
						LittleSharpRenderEngine.Style.Point ps = new LittleSharpRenderEngine.Style.Point();
						ps.Type = LittleSharpRenderEngine.Style.Point.PointType.Circle;
						m_IStyle = ps;
						break;
					case MapInfoObjectTypes.Line:
						m_IStyle = new LittleSharpRenderEngine.Style.Line(this.BorderColor, 1);
						break;
					case MapInfoObjectTypes.Region:
						LittleSharpRenderEngine.Style.Area a = new LittleSharpRenderEngine.Style.Area();
						a.Outline.ForegroundColor = this.BorderColor;
						a.Outline.Width = 1;
						a.Fill.BackgroundColor = Color.White;
						a.Fill.ForegroundColor = this.FillColor;
						m_IStyle = a;
						break;
					default:
						throw new Exception("MIF type " + this.ObjectType.ToString() + " not yet supported");
				}
				return m_IStyle;
			}
		}

		#endregion
	}

	public class MapInfoLayer : IEnumerable
	{
		public string Name;
		public string Index = "";
		public string CoordinateSystem = "CoordSys Earth Projection 8, 115, \"m\", 9, 0, 0.9996, 500000, 0 Bounds (-400000, 5200000) (1600000, 7200000)";		//assume UTM32
		public string[] ColumnNames = new string[0];
		public string[] ColumnTypes = new string[0];
		public MapInfoObject[] Objects;
		public MapInfoLayer(string name)
		{
			Name = name;
		}
		public void AddColumn(string name, string type)
		{
			Array.Resize<string>(ref ColumnNames, ColumnNames.Length + 1);
			Array.Resize<string>(ref ColumnTypes, ColumnTypes.Length + 1);
			ColumnNames[ColumnNames.Length - 1] = name;
			ColumnTypes[ColumnTypes.Length - 1] = type;

			//also add an extra field at all objects .... bugger
			foreach (MapInfoObject obj in this)
				Array.Resize<string>(ref obj.Data, obj.Data.Length + 1);
		}
		public IEnumerator GetEnumerator()
		{
			return Objects.GetEnumerator();
		}
		public int FindColumn(string name)
		{
			for (int i = 0; i < ColumnNames.Length; i++)
				if (ColumnNames[i].Equals(name, StringComparison.OrdinalIgnoreCase)) return i;
			return -1;
		}

		public Type GetColumnType(int i)
		{
			string mitype = ColumnTypes[i].ToLower();
			if (mitype == "integer")
				return typeof(int);
			else if (mitype.StartsWith("char("))
				return typeof(string);
			else if (mitype == "float")
				return typeof(double);
			else if (mitype == "date")
				return typeof(DateTime);
			else if (mitype == "logical")
				return typeof(bool);
			else if (mitype == "smallint")
				return typeof(Int16);
			else if (mitype.StartsWith("decimal("))
				return typeof(decimal);
			else
				throw new Exception("MITypen " + mitype + " er endnu ikke understøttet i GetColumnType");
		}

		public int GetColumnWidth(int i)
		{
			string mitype = ColumnTypes[i].ToLower();
			if (mitype.StartsWith("char("))
			{
				int e = mitype.IndexOf(')');
				return int.Parse(mitype.Substring(5, e - 5-1).Trim());
			}
			else if (mitype.StartsWith("decimal("))
			{
				int e = mitype.IndexOf(',');
				return int.Parse(mitype.Substring(5, e - 5-1).Trim());
			}
			else return -1;
		}
	}

	public class MIFParser
	{
		/// <summary>
		/// Will save the given layer to the given path
		/// </summary>
		/// <param name="layer"></param>
		/// <param name="path"></param>
		public static void SaveFile(MapInfoLayer layer, string path)
		{
			//create MID
			string midpath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path), System.IO.Path.GetFileNameWithoutExtension(path) + ".mid");
			System.IO.StreamWriter sw = new System.IO.StreamWriter(midpath, false, System.Text.Encoding.Default);
			foreach (MapInfoObject obj in layer)
			{
				string line = "";
				for (int i = 0; i < layer.ColumnNames.Length; i++)
				{
					//write
					if (layer.ColumnTypes[i].StartsWith("Char("))
						line += "\"" + obj.Data[i] + "\",";
					else
						line += obj.Data[i] + ",";
				}
				line = line.Substring(0, line.Length - 1);
				sw.WriteLine(line);
			}
			sw.Close();
			sw.Dispose();

			//create MIF
			string mifpath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path), System.IO.Path.GetFileNameWithoutExtension(path) + ".mif");
			try
			{
				sw = new System.IO.StreamWriter(mifpath, false, System.Text.Encoding.Default);
			}
			catch (Exception ex)
			{
				throw new Exception("Kunne ikke få adgang til filen \"" + mifpath + "\"\nError: " + ex.Message);
			}
			try
			{
				sw.WriteLine("Version 600");
				sw.WriteLine("Charset \"WindowsLatin1\"");
				sw.WriteLine("Delimiter \", \"");
				if(!String.IsNullOrEmpty(layer.Index))sw.WriteLine("Index " + layer.Index);
				sw.WriteLine(layer.CoordinateSystem);
				sw.WriteLine("Columns " + (layer.ColumnNames.Length).ToString());
				for (int i = 0; i < layer.ColumnNames.Length; i++)
				{
					sw.WriteLine("  " + layer.ColumnNames[i] + " " + layer.ColumnTypes[i]);
				}
				sw.WriteLine("Data");
				sw.WriteLine("");
			}
			catch (Exception ex)
			{
				throw new Exception("Kunne ikke skrive header til filen \"" + mifpath + "\"\nError: " + ex.Message);
			}

			try
			{
				foreach (MapInfo.MapInfoObject obj in layer)
				{
					switch (obj.ObjectType)
					{
						case MapInfoObjectTypes.Line:
							if (obj.SubObjects.Length != 1) throw new Exception("Linie-samlinger er ikke understøttet");
							sw.WriteLine("Pline " + (obj.SubObjects[0].Length / 2).ToString());
							for (int j = 0; j < obj.SubObjects[0].Length; j += 2)
								sw.WriteLine(obj.SubObjects[0][j].ToString(System.Globalization.CultureInfo.InvariantCulture) + " " + obj.SubObjects[0][j + 1].ToString(System.Globalization.CultureInfo.InvariantCulture));
							sw.WriteLine("    Pen (1,2," + ((obj.BorderColor.R * 65536) + (obj.BorderColor.G * 256) + obj.BorderColor.B) + ") ");
							break;
						case MapInfoObjectTypes.Point:
							if (obj.SubObjects.Length != 1) throw new Exception("Punkt-samlinger er ikke understøttet");
							sw.WriteLine("Point " + obj.SubObjects[0][0].ToString(System.Globalization.CultureInfo.InvariantCulture) + " " + obj.SubObjects[0][1].ToString(System.Globalization.CultureInfo.InvariantCulture));
							sw.WriteLine("    Symbol (35," + ((obj.BorderColor.R * 65536) + (obj.BorderColor.G * 256) + obj.BorderColor.B) + ",12) ");
							break;
						case MapInfoObjectTypes.Region:
							sw.WriteLine("Region  " + obj.SubObjects.Length);
							foreach (double[] objcoord in obj.SubObjects)
							{
								if (objcoord.Length % 2 != 0) throw new Exception("Antallet af koordinater er ikke deleligt med 2 (" + objcoord.Length + ")");
								sw.WriteLine("  " + (objcoord.Length / 2).ToString());
								for (int j = 0; j < objcoord.Length; j += 2)
									sw.WriteLine(objcoord[j].ToString(System.Globalization.CultureInfo.InvariantCulture) + " " + objcoord[j + 1].ToString(System.Globalization.CultureInfo.InvariantCulture));
							}
							sw.WriteLine("    Pen (1,2," + ((obj.BorderColor.R * 65536) + (obj.BorderColor.G * 256) + obj.BorderColor.B) + ") ");
							if (!obj.FillColor.IsEmpty)
								sw.WriteLine("    Brush (1," + ((obj.FillColor.R * 65536) + (obj.FillColor.G * 256) + obj.FillColor.B) + ",0)");
							else
								sw.WriteLine("    Brush (1,0,0)");
							break;
						default:
							sw.WriteLine("None");
							break;
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Kunne ikke skrive objekter til filen \"" + mifpath + "\"\nError: " + ex.Message);
			}
			finally
			{
				if (sw != null)
				{
					sw.Close();
					sw.Dispose();
					sw = null;
				}
			}
		}

		/// <summary>
		/// This will parse the given mif and its counterpart
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static MapInfoLayer LoadFile(string path)
		{
			MapInfoLayer layer = new MapInfoLayer(Path.GetFileNameWithoutExtension(path));

			//read MID
			string midfilepath = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path) + ".mid";
			if (!File.Exists(midfilepath)) throw new Exception("Kunne ikke finde MID-fil: \"" + midfilepath + "\"");
			StreamReader sr = new StreamReader(midfilepath, Encoding.Default);
			string[] midlines = sr.ReadToEnd().Trim().Split('\n');
			sr.Close();
			layer.Objects = new MapInfoObject[midlines.Length];
			for (int i = 0; i < midlines.Length; i++)
			{
				layer.Objects[i] = new MapInfoObject(layer);
				layer.Objects[i].LoadFromMIDLine(midlines[i]);
			}

			//read MIF
			if (!File.Exists(path)) throw new Exception("Kunne ikke finde MIF-fil");
			sr = new StreamReader(path, Encoding.Default);
			string[] miflines = sr.ReadToEnd().Trim().Split('\n');
			sr.Close();

			//coordsys
			int l = 0;
			while (!miflines[l].StartsWith("CoordSys")) l++;
			layer.CoordinateSystem = miflines[l];

			//Index 
			if (miflines.Length >= 4 && miflines[3].StartsWith("Index ")) layer.Index = miflines[3].Substring(6).Trim();

			//columns
			l = 0;
			while (!miflines[l].StartsWith("Columns")) l++;
			layer.ColumnNames = new string[int.Parse(miflines[l].Substring(8))];
			layer.ColumnTypes = new string[int.Parse(miflines[l].Substring(8))];
			l++;
			int c = 0;
			string[] e;
			while (!miflines[l].StartsWith("Data"))
			{
				e = miflines[l].Trim().Split(' ');
				layer.ColumnNames[c] = e[0];
				layer.ColumnTypes[c] = e[1];
				if (layer.ColumnTypes[c].StartsWith("Decimal(")) layer.ColumnTypes[c] += " " + e[2];
				c++;
				l++;
			}
			l++;

			//objects
			int curobj = 0;
			while (l < miflines.Length)
			{
				//find object start
				while (l < miflines.Length && (miflines[l] == "" || miflines[l][0] == '\r' || miflines[l][0] == ' ')) l++;
				if (l == miflines.Length) break;

				if (l < miflines.Length && miflines[l].StartsWith("Region "))
				{
					layer.Objects[curobj].ObjectType = MapInfoObjectTypes.Region;
					layer.Objects[curobj].SubObjects = new double[int.Parse(miflines[l++].Substring(7))][];
					for (int re = 0; re < layer.Objects[curobj].SubObjects.Length; re++)
					{
						layer.Objects[curobj].SubObjects[re] = new double[int.Parse(miflines[l++].Trim()) * 2];
						for (int i = 0; i < layer.Objects[curobj].SubObjects[re].Length; i += 2)
						{
							double[] coord = ParseCoordinatePair(miflines[l++]);
							layer.Objects[curobj].SubObjects[re][i] = coord[0];
							layer.Objects[curobj].SubObjects[re][i + 1] = coord[1];
						}
					}
				}
				else if (l < miflines.Length && miflines[l].StartsWith("Point "))
				{
					layer.Objects[curobj].ObjectType = MapInfoObjectTypes.Point;
					layer.Objects[curobj].SubObjects = new double[1][];
					layer.Objects[curobj].SubObjects[0] = ParseCoordinatePair(miflines[l++].Substring(6));
				}
				else if (l < miflines.Length && miflines[l].StartsWith("Pline Multiple"))
				{
					layer.Objects[curobj].ObjectType = MapInfoObjectTypes.Line;
					layer.Objects[curobj].SubObjects = new double[int.Parse(miflines[l++].Substring(15))][];
					for (int i = 0; i < layer.Objects[curobj].SubObjects.Length; i++ )
					{
						layer.Objects[curobj].SubObjects[i] = new double[int.Parse(miflines[l++].Trim()) * 2];
						for (int j = 0; j < layer.Objects[curobj].SubObjects[i].Length; j += 2)
						{
							double[] coord = ParseCoordinatePair(miflines[l++]);
							layer.Objects[curobj].SubObjects[i][j] = coord[0];
							layer.Objects[curobj].SubObjects[i][j + 1] = coord[1];
						}
					}
				}
				else if (l < miflines.Length && miflines[l].StartsWith("Pline "))
				{
					layer.Objects[curobj].ObjectType = MapInfoObjectTypes.Line;
					layer.Objects[curobj].SubObjects = new double[1][];
					layer.Objects[curobj].SubObjects[0] = new double[int.Parse(miflines[l++].Substring(6)) * 2];
					for (int i = 0; i < layer.Objects[curobj].SubObjects[0].Length; i += 2)
					{
						double[] coord = ParseCoordinatePair(miflines[l++]);
						layer.Objects[curobj].SubObjects[0][i] = coord[0];
						layer.Objects[curobj].SubObjects[0][i + 1] = coord[1];
					}
				}
				else if (l < miflines.Length && miflines[l].StartsWith("Text"))
				{
					layer.Objects[curobj].ObjectType = MapInfoObjectTypes.Text;
					string text = miflines[++l];	//what to do with the text? Do we wanna keep it? Nah, text sucks
					l++;
					layer.Objects[curobj].SubObjects = new double[1][];
					string[] tmp = miflines[l++].Trim().Split(' ');
					layer.Objects[curobj].SubObjects[0] = new double[] { double.Parse(tmp[0], System.Globalization.CultureInfo.InvariantCulture), double.Parse(tmp[1], System.Globalization.CultureInfo.InvariantCulture), double.Parse(tmp[2], System.Globalization.CultureInfo.InvariantCulture), double.Parse(tmp[3], System.Globalization.CultureInfo.InvariantCulture) };
					l++;	//this be the font line
				}
				else if (l < miflines.Length && miflines[l].StartsWith("Line "))
				{
					layer.Objects[curobj].ObjectType = MapInfoObjectTypes.Line;
					layer.Objects[curobj].SubObjects = new double[1][];
					string[] tmp = miflines[l++].Split(' ');
					layer.Objects[curobj].SubObjects[0] = new double[] { double.Parse(tmp[1], System.Globalization.CultureInfo.InvariantCulture), double.Parse(tmp[2], System.Globalization.CultureInfo.InvariantCulture), double.Parse(tmp[3], System.Globalization.CultureInfo.InvariantCulture), double.Parse(tmp[4], System.Globalization.CultureInfo.InvariantCulture) };
				}
				else if (l < miflines.Length && miflines[l].StartsWith("Ellipse "))
				{
					layer.Objects[curobj].ObjectType = MapInfoObjectTypes.Ellipse;
					layer.Objects[curobj].SubObjects = new double[1][];
					string[] tmp = miflines[l++].Split(' ');
					layer.Objects[curobj].SubObjects[0] = new double[] { double.Parse(tmp[1], System.Globalization.CultureInfo.InvariantCulture), double.Parse(tmp[2], System.Globalization.CultureInfo.InvariantCulture), double.Parse(tmp[3], System.Globalization.CultureInfo.InvariantCulture), double.Parse(tmp[4], System.Globalization.CultureInfo.InvariantCulture) };
				}
				else if (l < miflines.Length && miflines[l].StartsWith("Roundrect "))
				{
					layer.Objects[curobj].ObjectType = MapInfoObjectTypes.Roundrect;
					layer.Objects[curobj].SubObjects = new double[1][];
					string[] tmp = miflines[l++].Split(' ');
					layer.Objects[curobj].SubObjects[0] = new double[] { double.Parse(tmp[1], System.Globalization.CultureInfo.InvariantCulture), double.Parse(tmp[2], System.Globalization.CultureInfo.InvariantCulture), double.Parse(tmp[3], System.Globalization.CultureInfo.InvariantCulture), double.Parse(tmp[4], System.Globalization.CultureInfo.InvariantCulture), double.Parse(tmp[5], System.Globalization.CultureInfo.InvariantCulture) };
				}
				else if (l < miflines.Length && miflines[l].StartsWith("Arc "))
				{
					layer.Objects[curobj].ObjectType = MapInfoObjectTypes.Arc;
					layer.Objects[curobj].SubObjects = new double[1][];
					string[] tmp = miflines[l++].Split(' ');
					layer.Objects[curobj].SubObjects[0] = new double[] { double.Parse(tmp[1], System.Globalization.CultureInfo.InvariantCulture), double.Parse(tmp[2], System.Globalization.CultureInfo.InvariantCulture), double.Parse(tmp[3], System.Globalization.CultureInfo.InvariantCulture), double.Parse(tmp[4], System.Globalization.CultureInfo.InvariantCulture), double.Parse(tmp[5], System.Globalization.CultureInfo.InvariantCulture), double.Parse(tmp[6], System.Globalization.CultureInfo.InvariantCulture) };
				}
				else if (l < miflines.Length && miflines[l].StartsWith("Rect "))
				{
					layer.Objects[curobj].ObjectType = MapInfoObjectTypes.Region;
					layer.Objects[curobj].SubObjects = new double[1][];
					string[] tmp = miflines[l++].Split(' ');
					double[] tmppoints = new double[8];
					tmppoints[0] = double.Parse(tmp[1], System.Globalization.CultureInfo.InvariantCulture);
					tmppoints[1] = double.Parse(tmp[2], System.Globalization.CultureInfo.InvariantCulture);
					tmppoints[4] = double.Parse(tmp[3], System.Globalization.CultureInfo.InvariantCulture);
					tmppoints[5] = double.Parse(tmp[4], System.Globalization.CultureInfo.InvariantCulture);
					tmppoints[2] = tmppoints[4];
					tmppoints[3] = tmppoints[1];
					tmppoints[6] = tmppoints[0];
					tmppoints[7] = tmppoints[5];
					layer.Objects[curobj].SubObjects[0] = tmppoints;
				}
				else if (l < miflines.Length && miflines[l].StartsWith("none"))
				{
					layer.Objects[curobj].ObjectType = MapInfoObjectTypes.Unknown;
					l++;
				}
				else throw new Exception("MIF-objekt-type (" + miflines[l] + ") er endnu ikke understøttet");

				//parse style
				if (l < miflines.Length && miflines[l].StartsWith("    Pen "))
				{
					layer.Objects[curobj].BorderColor = Color.FromArgb(255, Color.FromArgb(ParseParenteseBlock(miflines[l].Substring(8))[2]));
					l++;
				}
				if (l < miflines.Length && miflines[l].StartsWith("    Brush "))
				{
					layer.Objects[curobj].FillColor = Color.FromArgb(255, Color.FromArgb(ParseParenteseBlock(miflines[l].Substring(10))[2]));
					l++;
				}

				curobj++;
			}

			return layer;
		}

		/// <summary>
		/// This will convert the .NET value to the MIF value. Eg. (int)17 -> "17"
		/// </summary>
		/// <param name="value"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public static string ConvertToMIFValue(object value, Type type)
		{
			if (value == DBNull.Value || value == null) value = "";

			if (type == typeof(int) || type == typeof(long))
				return value.ToString() == "" ? "0" : value.ToString();
			else if (type == typeof(System.Int16))
				return value.ToString() == "" ? "0" : value.ToString();
			else if (type == typeof(DateTime))
				return value.ToString() == "" ? "0" : ((DateTime)value).ToString("yyyyMMdd");
			else if (type == typeof(bool))
				return value.ToString() == "" || (bool)value == false ? "F" : "T";
			else if (type == typeof(string))
				return value.ToString();
			else if (type == typeof(float) || type == typeof(double))
				return value.ToString() == "" ? "0" : ((double)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
			else throw new Exception("Typen " + type.Name + " er endnu ikke understøttet.");
		}

		/// <summary>
		/// This will convert the given .NET type to MIF. Eg. int -> Integer
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static string ConvertToMIFType(Type type)
		{
			return ConvertToMIFType(type, -1);
		}

		/// <summary>
		/// This will convert the given .NET type to MIF. Eg. int -> Integer
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static string ConvertToMIFType(Type type, int length)
		{
			if (type == typeof(int) || type == typeof(long))
				return "Integer";
			else if (type == typeof(System.Int16))
				return "SmallInt";
			else if (type == typeof(DateTime))
				return "Date";
			else if (type == typeof(bool))
				return "Logical";
			else if (type == typeof(string))
				return length > 0 && length <= 255 ? "Char(" + length + ")" : "Char(255)";
			else if (type == typeof(float) || type == typeof(double))
				return "Float";
			else if (type == typeof(byte[]))
				throw new Exception("Typen " + type.Name + " er endnu ikke understøttet. Hvis dette i virkeligheden er geometri-kolonnen, så skal du installere den seneste Service Pack til .NET");
			else throw new Exception("Typen " + type.Name + " er endnu ikke understøttet.");
		}

		/// <summary>
		/// This parses a coordinate pair. Eg. "45.4 7895.2"
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		private static double[] ParseCoordinatePair(string str)
		{
			str = str.Trim();
			string[] e = str.Split(' ');
			double[] ret = new double[2];
			ret[0] = double.Parse(e[0], System.Globalization.CultureInfo.InvariantCulture) ;
			ret[1] = double.Parse(e[1], System.Globalization.CultureInfo.InvariantCulture);
			return ret;
		}

		/// <summary>
		/// This will parse a "(1,2,0)"
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		private static int[] ParseParenteseBlock(string str)
		{
			str = str.Trim().Trim('(').Trim(')');
			string[] e = str.Split(',');
			int[] ret = new int[e.Length];
			for (int i = 0; i < e.Length; i++)
				ret[i] = int.Parse(e[i]);
			return ret;
		}
	}
}
