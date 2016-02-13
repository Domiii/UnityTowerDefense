using UnityEngine;
using System.Collections;

namespace Spells {
	[System.Serializable]
	public class CastPhaseTemplate : SpellPhaseTemplate {
		public override SpellPhaseId SpellPhaseId {
			get {
				return SpellPhaseId.CastPhase;
			}
		}
	}

}