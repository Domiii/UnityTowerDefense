using UnityEngine;
using System.Collections;


namespace Spells {
	[CustomScriptableObject("Hostile", "Only target objects that are hostile to caster.")]
	public class HostileTargetFilter : SpellTargetFilter {
		public override bool IsValidTarget(SpellTargetCollection targets, GameObject target) {
			return FactionManager.AreHostile(target, targets.ContextOwner);
		}
	}
}
