using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;

public class GOEditorUtility : MonoBehaviour {
	
	
	// see: http://answers.unity3d.com/questions/458207/copy-a-component-at-runtime.html
	public static bool CopyComponent<T>(T src, GameObject destinationObj) where T : Component
	{
		System.Type type = typeof(T);
		var dst = destinationObj.GetComponent<T> ();
		
		if (dst == null) {
			dst = (T)destinationObj.AddComponent (type);
		}
		
		// see: http://answers.unity3d.com/answers/603984/view.html
		return ComponentUtility.CopyComponent (src) && ComponentUtility.PasteComponentValues (dst);
		
		//		System.Reflection.FieldInfo[] fields = type.GetFields(
		//			BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly | BindingFlags.FlattenHierarchy);
		//		foreach (System.Reflection.FieldInfo field in fields)
		//		{
		//			field.SetValue(dst, field.GetValue(original));
		//		}
		//		return dst as T;
	}
}
