// $Id: MiWrapper.cs,v 1.2 2005/03/24 17:02:06 dmorissette Exp $
//

using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections;
using System.Text;

namespace EBop.MapObjects.MapInfo 
{

	/*
	 * These classes wrap the mitab c api functions to produce a hierarchy of classes giving readonly 
	 * access to the feature points.
	 * 
	 * Requires mitab.dll (www.maptools.org)
	 * See http://mitab.maptools.org/
	 * 
	 * Graham Sims,
	 * Environment Bay of Plenty, Whakatane, New Zealand
	 * http://www.envbop.govt.nz		
	 */

	/// <summary>
	/// This is a helper class for our standard enumerator based on EnumImpl. Implementataions 
	/// (Features, Parts, Fields and Vertices) are array like structures that can provide
	/// an object at a given index between 0 and Count  - 1.
	/// </summary>
	interface IObjectProvider 
    {
		int Count {	get;}
		object GetObj(int idx);
	}

	/// <summary>
	/// Implementation of an enumeration scheme over an index (array like) structure. 
	/// This class provides an enumerator that will work over any IObjectProvider implementation
	/// (Features, Parts, Fields and Vertices).
	/// </summary>
	/// <remarks>
	/// Calls to the GetEnumerator method of Fields, Parts and Vertices will return
	/// an instance of this class. Calls to GetEnumerator of Features will return a descendant
	/// of this class (due to the fact that features don't necessarily have a sequential
	/// index).
	/// </remarks>
	public class IndexedEnum : IEnumerator 
    {
		public readonly int Count;
		protected int eIdx = -1;
		private readonly IObjectProvider objProvider;

		internal IndexedEnum(IObjectProvider objProvider) 
        {
			this.objProvider = objProvider;
		}

		public virtual void Reset() 
        {
			eIdx = -1;
		}

		public object Current 
        {
			get {
				return objProvider.GetObj(eIdx);
			}
		}

		public virtual bool MoveNext() 
        {
			return (++eIdx < objProvider.Count);
		}
	}

	/// <summary>
	/// Partial implementation of IEnumerable over an indexed (aray like) structure.
	/// </summary>
	/// <remarks>
	/// Fields, Vertices, Parts and Features all descend from this class. It serves to
	/// provide the common functionality required to generate their enumerators.
	/// </remarks>
	public abstract class EnumImpl : IEnumerable, IObjectProvider 
    {
		private readonly int count;

		protected EnumImpl(int count) 
        {
			this.count = count;
		}

		public int Count {get  {return count;}}

		public virtual IEnumerator GetEnumerator() 
        {
			return new IndexedEnum(this);
		}

		public abstract object GetObj(int idx);
	}


	/// <summary>
	/// Represents a readonly view of field in a layer.
	/// </summary>
	/// <remarks>
	/// A field instance does not relate explicity to a single feature instance. Rather
	/// it represents all the features in the layer. To find the value of a field for a particular
	/// feature one of the GetValueAs methods should be called, passing the feature in.
	/// </remarks>
	public class Field {
		/// <summary>
		/// The field name
		/// </summary>
		public readonly string Name;
		/// <summary>
		/// The field type
		/// </summary>
		public FieldType Type;
		/// <summary>
		/// The index of the field within the layers set of fields.
		/// </summary>
		public readonly int Index;
		/// <summary>
		/// The field width
		/// </summary>
		public readonly int Width;
		/// <summary>
		/// The field precision
		/// </summary>
		public readonly short Precision;
		/// <summary>
		/// The layer this field belongs to.
		/// </summary>
		public readonly Layer Layer;

		public readonly int Ordinal;

		protected internal Field(Layer layer, int i) 
        {
			this.Layer = layer;
			this.Index = i;
			this.Name = MiApi.mitab_c_get_field_name(layer.Handle, i);
			this.Type = MiApi.mitab_c_get_field_type(layer.Handle, i);
			this.Precision = (short) MiApi.mitab_c_get_field_precision(layer.Handle, i);
			this.Width = MiApi.mitab_c_get_field_width(layer.Handle, i);
			this.Ordinal = i;
		}

		public System.Type DotNetType
		{
			get
			{
				switch (Type)
				{
					case FieldType.TABFT_Char:
						return typeof(string);
					case FieldType.TABFT_Date:
						return typeof(DateTime);
					case FieldType.TABFT_Decimal:
						return typeof(double);
					case FieldType.TABFT_Float:
						return typeof(float);
					case FieldType.TABFT_Integer:
						return typeof(int);
					case FieldType.TABFT_Logical:
						return typeof(bool);
					case FieldType.TABFT_SmallInt:
						return typeof(short);
				}
				return typeof(object);
			}
		}

		/// <summary>
		/// Returns a string representation of this fields value for the given feature.
		/// </summary>
		/// <param name="feature">The feature to find the fields value for.</param>
		/// <returns>A string representation of this fields value for the given feature</returns>
		public string GetValueAsString(Feature feature) 
        {
			return MiApi.mitab_c_get_field_as_string(feature.Handle, this.Index);
		}

		/// <summary>
		/// Returns a double representation of this fields value for the given feature.
		/// </summary>
		/// <param name="feature">The feature to find the fields value for.</param>
		/// <returns>A double representation of this fields value for the given feature</returns>
		public double GetValueAsDouble(Feature feature) 
        {
			return MiApi.mitab_c_get_field_as_double(feature.Handle, this.Index);
		}

		public override string ToString() 
        {
			return this.Name+", "+this.Type;
		}

	}

	/// <summary>
	/// Contains the set of fields belonging to a layer.
	/// </summary>
	/// <remarks>
	/// This class descends EnumImpl, meaning the fields in the
	/// set can be iterated using foreach.
	/// It also has an index property allowing any field between 0 and Fields.Count-1
	/// to be accessed directly with Fields[idx]
	/// </remarks>
	public class Fields : EnumImpl 
    {
		private Field[] fields;

		protected internal Fields(Layer layer):base(MiApi.mitab_c_get_field_count(layer.Handle)) 
        {
			fields = new Field[Count];
			for (int i=0; i<Count; i++)
				fields[i] = CreateField(layer, i);
		}

		/// <summary>
		/// Override this to support descendants of the Field class.
		/// </summary>
		/// <returns>A Field, with the given index, belonging to the given Layer</returns>
		protected internal virtual Field CreateField(Layer layer, int index) 
        {
			return new Field(layer, index);
		}

		public virtual Field this[int index] 
        {
			get 
            {
				return index < Count ? fields[index] : null;
			}
		}

		public override object GetObj(int idx) 
        {
			return this[idx];
		}

		public override string ToString() 
        {
			StringBuilder str = new StringBuilder();
			str.Append("Columns:\n");
			foreach (Field field in this)
				str.Append(field.ToString()+"\t");
			return str.ToString();
		}
	}

	/// <summary>
	/// Contains the set of values belonging to a feature.
	/// </summary>
	/// <remarks>
	/// This class descends EnumImpl, meaning the fields in the
	/// set can be iterated using foreach.
	/// It also has an index property allowing any field between 0 and DataValues.Count-1
	/// to be accessed directly with DataValues[idx]
	/// </remarks>
	public class DataValues : EnumImpl
	{
		public object[] Values;

		protected internal DataValues(Feature feature):base(MiApi.mitab_c_get_field_count(feature.Layer.Handle)) 
        {
			Values = new object[Count];
			for (int i = 0; i < Count; i++)
			{
				switch (feature.Layer.Fields[i].Type)
				{
					case FieldType.TABFT_Char:
						Values[i] = MiApi.mitab_c_get_field_as_string(feature.Handle, i);
						break;
					case FieldType.TABFT_Date:
						double d = MiApi.mitab_c_get_field_as_double(feature.Handle, i);
						if (d == 0)
							Values[i] = null;
						else
						{
							int year = (int)(d / 10000);
							int month = (int)((d - year * 10000) / 100);
							int day = (int)(d - year * 10000 - month * 100);
							Values[i] = new DateTime(year, month, day);
						}	
						break;
					case FieldType.TABFT_Decimal:
						Values[i] = MiApi.mitab_c_get_field_as_double(feature.Handle, i);
						break;
					case FieldType.TABFT_Float:
						Values[i] = (float)MiApi.mitab_c_get_field_as_double(feature.Handle, i);
						break;
					case FieldType.TABFT_Integer:
						Values[i] = (int)MiApi.mitab_c_get_field_as_double(feature.Handle, i);
						break;
					case FieldType.TABFT_Logical:
						Values[i] = MiApi.mitab_c_get_field_as_string(feature.Handle, i) == "T"? true : false;
						break;
					case FieldType.TABFT_SmallInt:
						Values[i] = (short)MiApi.mitab_c_get_field_as_double(feature.Handle, i);
						break;
				}
			}
		}

		public virtual object this[int index] 
        {
			get 
            {
				return index < Count ? Values[index] : null;
			}
		}

		public override object GetObj(int idx) 
        {
			return this[idx];
		}

		public override string ToString() 
        {
			StringBuilder str = new StringBuilder();
			str.Append("Values:\n");
			foreach (object value in this)
				if(value != null) str.Append(value.ToString() + "\t");
			return str.ToString();
		}
	}

	/// <summary>
	/// Contains the set of Vertices belonging to a single Part.
	/// </summary>
	/// <remarks>
	/// This class descends EnumImpl, meaning the vertices in the
	/// set can be iterated using foreach.
	/// It also has an index property allowing any vertice between 0 and Vertices.Count-1
	/// to be accessed directly with Vertices[idx]
	/// </remarks>
	public class Vertices : EnumImpl 
    {
		public readonly Part Part;

		protected internal Vertices(Part part):	base(MiApi.mitab_c_get_vertex_count(part.Feature.Handle, part.Index)) 
        {
			this.Part = part;
			
		}

		public virtual Topology.Geometries.Coordinate this[int index] 
        {
			get 
            {
				return index < Count ? new Topology.Geometries.Coordinate(MiApi.mitab_c_get_vertex_x(Part.Feature.Handle, Part.Index, index),MiApi.mitab_c_get_vertex_y(Part.Feature.Handle, Part.Index, index)) : null;
			}
		}

		public override object GetObj(int idx) 
        {
			return this[idx];
		}
		
		public override string ToString() 
        {
			StringBuilder str = new StringBuilder();
			foreach (Topology.Geometries.Coordinate v in this)
				str.Append(v+"\t");
			return str.ToString();
		}
	}

	/// <summary>
	/// Represents a Part.
	/// </summary>
	/// <remarks>A feature will contain one or more parts.</remarks>
	public class Part 
    {
		public readonly Feature Feature;
		public readonly Vertices Vertices;
		public readonly int Index;

		protected internal Part(Feature feature, int partIdx) 
        {
			this.Index = partIdx;
			this.Feature = feature;
			this.Vertices = CreateVertices(this);
		}

		/// <summary>
		/// Override this to support descendants of the Vertices class.
		/// </summary>
		/// <returns>This parts vertices.</returns>
		protected internal virtual Vertices CreateVertices(Part part) 
        {
			return new Vertices(this);
		}

		public override string ToString() 
        {
			return "Part: "+Index+"\nVertices:\n"+this.Vertices.ToString();
		}

	}

	/// <summary>
	/// Contains the set of Parts belonging to a single Feature.
	/// </summary>
	/// <remarks>This class descends EnumImple, meaning the Parts in the
	/// set can be iterated using foreach.
	/// It also has an index property allowing any Part between 0 and Parts.Count-1
	/// to be accessed directly with Parts[idx]
	/// </remarks>
	public class Parts :  EnumImpl 
    {
		/// <summary>
		/// The feature these parts belong to.
		/// </summary>
		public readonly Feature Feature;

		protected internal Parts(Feature feature):base(MiApi.mitab_c_get_parts(feature.Handle)) 
        {
			this.Feature = feature;
		}

		/// <summary>
		/// Override this to support descendants of the Part class.
		/// </summary>
		/// <returns>A part with the given index</returns>
		protected internal virtual Part CreatePart(int partIdx) 
        {
			return new Part(this.Feature, partIdx);
		}

		public Part this[int index] 
        {
			get 
            {
				return index < Count ? CreatePart(index) : null;
			}
		}


		public override object GetObj(int idx) 
        {
			return this[idx];
		}

		public override string ToString() 
        {
			StringBuilder str = new StringBuilder();
			str.Append("Part Count: "+this.Count+"\n");
			foreach (Part part in this) 
				str.Append(part.ToString()+"\n");
			return str.ToString();
		}

	}

	public class FeatureStyle
	{
		private IntPtr m_feature;

		public FeatureStyle(IntPtr feature)
		{
			m_feature = feature;
		}

		public int PenColor
		{
			get
			{
				return MiApi.mitab_c_get_pen_color(m_feature);
			}
			set
			{
				MiApi.mitab_c_set_pen(m_feature, PenWidth, PenPattern, value);
			}
		}

		public int PenWidth
		{
			get
			{
				return MiApi.mitab_c_get_pen_width(m_feature);
			}
			set
			{
				MiApi.mitab_c_set_pen(m_feature, value, PenPattern, PenColor);
			}
		}

		public int PenPattern
		{
			get
			{
				return MiApi.mitab_c_get_pen_pattern(m_feature);
			}
			set
			{
				MiApi.mitab_c_set_pen(m_feature, PenWidth, value, PenColor);
			}
		}

		public int BrushForeColor
		{
			get
			{
				return MiApi.mitab_c_get_brush_fgcolor(m_feature);
			}
			set
			{
				MiApi.mitab_c_set_brush(m_feature, value, BrushBackColor, BrushPattern, BrushTransparent ? 1 : 0);
			}
		}

		public int BrushBackColor
		{
			get
			{
				return MiApi.mitab_c_get_brush_bgcolor(m_feature);
			}
			set
			{
				MiApi.mitab_c_set_brush(m_feature, BrushForeColor, value, BrushPattern, BrushTransparent ? 1 : 0);
			}
		}

		public int BrushPattern
		{
			get
			{
				return MiApi.mitab_c_get_brush_pattern(m_feature);
			}
			set
			{
				MiApi.mitab_c_set_brush(m_feature, BrushForeColor, BrushBackColor, value, BrushTransparent ? 1 : 0);
			}
		}

		public bool BrushTransparent
		{
			get
			{
				return MiApi.mitab_c_get_brush_transparent(m_feature) == 1;
			}
			set
			{
				MiApi.mitab_c_set_brush(m_feature, BrushForeColor, BrushBackColor, BrushPattern, value ? 1 : 0);
			}
		}
	}

	/// <summary>
	/// Represents a single feature beloning to a layer.
	/// </summary>
	public class Feature : IDisposable, LittleSharpRenderEngine.IFeature
    {
		/// <summary>
		/// Handle used to manipulate the object in the C API.
		/// </summary>
		public readonly IntPtr Handle;
		/// <summary>
		/// The id of this feature.
		/// </summary>
		public readonly int Id;
		/// <summary>
		/// The feature type.
		/// </summary>
		public readonly FeatureType Type;
		/// <summary>
		/// The layer the Feature belongs to.
		/// </summary>
		public readonly Layer Layer;
		/// <summary>
		/// The set of parts comprising this feature.
		/// </summary>
		public readonly Parts Parts;
		/// <summary>
		/// The actual style ... added by mkv
		/// </summary>
		public readonly FeatureStyle Style;

		private DataValues m_data;

		public DataValues Data
		{
			get
			{
				if (m_data == null) m_data = new DataValues(this);
				return m_data;
			}
		}

		protected internal Feature(Layer layer, int featureId) 
        {
			this.Id = featureId;
			this.Layer = layer;
			this.Handle = MiApi.mitab_c_read_feature(layer.Handle, featureId);
			this.Type = MiApi.mitab_c_get_type(Handle);
			this.Style = new FeatureStyle(Handle);
			this.Parts = CreateParts(this);
		}

		/// <summary>
		/// Override this to support descendants of the Parts class.
		/// </summary>
		/// <returns>This layers fields</returns>
		protected internal virtual Parts CreateParts(Feature feature) 
        {
			return new Parts(this);
		}

		/// <summary>
		/// Returns text associated with this feature
		/// </summary>
		/// <remarks>This will return an empty string, unless this features type is
		/// TABFC_Text.</remarks>
		public string Text 
        {
			get 
            {
				return (this.Type == FeatureType.TABFC_Text) ? MiApi.mitab_c_get_text(this.Handle) : "";
			}
		}

		public override string ToString()
        {
			StringBuilder str = new StringBuilder();
			str.Append("Feature: "+Id+"\nFields:\n");

			foreach (Field field in this.Layer.Fields)
				str.Append(field.GetValueAsString(this).Trim()+"\t");

			str.Append("\n"+this.Parts.ToString());

			return str.ToString();
		}

		/// <summary>
		/// Convenience method to return the next Feature in the file.
		/// </summary>
		/// <returns>A following feature in the file.</returns>
		public Feature GetNext() 
        {
			return new Feature(this.Layer, MiApi.mitab_c_next_feature_id(Layer.Handle, this.Id));
		}

		private bool disposed = false;

		public void Dispose(bool disposing) 
        {
			if (disposing && !disposed) 
            {
				MiApi.mitab_c_destroy_feature(this.Handle);
				disposed = true;
			}
		}

		public void Dispose() 
        {
			Dispose(true);
		}

		~Feature() 
        {
			Dispose(false);
		}

		#region IFeature Members

		private Topology.Geometries.IGeometry m_IGeometri;
		private LittleSharpRenderEngine.Style.IStyle m_IStyle;

		public Topology.Geometries.IGeometry Geometry
		{
			get 
			{
				if (m_IGeometri != null) return m_IGeometri;
				switch (this.Type)		
				{
					case FeatureType.TABFC_NoGeom:
						return null;
					case FeatureType.TABFC_Point:
						m_IGeometri = new Topology.Geometries.Point(this.Parts[0].Vertices[0]);
						break;
					case FeatureType.TABFC_Polyline:
						Topology.Geometries.ICoordinate[] arr = new Topology.Geometries.ICoordinate[this.Parts[0].Vertices.Count];
						int i = 0;
						foreach (Topology.Geometries.ICoordinate c in this.Parts[0].Vertices)
						{
							arr[i++] = c;
						}
						m_IGeometri = new Topology.Geometries.LineString(arr);
						break;
					case FeatureType.TABFC_Region:
						Topology.Geometries.Polygon[] polygons = new Topology.Geometries.Polygon[this.Parts.Count];
						int p = 0;
						foreach (Part pa in this.Parts)
						{
							Topology.Geometries.ICoordinate[] coords = new Topology.Geometries.ICoordinate[pa.Vertices.Count];
							int ii = 0;
							foreach (Topology.Geometries.ICoordinate c in pa.Vertices)
							{
								coords[ii++] = c;
							}
							polygons[p++] = new Topology.Geometries.Polygon(new Topology.Geometries.LinearRing(coords));
						}
						if (polygons.Length > 1) m_IGeometri = new Topology.Geometries.MultiPolygon(polygons);
						else m_IGeometri = polygons[0];
						break;
					default:
						throw new Exception("MapInfo type " + this.Type.ToString() + " not yet supported");
				}
				return m_IGeometri;
			}
		}

		LittleSharpRenderEngine.Style.IStyle LittleSharpRenderEngine.IFeature.Style
		{
			get 
			{
				if (m_IStyle != null) return m_IStyle;
				switch (this.Type)
				{
					case FeatureType.TABFC_NoGeom:
						return null;
					case FeatureType.TABFC_Point:
						LittleSharpRenderEngine.Style.Point ps = new LittleSharpRenderEngine.Style.Point();
						ps.Type = LittleSharpRenderEngine.Style.Point.PointType.Circle;
						m_IStyle = ps;
						break;
					case FeatureType.TABFC_Polyline:
						m_IStyle = new LittleSharpRenderEngine.Style.Line(System.Drawing.Color.FromArgb(this.Style.PenColor), this.Style.PenWidth);
						break;
					case FeatureType.TABFC_Region:
						LittleSharpRenderEngine.Style.Area a = new LittleSharpRenderEngine.Style.Area();
						a.Outline.ForegroundColor = System.Drawing.Color.FromArgb(255, System.Drawing.Color.FromArgb(this.Style.PenColor));
						a.Outline.Width = this.Style.PenWidth;
						a.Fill.BackgroundColor = System.Drawing.Color.FromArgb(255, System.Drawing.Color.FromArgb(this.Style.BrushBackColor));
						a.Fill.ForegroundColor = System.Drawing.Color.FromArgb(255, System.Drawing.Color.FromArgb(this.Style.BrushForeColor));
						m_IStyle = a;
						break;
					default:
						throw new Exception("MapInfo type " + this.Type.ToString() + " not yet supported");
				}
				return m_IStyle;
			}
		}

		#endregion
	}

	/// <summary>
	/// Unlike the other enumerators. The feature id set isn't guaranteed to be sequential. 
	/// So we override the default seqeuntial iterator.
	/// </summary>
	internal class FeaturesEnum : IndexedEnum 
    {

		private readonly Layer layer;

		internal FeaturesEnum(IObjectProvider objProvider, Layer layer) : base(objProvider) 
        {
			this.layer = layer;
		}

		public override bool MoveNext() 
        {
			return (eIdx = MiApi.mitab_c_next_feature_id(layer.Handle, eIdx)) != -1;
		}
	}

	/// <summary>
	/// Contains the set of features belonging to a single layer.
	/// </summary>
	/// <remarks>This class descends EnumImpl, meaning the features in the
	/// set can be iterated using foreach.
	/// It also has an index property allowing any feature between 0 and Features.Count-1
	/// to be accessed directly with Features[idx]</remarks>
	public class Features : EnumImpl 
    {
		/// <summary>
		/// The layer the features belong to
		/// </summary>
		public readonly Layer Layer;

		protected internal Features(Layer layer) : base(MiApi.mitab_c_get_feature_count(layer.Handle)) 
        {
			this.Layer = layer;
		}

		/// <summary>
		/// Override this to support descendants of the Feature class.
		/// </summary>
		/// <returns>This layers fields</returns>
		protected internal virtual Feature CreateFeature(int index) 
        {
			return new Feature(this.Layer, index);
		}

		public Feature this[int index] 
        {
			get 
            {
				return (index != -1) ? CreateFeature(index) : null;
			}
		}

		public Feature GetFirst() 
        {
			return this[MiApi.mitab_c_next_feature_id(Layer.Handle, -1)];
		}

		public override object GetObj(int idx) 
        {
			return this[idx];
		}

		public override IEnumerator GetEnumerator() 
        {
			return new FeaturesEnum(this, Layer);
		}

		public override string ToString() 
        {
			StringBuilder str = new StringBuilder();
			str.Append("Feature Count: "+this.Count+"\n");
			foreach (Feature feature in this) 
				str.Append(feature.ToString()+"\n");
			return str.ToString();
		}

	}

	public class Layer 
    {
		/// <summary>
		/// Handle used to manipulate the object in the C API
		/// </summary>
		public readonly IntPtr Handle;
		public readonly Fields Fields;
		public readonly Features Features;
		public readonly string FileName;

		protected internal Layer(string fileName) 
        {
			this.Handle = MiApi.mitab_c_open(fileName);
			if (this.Handle == IntPtr.Zero)
				throw new FileNotFoundException("File "+fileName+" not found", fileName);
			this.Fields = CreateFields();
			this.Features = CreateFeatures();
			this.FileName = fileName;
		}

		public MiApi MiApi
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
			}
		}

		/// <summary>
		/// Override this to support descendants of the Fields class.
		/// </summary>
		/// <returns>This layers fields</returns>
		protected internal virtual Fields CreateFields() 
        {
			return new Fields(this);
		}

		/// <summary>
		/// Override this to support descendants of the Feature class.
		/// </summary>
		/// <returns>This layers features</returns>
		protected internal virtual Features CreateFeatures() 
        {
			return new Features(this);
		}

		/// <summary>
		/// Factory method to return the layer with a given name.
		/// </summary>
		/// <param name="tabFileName"></param>
		/// <returns></returns>
		public static Layer GetByName(string tabFileName) 
        {
			return new Layer(tabFileName);
		}

		public override string ToString() 
        {
			return "Layer: "+this.FileName;
		}

		/// <summary>
		/// Writes this layers features to the given textwriter
		/// </summary>
		/// <param name="writer">Destintation for the layers features</param>
		public void ToText(TextWriter writer) 
        {
			writer.WriteLine(this);
			writer.WriteLine(this.Fields+"\n");
			writer.WriteLine(this.Features);
		}

		/// <summary>
		/// Writes this layers features as a text file.
		/// </summary>
		/// <param name="fileName">The name of the file that will be created.</param>
		public void ToText(string fileName) 
        {
			ToText(new StreamWriter(fileName));
		}
	}
}
