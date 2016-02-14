using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class CustomScriptableObjectAttribute : System.Attribute {
	public string Name;
	public string Description;

	public CustomScriptableObjectAttribute(string name, string description = "") {
		Name = name;
		Description = description;
	}
}

public class CustomScriptableObjectEntry {
	public System.Type Type;
	public CustomScriptableObjectAttribute Attribute;

	public string Name {
		get { return !string.IsNullOrEmpty(Attribute.Name) ? Attribute.Name : Type.Name; }
	}
}

/// <summary>
/// Manage Behavior scripts. 
/// </summary>
public class CustomScriptableObjectManager {
	public static void GetAllTypesWithAttribute<A> (Assembly assembly, System.Action<System.Type, A> cb)
		where A : System.Attribute
	{
		foreach (var type in assembly.GetTypes()) {
			var attrs = type.GetCustomAttributes(typeof(A), true);
			if (attrs.Length > 0) {
				var attr = (A)attrs[0];
				cb(type, attr);
			}
		}
	}

	public static CustomScriptableObjectManager FindAllCustomScriptableObjects() {
		var mgr = new CustomScriptableObjectManager ();
		GetAllTypesWithAttribute<CustomScriptableObjectAttribute>(Assembly.GetAssembly(typeof(CustomScriptableObjectAttribute)), mgr.AddEntry);
		return mgr;
	}
	
	static bool IsSameOrSubclass(System.Type baseType, System.Type subclass) {
		return subclass.IsSubclassOf(baseType)
			|| subclass == baseType;
	}

	public List<CustomScriptableObjectEntry> Scripts;
	CustomScriptableObjectManager() {
		Scripts = new List<CustomScriptableObjectEntry> ();
	}

	void AddEntry(System.Type type, CustomScriptableObjectAttribute attr) {
		Scripts.Add(new CustomScriptableObjectEntry() {
			Type = type,
			Attribute = attr
		});
	}

	public IEnumerable<CustomScriptableObjectEntry> GetEntries<T>() {
		return Scripts.Where(script => IsSameOrSubclass(typeof(T), script.Type));
	}
}
