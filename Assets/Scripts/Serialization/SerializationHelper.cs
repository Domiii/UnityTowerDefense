////using UnityEngine;
//
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//
//
//#region Serializable Types
//[System.Serializable]
//public class ReferenceTypeInfo {
//	public string AssemblyName;
//	public string TypeName;
//
//	public ReferenceTypeInfo() {
//	}
//
//	public ReferenceTypeInfo(Type type) {
//		AssemblyName = type.Assembly.GetName().FullName;
//		TypeName = type.FullName;
//	}
//
//	public Type ResolveType() {
//		var asm = SerializationHelper.GetAssemblyByName(AssemblyName);
//		if (asm == null) {
//			UnityEngine.Debug.LogWarning("Could not find Assembly for deserialization: " + AssemblyName);
//			return null;
//		}
//
//		var type = asm.GetType(TypeName);
//		if (type == null) {
//			UnityEngine.Debug.LogWarning("Could not find Type for deserialization: " + TypeName);
//		}
//
//		return type;
//	}
//}
//
//[System.Serializable]
//public class SimplifiedSerializableObject {
//	public ReferenceTypeInfo TypeInfo;
//	public string FieldName;
//
//	/// <summary>
//	/// In case, this is a simple value type
//	/// </summary>
//	public string SimpleValue;
//
//	/// <summary>
//	/// In case, this is not a simple value type.
//	/// </summary>
//	public SimplifiedSerializableObject[] NestedValues;
//}
//#endregion
//
//
//#region Serialization
//public class ReferenceObjectSerializer {
//	object Obj;
//
//	public ReferenceObjectSerializer(object o) {
//		Obj = o;
//	}
//	
//	public SimplifiedSerializableObject Serialize() {
//		return SerializeObject ("", Obj);  // root has no name
//	}
//	
//	public SimplifiedSerializableObject SerializeObject(string name, object value) {
//		var so = new SimplifiedSerializableObject ();
//		so.FieldName = name;
//
//		if (value == null) {
//			return so;
//		}
//
//		// TODO: Sanity checks (must be array or string if IEnumerable etc)
//
//		var type = value.GetType ();
//		if (type.IsSimpleType()) {
//			SerializeValue(so, value);
//		}
//		else if (type.IsArray) {
//			SerializeArray(so, value as Array);
//		}
//		else {
//			SerializeNestedFields(so, value);
//		}
//
//		return so;
//	}
//	
//	public void SerializeValue(SimplifiedSerializableObject so, object value) {
//		var type = value.GetType ();
//		so.TypeInfo = new ReferenceTypeInfo (type);
//
//		so.SimpleValue = value + "";
//	}
//
//	public void SerializeArray(SimplifiedSerializableObject so, Array arr) {
//		var type = arr.GetType ();
//		var elementType = type.GetElementType ();
//		so.TypeInfo = new ReferenceTypeInfo (elementType);
//
//		var nestedValues = new SimplifiedSerializableObject [arr.Length];
//
//		for (var i = 0; i < arr.Length; ++i) {
//			var val = arr.GetValue(i);
//			nestedValues[i] = SerializeObject(i.ToString(), val);
//		}
//
//		so.NestedValues = nestedValues.ToArray ();
//	}
//	
//	public void SerializeNestedFields(SimplifiedSerializableObject so, object value) {
//		var type = value.GetType ();
//		so.TypeInfo = new ReferenceTypeInfo (type);
//		
//		var nestedValues = new List<SimplifiedSerializableObject> ();
//		
//		// only public, writable fields for now
//		foreach (var member in type.GetMembers(BindingFlags.Public | BindingFlags.Instance)) {
//			if (!(member is FieldInfo) || member.IsReadonly()) {
//				continue;
//			}
//			
//			var field = (FieldInfo)member;
//			var varType = field.FieldType;
//			object nestedValue = field.GetValue(value);
//
//			var nestedSo = SerializeObject(field.Name, nestedValue);
//			nestedValues.Add(nestedSo);
//		}
//
//		so.NestedValues = nestedValues.ToArray ();
//	}
//}
//#endregion
//
//
//#region Deserialization
//public class ReferenceObjectDeserializer {
//	SimplifiedSerializableObject SO;
//	
//	public ReferenceObjectDeserializer(SimplifiedSerializableObject o) {
//		SO = o;
//	}
//	
//	public object Deserialize() {
//		return Deserialize (SO);
//	}
//
//	public object Deserialize(SimplifiedSerializableObject so) {
//		var typeInfo = so.TypeInfo;
//		if (typeInfo == null) {
//			// most probably null
//			return null;
//		}
//
//		var type = typeInfo.ResolveType();
//		if (type == null) {
//			// could not resolve type
//			return null;
//		}
//
//		object value;
//		if (type.IsSimpleType ()) {
//			value = DeserializeSimpleValue (so, type);
//		} else if (type.IsArray) {
//			value = DeserializeArray (so, type);
//		} else {
//			value = DeserializeNestedFields(so, type);
//		}
//		return value;
//
////		if (m_isXmlSerializable)
////		{
////			if (Value == null)
////			{
////				Value = Activator.CreateInstance(type);
////			}
////			((IXmlSerializable)Value).ReadXml(reader);
////		}
////		else 
////		else
////		{
////			if (m_collectionType != null)
////			{
////				IList collection;
////				if (m_Member.GetVariableType().IsArray)
////				{
////					//collection = (IList)Activator.CreateInstance(varType);
////					collection = new List<object>();
////					ReadCollection(reader, collection);
////					var arr = Array.CreateInstance(m_collectionType, collection.Count);
////					for (var i = 0; i < collection.Count; i++)
////					{
////						ArrayUtil.SetValue(arr, i, collection[i]);
////					}
////					Value = arr;
////				}
////				else
////				{
////					collection = (IList)Activator.CreateInstance(type);
////					ReadCollection(reader, collection);
////					Value = collection;
////				}
////			}
////			else
////			{
////				// should never happen due to the initial checks
////				throw new NotImplementedException("Cannot serialize Variable because it has an invalid Type: " + type);
////			}
////		}
//
//	}
//
//	public object DeserializeSimpleValue(SimplifiedSerializableObject so, Type type) {
//		var str = so.SimpleValue + "";
//		object result = null;
//		if (!SerializationHelper.ParseSingleValue(str, type, ref result)) {
//			UnityEngine.Debug.LogWarning("Could not parse simple value of type: " + type);
//		}
//		return result;
//	}
//	
//	public object DeserializeArray(SimplifiedSerializableObject so, Type elementType) {
//		if (so.NestedValues == null) {
//			return null;
//		}
//
//		var arr = Array.CreateInstance (elementType, so.NestedValues.Length);
//		for (int i = 0; i < so.NestedValues.Length; ++i) {
//			var nestedSo = so.NestedValues [i];
//
//			var nestedValue = Deserialize (nestedSo);
//			arr.SetValue(nestedValue, i);
//		}
//		return arr;
//	}
//	
//	public object DeserializeNestedFields(SimplifiedSerializableObject so, Type type) {
//		if (so.NestedValues == null) {
//			return null;
//		}
//
//		var value = Activator.CreateInstance(type);
//		for (int i = 0; i < so.NestedValues.Length; ++i) {
//			var nestedSo = so.NestedValues[i];
//
//			var field = type.GetField (nestedSo.FieldName);
//			if (field == null) {
//				UnityEngine.Debug.LogWarning("Serialized field does not exist anymore: " + type.FullName + "." + nestedSo.FieldName);
//				continue;
//			}
//			var nestedValue = Deserialize(nestedSo);
//			field.SetValue(value, nestedValue);
//		}
//		return value;
//	}
//
//}
//#endregion
//
//public static class SerializationHelper {
//	public static readonly object[] EmptyObjectArray = new object[0];
//	public static readonly System.Type GenericListType = typeof(IList<>);
//
//	#region Reflection Utilities
//	public static bool IsSimpleType(this Type type)
//	{
//		return type.IsEnum || type.IsPrimitive || type == typeof(string);
//	}
//
//	public static bool IsReadonly(this MemberInfo member) {
//		if (member is FieldInfo)
//		{
//			return ((FieldInfo)member).IsInitOnly || ((FieldInfo)member).IsLiteral;
//		}
//		else if (member is PropertyInfo)
//		{
//			return !((PropertyInfo)member).CanWrite || ((PropertyInfo)member).GetSetMethod() == null ||
//				!((PropertyInfo)member).GetSetMethod().IsPublic;
//		}
//		return true;
//	}
//	#endregion
//
//	#region String Parsing Utilities
//	public static Dictionary<Type, Func<string, object>> TypeParsers =
//		new Func<Dictionary<Type, Func<string, object>>>(() =>
//		                                                 {
//			var parsers =
//				new Dictionary<Type, Func<string, object>>();
//			
//			parsers.Add(typeof(int),
//			            strVal => int.Parse(strVal));
//			
//			parsers.Add(typeof(float),
//			            strVal => float.Parse(strVal));
//			
//			parsers.Add(typeof(long),
//			            strVal => long.Parse(strVal));
//			
//			parsers.Add(typeof(ulong),
//			            strVal => ulong.Parse(strVal));
//			
//			parsers.Add(typeof(bool),
//			            strVal =>
//			            strVal.Equals("true",
//			              StringComparison.
//			              InvariantCultureIgnoreCase) ||
//			            strVal.Equals("1",
//			              StringComparison.
//			              InvariantCultureIgnoreCase) ||
//			            strVal.Equals("yes",
//			              StringComparison.
//			              InvariantCultureIgnoreCase));
//			
//			parsers.Add(typeof(double),
//			            strVal => double.Parse(strVal));
//			
//			parsers.Add(typeof(uint),
//			            strVal => uint.Parse(strVal));
//			
//			parsers.Add(typeof(short),
//			            strVal => short.Parse(strVal));
//			
//			parsers.Add(typeof(ushort),
//			            strVal => short.Parse(strVal));
//			
//			parsers.Add(typeof(byte),
//			            strVal => byte.Parse(strVal));
//			
//			parsers.Add(typeof(char), strVal => strVal[0]);
//			
//			return parsers;
//		})();
//	
//	public static bool ParseSingleValue(string str, Type type, ref object obj)
//	{
//		if (type == typeof(string))
//		{
//			obj = str;
//		}
//		else if (type.IsEnum)
//		{
//			try
//			{
//				obj = Enum.Parse(type, str, true);
//			}
//			catch
//			{
//				return false;
//			}
//		}
//		else
//		{
//			Func<string, object> parser;
//			if (TypeParsers.TryGetValue(type, out parser))
//			{
//				try
//				{
//					obj = parser(str);
//					return obj != null;
//				}
//				catch
//				{
//					return false;
//				}
//			}
//			return false;
//		}
//		return true;
//	}
//	#endregion
//
//	/// <summary>
//	/// Attempts to convert any object into a serializable array of objects.
//	/// </summary>
//	public static SimplifiedSerializableObject Serialize(object o) {
//		var serializer = new ReferenceObjectSerializer (o);
//		return serializer.Serialize ();
//	}
//
//
//	public static object Deserialize(SimplifiedSerializableObject so) {
//		var deserializer = new ReferenceObjectDeserializer (so);
//		return deserializer.Deserialize ();
//	}
//	
//	public static Assembly GetAssemblyByName(string fullName) {
//		return AppDomain.CurrentDomain.GetAssemblies().
//			SingleOrDefault(assembly => assembly.GetName().FullName == fullName);
//	}
//}
