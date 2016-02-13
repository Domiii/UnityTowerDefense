using UnityEngine;
using System.Collections;


namespace Spells {
	// TODO: InFront (angle), Wounded

	[System.Serializable]
	[BehaviorScript("Hostile", "Only target objects that are hostile to caster.")]
	public class HostileTargetFilter : SpellTargetFilter {
		public override bool IsValidTarget(SpellTargetCollection targets, GameObject target) {
			return FactionManager.AreHostile(target, targets.ContextOwner);
		}
	}
	
	[System.Serializable]
	[BehaviorScript("Friendly", "Only target objects that are friendly to caster.")]
	public class FriendlyTargetFilter : SpellTargetFilter {
		public override bool IsValidTarget(SpellTargetCollection targets, GameObject target) {
			return !FactionManager.AreHostile(target, targets.ContextOwner);
		}
	}
}
