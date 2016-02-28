using UnityEngine;
using System.Collections;

namespace Spells {
	[System.Serializable]
	public class AuraTemplate : ScriptableObject {
		/// <summary>
		/// Time in seconds
		/// </summary>
		public float Duration;
		
		/// <summary>
		/// AuraEffects to be applied to the PhaseObject during the phase
		/// </summary>
		public AuraEffectCollection AuraEffects;
		
		public float RepeatDelay;
		public int MaxRepetitions = 0;
		public GameObject PhaseObjectPrefab;
	}

}