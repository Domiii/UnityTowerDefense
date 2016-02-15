using UnityEngine;
using System.Collections;

namespace Spells {
	[CustomScriptableObjectAttribute("Damage")]
	public class DamageEffect : SpellEffect {
		public int DamageMin = 10;
		public int DamageMax = 20;

		public override void Apply(SpellPhaseContext context, GameObject target) {
			var unit = target.GetComponent<Unit> ();
			if (unit != null) {
				var damageInfo = ObjectManager.Instance.Obtain<DamageInfo>();
				damageInfo.Value = Random.Range(DamageMin, DamageMax);
				damageInfo.SourceFactionType = FactionManager.GetFactionType(context.ContextOwner);
				unit.Damage(damageInfo);
			}
		}
	}
}