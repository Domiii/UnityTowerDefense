using UnityEngine;
using System.Collections;

namespace Spells {
	public enum SpellPhaseId {
		CastPhase = 1,
		ProjectilePhase = 2,
		ImpactPhase = 3
	}

	[System.Serializable]
	public class SpellPhaseTemplate {
		public SpellEffectCollection StartEffects;
		public SpellEffectCollection RepeatEffects;
		public SpellEffectCollection EndEffects;

		/// <summary>
		/// AuraEffects to be applied to the PhaseObject during the phase
		/// </summary>
		public AuraTemplate AuraTemplate;

		public float RepeatDelay;
		public int MaxRepetitions = 0;
		public GameObject PhaseObjectPrefab;
	}

}