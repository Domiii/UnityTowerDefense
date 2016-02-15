using UnityEngine;
using System.Collections;


namespace Spells {
	[CustomScriptableObject("CasterSelectedPosition")]
	[System.Serializable]
	public class CasterSelectedPositionCollector : SpellTargetCollector {
		public override bool HasObjectTargets {
			get { return false; }
		}
		
		public override bool HasPositionTarget {
			get { return true; }
		}
		
		public override void CollectTargets(SpellTargetCollection targets) {
			targets.TargetPosition = targets.SpellCastContext.InitialTargetPosition;
		}
	}
}