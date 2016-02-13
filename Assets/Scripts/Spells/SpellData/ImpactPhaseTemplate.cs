using UnityEngine;
using System.Collections;

namespace Spells {
	[System.Serializable]
	public class ImpactPhaseTemplate : SpellPhaseTemplate {
		public override SpellPhaseId SpellPhaseId {
			get {
				return SpellPhaseId.ImpactPhase;
			}
		}
	}

}