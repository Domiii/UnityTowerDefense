﻿using UnityEngine;
using System.Collections;


namespace Spells {
	[CustomScriptableObject("CasterSelectedUnit")]
	[System.Serializable]
	public class CasterSelectedUnitCollector : SpellTargetCollector {
		public override bool HasObjectTargets {
			get { return true; }
		}
		
		public override bool HasPositionTarget {
			get { return false; }
		}
		
		public override void CollectTargets(SpellTargetCollection targets) {
			if (targets.SpellCastContext.InitialTarget != null) {
				targets.AddTarget (targets.SpellCastContext.InitialTarget);
			}
		}
	}
}