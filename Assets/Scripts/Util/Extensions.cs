using UnityEngine;
using System;
using System.Collections;

public static class Extensions {
	public static Transform FindChildByTag(this Transform target, String tag)
	{
		if (target.CompareTag(tag)) return target;
		
		for (int i = 0; i < target.childCount; ++i)
		{
			var result = FindChildByTag(target.GetChild(i), tag);
			
			if (result != null) return result;
		}
		
		return null;
	}
}
