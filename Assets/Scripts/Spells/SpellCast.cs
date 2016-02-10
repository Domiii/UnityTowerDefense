using UnityEngine;
using System.Collections;

namespace Spells {
	public class SpellCast : MonoBehaviour {
		public SpellCaster SpellCaster;
		public Spell Spell;
		public SpellPhase CurrentPhase;

		public void NextPhase() {
			// TODO: Next spell casting phase
		}

		void StartCastPhase() {
		}

		void StartProjectilePhase() {
		}

		void Impact() {
			// TODO: When do we collect impact targets?
		}

		public void CleanUp() {
			// TODO: Clean up?
		}
	}

}