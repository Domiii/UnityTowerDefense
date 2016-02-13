using UnityEngine;
using UnityEngine.UI;

using UnityEditor;

using System.Collections.Generic;
using System.Reflection;

using Spells;
using System;

[CustomEditor(typeof(SpellManager))]
public class SpellManagerEditor : Editor {
	public static void GetAllTypesWithAttribute<A> (Assembly assembly, Action<System.Type, A> cb)
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
}
