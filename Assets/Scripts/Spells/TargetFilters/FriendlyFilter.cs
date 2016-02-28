using UnityEngine;
using System.Collections;


namespace Spells {
	[CustomScriptableObject("Friendly", "Only target objects that are friendly to caster.")]
	public class FriendlyFilter : SpellTargetFilter {
		public override bool IsValidTarget(SpellTargetCollection targets, GameObject target) {
			return !FactionManager.AreHostile(target, targets.ContextOwner);
		}
	}
}
