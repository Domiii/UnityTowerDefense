using UnityEngine;
using System.Collections;

namespace Spells {
	[System.Serializable]
	public class ProjectilePhaseTemplate : SpellPhaseTemplate {
		// TODO: Use more special components for projectile targeting
		public float Speed = 10.0f;
		//public bool FollowsTarget;

		public override SpellPhaseId SpellPhaseId {
			get {
				return SpellPhaseId.ProjectilePhase;
			}
		}
	}

}