using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class BehaviorScriptAttribute : System.Attribute {
	public string Name;
	public string Description;

	public BehaviorScriptAttribute(string name, string description = "") {
		Name = name;
		Description = description;
	}
}

public class BehaviorScriptEntry {
	public System.Type Type;
	public BehaviorScriptAttribute Attribute;

	public string Name {
		get { return !string.IsNullOrEmpty(Attribute.Name) ? Attribute.Name : Type.Name; }
	}
}

/// <summary>
/// Manage Behavior scripts. 
/// </summary>
public class BehaviorScriptManager {
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

	public static BehaviorScriptManager FindAllBehaviorScripts() {
		var mgr = new BehaviorScriptManager ();
		GetAllTypesWithAttribute<BehaviorScriptAttribute>(Assembly.GetAssembly(typeof(BehaviorScriptAttribute)), mgr.AddEntry);
		return mgr;
	}
	
	static bool IsSameOrSubclass(System.Type baseType, System.Type subclass) {
		return subclass.IsSubclassOf(baseType)
			|| subclass == baseType;
	}

	public List<BehaviorScriptEntry> Scripts;
	BehaviorScriptManager() {
		Scripts = new List<BehaviorScriptEntry> ();
	}

	void AddEntry(System.Type type, BehaviorScriptAttribute attr) {
		Scripts.Add(new BehaviorScriptEntry() {
			Type = type,
			Attribute = attr
		});
	}

	public IEnumerable<BehaviorScriptEntry> GetEntries<T>() {
		return Scripts.Where(script => IsSameOrSubclass(typeof(T), script.Type));
	}
}
