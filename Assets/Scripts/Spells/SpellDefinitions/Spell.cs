using UnityEngine;
using System.Collections;

namespace Spells {
	[System.Serializable]
	public class Spell {
		public string Name = "new spell";

		public float Cooldown;

		public CastPhaseTemplate CastPhase;
		public ProjectilePhaseTemplate ProjectilePhase;
		public ImpactPhaseTemplate ImpactPhase;
	}

}