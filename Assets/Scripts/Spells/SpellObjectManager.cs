using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Spells {
	/// <summary>
	/// Interface implemented by all short-lived objects used in the spell system
	/// </summary>
	public interface ISpellObject {
	}

	/// <summary>
	/// All run-time instantiations of ISpellObjects
	/// are managed by the SpellObjectManager
	/// </summary>
	public class SpellObjectManager : MonoBehaviour {
		public static SpellObjectManager Instance;

		public SpellObjectManager() {
			Instance = this;
		}

		public T Obtain<T>() where
			T : ISpellObject, new()
		{
			// TODO: Implement object recycling
			return new T ();
		}

		public void Recycle<T>()
			where T : ISpellObject
		{
			// TODO: Implement object recycling
		}
	}


	/// <summary>
	/// All run-time instantiations of spell-related GameObjects
	/// are managed by the SpellGameObjectManager
	/// </summary>
	public class SpellGameObjectManager : MonoBehaviour {
		public static SpellGameObjectManager Instance;
		
		public SpellGameObjectManager() {
			Instance = this;
		}

		public GameObject Obtain(GameObject prefab, Vector3 position, Quaternion rotation) {
			// TODO: Implement GO recycling
			var go = (GameObject)Instantiate(prefab, position, rotation);
			return go;
		}
		
		public void Recycle(GameObject obj) {
			// TODO: Implement GO recycling
			GameObject.Destroy (obj);
		}

		public T AddComponent<T>(GameObject go)
			where T : MonoBehaviour, ISpellObject
		{
			return go.AddComponent<T> ();
		}
		
		public void RemoveComponent<T>(T component)
			where T : MonoBehaviour, ISpellObject
		{
			Destroy (component);
		}
	}
}
