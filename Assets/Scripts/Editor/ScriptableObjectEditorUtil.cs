using UnityEngine;
using UnityEngine.UI;

using UnityEditor;
using UnityEditorInternal;

using System.Collections.Generic;
using System.Reflection;
using System.Linq;


public class ScriptableObjectEditorUtil : Editor {
	static ScriptableObjectUtil scripts;
	public static ScriptableObjectUtil Scripts {
		get {
			if (scripts == null) {
				OnReloadScripts();
			}
			return scripts;
		}
	}

	[UnityEditor.Callbacks.DidReloadScripts]
	public static void OnReloadScripts() {
		scripts = ScriptableObjectUtil.FindAllCustomScriptableObjects ();
	}
	
	#region Object Management
	public static T AddNewObjectToAsset<T>(Object owner) 
		where T : ScriptableObject
	{
		return (T)AddNewObjectToAsset(owner, typeof(T));
	}
	
	public static ScriptableObject AddNewObjectToAsset(Object owner, System.Type type) {
		var newObj = ScriptableObject.CreateInstance(type);
		newObj.hideFlags = HideFlags.HideInHierarchy;
		newObj.name = type.Name;
		AssetDatabase.AddObjectToAsset(newObj, owner);
		AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(newObj));
		AssetDatabase.SaveAssets();
		
		return newObj;
	}
	
	public static void DeleteObjectFromAsset(ScriptableObject obj) {
		UnityEngine.Object.DestroyImmediate (obj, true);
	}
	#endregion
	
	
	#region GUI Components
	public static T ToggleAddRemoveScriptableObject<T>(string label, Object owner, T obj)
		where T : ScriptableObject
	{
		return ToggleAddRemoveScriptableObject (label, owner, obj, typeof(T));
	}
	
	public static T ToggleAddRemoveScriptableObject<T>(string label, Object owner, T obj, System.Type type) 
		where T : ScriptableObject
	{
		Debug.Assert (type == typeof(T) || type.IsSubclassOf(typeof(T)), "Given type (should be, but) is not equal to and does not inherit from T");
		var wasAdded = obj != null;
		var isObjectAdded = EditorGUILayout.Toggle (label, wasAdded);
		if (isObjectAdded != wasAdded) {
			if (!isObjectAdded) {
				// remove
				DeleteObjectFromAsset(obj);
				obj = null;
			}
			else {
				// add
				obj = (T)AddNewObjectToAsset(owner, type);
			}
		}
		return obj;
	}
	#endregion
}

// TODO: Rename or hide different options in different contexts

public class ReorderableListManager<A>
	where A : ScriptableObject
{
	Object owner;
	Dictionary<A[], ScriptableObjectReorderableList<A>> lists;
	//SerializedObject so;

	public ReorderableListManager(Object owner) {
		this.owner = owner;
		lists = new Dictionary<A[], ScriptableObjectReorderableList<A>> ();
	}
	
	public ScriptableObjectReorderableList<A> GetOrCreate(string label, SerializedObject so, SerializedProperty prop, A[] arr) {
		if (arr == null) {
			return null;
		}

		ScriptableObjectReorderableList<A> wrapper;
		if (!lists.TryGetValue (arr, out wrapper)) {
			wrapper = new ScriptableObjectReorderableList<A>(label, owner, arr, so, prop);
			lists.Add(arr, wrapper);
		}
		return wrapper;
	}
}

// TODO: Wrap ReorderableList to reflect array of custom ScriptableObjects
// see: http://va.lent.in/unity-make-your-lists-functional-with-reorderablelist/
public class ScriptableObjectReorderableList<A>
	where A : ScriptableObject
{
	string label;
	Object owner;
	protected A[] arr;
	SerializedObject serializedObject;
	IEnumerable<CustomScriptableObjectEntry> possibleEntries;
	ReorderableList list;
	
	public ScriptableObjectReorderableList(string label, Object owner, A[] arr, SerializedObject serializedObject, SerializedProperty serializedProperty) {
		if (serializedProperty == null) {
			throw new System.ArgumentNullException("serializedProperty");
		}
		this.label = label;
		this.owner = owner;
		this.arr = arr;
		this.serializedObject = serializedObject;
		
		possibleEntries = ScriptableObjectEditorUtil.Scripts.GetEntries<A>();
		list = new ReorderableList(serializedObject, serializedProperty, true, label != null, true, true);
		list.drawElementCallback = OnDrawElement;
		list.drawHeaderCallback = OnDrawHeader;
		list.onAddDropdownCallback = OnAddDropdown;
		list.onRemoveCallback = OnRemove;
	}
	
	public void Render() {
		var height = 0f;

//		EditorGUILayout.BeginHorizontal ();
//		//EditorGUI.
//		EditorGUILayout.EndHorizontal ();

		serializedObject.Update ();

		// compute height
		// Hack: Just using GetPropertyHeight on the array element won't work, so we have to iterate through the whole thing...
		for(var i = 0; i < list.serializedProperty.arraySize; ++i) {
			var h = EditorGUIUtility.singleLineHeight;
			
			var element = list.serializedProperty.GetArrayElementAtIndex(i);
			var obj = element.objectReferenceValue;
			if (obj == null) continue;

			var so = new SerializedObject (obj);
			var prop = so.GetIterator ();

			if (prop.hasChildren) {
				prop.NextVisible(true);		// ignore script
				while (prop.NextVisible(true)) {
					h += EditorGUI.GetPropertyHeight(prop);
				}
			}
			height = Mathf.Max(height, h);
		}
		list.elementHeight = height;
		list.DoLayoutList ();
	}
	
	void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused) {
		var element = list.serializedProperty.GetArrayElementAtIndex(index);
		var obj = element.objectReferenceValue;
		if (obj == null) {
			return;
		}
		
		// Hack: Just using PropertyField on the array element won't work, so we have to iterate through the whole thing...

		var h = EditorGUIUtility.singleLineHeight;
		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		var name = ScriptableObjectUtil.GetName(obj.GetType());
		EditorGUI.LabelField(rect, name, EditorStyles.boldLabel);
		rect.y += h;

		var so = new SerializedObject (obj);
		var prop = so.GetIterator ();
		if (prop.hasChildren) {
			prop.NextVisible(true);		// ignore script
			while (prop.NextVisible(true)) {
				EditorGUI.PropertyField(rect, prop, true);
				rect.y += h;
			}
		}

		EditorGUI.indentLevel = indent;
		so.ApplyModifiedProperties();
	}
	
	void OnDrawHeader(Rect rect) {
		if (label != null) {
			EditorGUI.LabelField (rect, label);
		}
	}
	
	void OnAddDropdown(Rect buttonRect, ReorderableList list) {  
		var menu = new GenericMenu();
		foreach (var entry in possibleEntries) {
			menu.AddItem(new GUIContent(entry.Name),
			             false, 
			             OnDropdownClick, 
			             entry);
		}
		menu.ShowAsContext();
	}
	
	void OnDropdownClick(object source) {
		var entry = (CustomScriptableObjectEntry)source;

		// create and add enw object
		var obj = (A)ScriptableObjectEditorUtil.AddNewObjectToAsset (owner, entry.Type);
		var index = list.serializedProperty.arraySize;
		list.serializedProperty.arraySize++;

		var element = list.serializedProperty.GetArrayElementAtIndex(index);
		element.objectReferenceValue = obj;
		
		serializedObject.ApplyModifiedProperties();
	}


	void OnRemove (ReorderableList list)
	{
		var index = list.index;
		var element = list.serializedProperty.GetArrayElementAtIndex(index);
		var obj = (A)element.objectReferenceValue;

		ReorderableList.defaultBehaviours.DoRemoveButton(list);

		ArrayUtility.Remove (ref arr, obj);
		ScriptableObjectEditorUtil.DeleteObjectFromAsset (obj);
		serializedObject.ApplyModifiedProperties ();
	}
}
