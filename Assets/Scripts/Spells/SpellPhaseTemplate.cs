using UnityEngine;
using System.Collections;

namespace Spells {
	[System.Serializable]
	public class SpellPhaseTemplate {
		public SpellEffectCollection StartEffects;
		public SpellEffectCollection RepeatEffects;
		public SpellEffectCollection EndEffects;

		public float RepeatDelay;
		public int MaxRepetitions = 0;
		public GameObject SpellPhasePrefab;
	}

}