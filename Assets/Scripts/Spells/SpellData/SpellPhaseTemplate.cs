using UnityEngine;
using System.Collections;

namespace Spells {
	public enum SpellPhaseId {
		CastPhase = 1,
		ProjectilePhase = 2,
		ImpactPhase = 3
	}

	[System.Serializable]
	public abstract class SpellPhaseTemplate : ScriptableObject {
		/// <summary>
		/// Time in seconds
		/// </summary>
		public float Duration = 2;
		public GameObject PhaseObjectPrefab;

		public SpellEffectCollection StartEffects;
		public SpellEffectCollection RepeatEffects;
		public SpellEffectCollection EndEffects;

		/// <summary>
		/// AuraEffects to be applied to the PhaseObject during the phase
		/// </summary>
		public AuraTemplate AuraTemplate;

		public float RepeatDelay;
		public int MaxRepetitions = 0;

		public abstract SpellPhaseId SpellPhaseId {
			get;
		}

		public override string ToString ()
		{
			return SpellPhaseId + " Template";
		}
	}

}