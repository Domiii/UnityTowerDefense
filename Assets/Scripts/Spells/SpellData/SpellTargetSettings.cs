﻿using UnityEngine;
using System.Collections;
using System.Linq;

namespace Spells {
	
	// CastPhase (while casting; start position = caster)
	// ProjectilePhase (while projectile in flight; start position = caster)
	// Impact (after projectile hit; start position = target)

	[System.Serializable]
	public class SpellTargetSettings {
		public float Range = 3;

		// TODO: RandomUnitInRange, UnitsInChain, UnitsInCollider
		public SpellTargetCollector[] TargetCollectors = new SpellTargetCollector[0];

		
		// TODO: InFront (angle), Wounded
		public SpellTargetFilter[] TargetFilters = new SpellTargetFilter[0];

		//public SimplifiedSerializableObject SerializableForm;
		
//		public bool HasObjectTargets {
//			get {
//				for (int i = 0; i < TargetSelectors.Length; ++i) {
//					var selector = TargetSelectors[i];
//					if (selector.HasObjectTargets) return true;
//				}
//				return false;
//			}
//		}
//		
//		public bool HasPositionTarget {
//			get {
//				for (int i = 0; i < TargetSelectors.Length; ++i) {
//					var selector = TargetSelectors[i];
//					if (selector.HasPositionTarget) return true;
//				}
//				return false;
//			}
//		}
	}

}