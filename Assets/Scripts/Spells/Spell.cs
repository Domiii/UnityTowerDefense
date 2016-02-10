using UnityEngine;
using System.Collections;

namespace Spells {
	[System.Serializable]
	public class Spell {
		public float Cooldown;

		public CastPhaseTemplate CastPhase;
		public ProjectilePhaseTemplate ProjectilePhase;
		
		public SpellEffectCollection ImpactEffects;
		public GameObject ImpactPrefab;
	}

}