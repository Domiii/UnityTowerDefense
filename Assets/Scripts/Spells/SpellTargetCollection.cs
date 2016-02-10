using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Spells {
	public class SpellTargetCollection {
		SpellTargetConfig config;
		List<GameObject> targets;
		
		public Transform TargetTransform;
		public Vector3 TargetPoint;

		public SpellTargetCollection(SpellTargetConfig config) : this() {
			this.config = config;
		}
		
		
		public SpellTargetCollection() {
		}
		
		/// <summary>
		/// </summary>
		/// <returns><c>true</c>, if targets were found, <c>false</c> otherwise.</returns>
		public bool FindTargets(SpellTargetConfig config) {
			this.config = config;
			return FindTargets ();
		}

		/// <summary>
		/// </summary>
		/// <returns><c>true</c>, if targets were found, <c>false</c> otherwise.</returns>
		public bool FindTargets() {
			targets = new List<GameObject> ();

			return true;
		}
	}

}