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

		public void Recycle<T>(T obj)
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
		
		public GameObject ObtainEmpty(string name, Vector3 position, Quaternion rotation) {
			// TODO: Implement GO recycling
			var go = new GameObject (name);
			go.transform.position = position;
			go.transform.rotation = rotation;
			return go;
		}
		
		public void Recycle(GameObject obj) {
			// TODO: Implement GO recycling (use special pool for empty's)
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

	#region Pool Implementation (stub)
	/*
	internal class PoolState {
		public System.Object Value;
		public int Count;
		public ObjectPool Pool;
	}
	
	public struct PooledObject<T> {
		PooledObject(PooledObject<T> ptr) {
			State = ptr.State;
			++State.Count;
		}
		
		internal void Init(PoolState state) {
			State = state;
			State.Value = value;
			State.Count = 0;
		}
		
		PoolState State {
			get;
			set;
		}
		
		public T Value {
			get { return State.Value; }
		}
		
		public static implicit operator PooledObject<T>(T value) {
			// new reference
			return ObjectPool.DefaultObjectPool.Obtain(value);
		}
		
		public static explicit operator PooledObject<T>(PooledObject<T> ptr) {
			// increase reference count
			return new PooledObject<T> (ptr);
		}
	}
	
	public class ObjectPool {
		public static readonly DefaultObjectPool = new ObjectPool();

		class ObjectSet  {
			public List<PoolState> List;

			public ObjectSet() {
				List = new List<PoolState>();
			}

			int GetNextFreeIndex() {
				// TODO
			}

			public PoolState GetOrAllocateNextFreeState() {
				// TODO
			}
		}
		
		Dictionary<System.Type, ObjectSet> objects;
		
		public ObjectPool() {
			objects = new Dictionary<System.Type, ObjectSet>();
		}
		
		internal PooledObject<T> Obtain<T>(T newValue)
			where T : new()
		{
			// TODO: Make this more efficient for a multi-threaded environment
			PoolState state;
			
			lock (objects) {
				ObjectSet set;
				if (!objects.TryGetValue(typeof(T), out set)) {
					objects.Add(typeof(T), set = new ObjectSet());
				}
				state = set.GetOrAllocateNextFreeState();
			}
			
			var obj = new PooledObject<T>();
			state.Value = newValue;
			obj.Init(state);
			return obj;
		}
	}
	*/
	#endregion
}
