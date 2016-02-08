using UnityEngine;
using System;
using System.Collections;

public static class Extensions {
	public static Transform FindChildByTag(this Transform target, String tag)
	{
		if (target.CompareTag(tag)) return target;
		
		// recurse
		for (int i = 0; i < target.childCount; ++i) {
			var result = FindChildByTag(target.GetChild(i), tag);
			if (result != null) return result;
		}
		
		return null;
	}
	
	/// <summary>
	/// Recursively looks for the first descendant object of given name.
	/// </summary>
	/// <returns>The first descendant by name.</returns>
	public static C FindFirstDescendantByName<C>(this Transform target, String name)
	{
		var child = target.FindChild (name);
		if (child != null) {
			var component = child.GetComponent<C>();
			if (component != null) {
				return component;
			}
		}
		
		// recurse
		for (int i = 0; i < target.childCount; ++i) {
			C result = FindFirstDescendantByName<C>(target.GetChild(i), name);
			if (result != null) return result;
		}
		
		return default(C);
	}
	
	/// <summary>
	/// Recursively looks for the first descendant object of given name.
	/// </summary>
	/// <returns>The first descendant by name.</returns>
	public static Transform FindFirstDescendantByName(this Transform target, String name)
	{
		var child = target.FindChild (name);
		if (child != null) {
			return child;
		}
		
		// recurse
		for (int i = 0; i < target.childCount; ++i) {
			var result = FindFirstDescendantByName(target.GetChild(i), name);
			if (result != null) return result;
		}
		
		return null;
	}
}
