using UnityEngine;
using System.Collections;


namespace Spells {
	[CustomScriptableObject("ContextOwner")]
	[System.Serializable]
	public class ContextOwnerCollector : SpellTargetCollector {
		public override bool HasObjectTargets {
			get { return true; }
		}
		
		public override bool HasPositionTarget {
			get { return false; }
		}
		
		public override void CollectTargets(SpellTargetCollection targets) {
			targets.AddTarget (targets.ContextOwner);
		}
	}
}