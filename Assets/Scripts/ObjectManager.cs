using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Simple object pool here: https://unity3d.com/learn/tutorials/modules/beginner/live-training-archive/object-pooling

/// <summary>
/// Interface implemented by relatively short-lived objects (usually seconds) that can be pooled
/// </summary>
public interface IPooledObject {
}

/// <summary>
/// All run-time instantiations of IPooledObject
/// are managed by the ObjectManager
/// </summary>
public class ObjectManager : MonoBehaviour {
	public static readonly ObjectManager Instance;

	static ObjectManager() {
		Instance = new ObjectManager ();
	}

	public ObjectManager() {
	}

	public T Obtain<T>() where
		T : IPooledObject, new()
	{
		// TODO: Implement object recycling
		return new T ();
	}

	public void Recycle<T>(T obj)
		where T : IPooledObject
	{
		// TODO: Implement object recycling
	}
}


/// <summary>
/// Run-time instantiations of GameObjects
/// should be managed by the GameObjectManager
/// </summary>
public class GameObjectManager : MonoBehaviour {
	public static readonly GameObjectManager Instance;
	
	static GameObjectManager() {
		Instance = new GameObjectManager ();
	}
	
	public GameObjectManager() {
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
		where T : MonoBehaviour, IPooledObject
	{
		return go.AddComponent<T> ();
	}
	
	public void RemoveComponent<T>(T component)
		where T : MonoBehaviour, IPooledObject
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
