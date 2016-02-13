using UnityEngine;
using System.Collections;
using System.Linq;

namespace Spells {
	[System.Serializable]
	public class SpellTargetSettings : ScriptableObject {

		// CastPhase (while casting; start position = caster)
		// ProjectilePhase (while projectile in flight; start position = caster)
		// Impact (after projectile hit; start position = target)
		public float MinRange, MaxRange;

		public SpellTargetSelector[] TargetSelectors = new SpellTargetSelector[0];
		// public Type Filter; // e.g. InFront (angle), Wounded, Hostile, Friendly
		
		public bool HasObjectTargets {
			get {
				for (int i = 0; i < TargetSelectors.Length; ++i) {
					var selector = TargetSelectors[i];
					if (selector.HasObjectTargets) return true;
				}
				return false;
			}
		}
		
		public bool HasPositionTarget {
			get {
				for (int i = 0; i < TargetSelectors.Length; ++i) {
					var selector = TargetSelectors[i];
					if (selector.HasPositionTarget) return true;
				}
				return false;
			}
		}
	}

}