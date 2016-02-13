using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Spells {
	#region SpellTargetSelector + SpellTargetFilter
	[System.Serializable]
	public abstract class SpellTargetCollector {
		public abstract bool HasObjectTargets {
			get;
		}
		
		public abstract bool HasPositionTarget {
			get;
		}
		
		public abstract void CollectTargets(SpellTargetCollection targets);
	}
	
	[System.Serializable]
	public abstract class SpellTargetFilter {
		public abstract bool IsValidTarget (SpellTargetCollection targets, GameObject target);
	}
	#endregion


	public class SpellTargetCollection : IPooledObject {
		public static readonly int MaxTargetsInRadius = 128;

		List<GameObject> list;
		Collider2D[] colliders;
		
		public SpellTargetCollection() {
			list = new List<GameObject> ();
		}
		
		public SpellTargetSettings Settings {
			get;
			private set;
		}
		
		public SpellCastContext SpellCastContext {
			get;
			private set;
		}
		
		public GameObject ContextOwner {
			get;
			private set;
		}

		public Vector3? TargetPosition {
			get;
			set;
		}
		
		public int Count
		{
			get { return list.Count; }
		}
		
		public GameObject this [int index] {
			get {
				return list[index];
			}
			set {
				list[index] = value;
			}
		}

		public void InitializeTargets(GameObject contextOwner, SpellCastContext spellCastContext) {
			ContextOwner = contextOwner;
			SpellCastContext = spellCastContext;

		}
		
		#region Manage Target List
		public void AddTarget(GameObject go) {
			if (!list.Contains (go)) {
				list.Add (go);
			}
		}
		
		public void RemoveTarget(GameObject go) {
			// TODO: Don't de-allocate
			list.Remove (go);
		}
		#endregion

		#region Finding, Collecting + Filtering
		/// <summary>
		/// </summary>
		/// <returns><c>true</c>, if targets were found, <c>false</c> otherwise.</returns>
		public void FindTargets(SpellTargetSettings settings) {
			if (ContextOwner == null) {
				return;
			}

			Clear ();

			Settings = settings;

			CollectTargets ();
			FilterTargets ();
		}
		
		public int AddTargetsInRadius2D(float radius, out Collider2D[] collidersInRange) {
			return AddTargetsInRadius2D (ContextOwner.transform.position, radius, out collidersInRange);
		}
		
		public int AddTargetsInRadius2D(Vector3 position, float radius, out Collider2D[] collidersInRange) {
			if (colliders == null) {
				colliders = new Collider2D[MaxTargetsInRadius];
			}
			collidersInRange = colliders;
			return Physics2D.OverlapCircleNonAlloc(position, radius, collidersInRange);
		}
		
		public bool IsValidTarget(GameObject target) {
			// we are currently only allowing Unit targets
			if (target.GetComponent<Unit> () == null) {
				return false;
			}

			// apply all other filters
			for (var i = 0; i < Settings.TargetFilters.Length; ++i) {
				var filter = Settings.TargetFilters[i];
				if (!filter.IsValidTarget(this, target)) {
					return false;
				}
			}
			return true;
		}

		void CollectTargets() {
			for (int i = 0; i < Settings.TargetCollectors.Length; i++) {
				var collector = Settings.TargetCollectors[i];
				collector.CollectTargets(this);
			}
		}

		void FilterTargets() {
			for (var i = Count-1; i >= 0; --i) {
				var target = this [i];
				if (!IsValidTarget(target)) {
					RemoveTarget (target);
				}
			}
		}
		#endregion

		#region Clear + CleanUp
		public void Clear () {
			// TODO: Don't de-allocate
			list.Clear ();
			TargetPosition = null;
		}
		
		public void CleanUp() {
			Clear();
		}
		#endregion
	}

}