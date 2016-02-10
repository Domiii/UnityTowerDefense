using UnityEngine;
using System.Collections;

namespace Spells {
	public class SpellCaster : MonoBehaviour {
		public Spell Spell;

		/// <summary>
		/// How often this spell caster can cast the spell.
		/// </summary>
		public int Charges;

		/// <summary>
		/// Set when cast phase finished
		/// </summary>
		float _lastCastTime;

		public SpellCast CurrentSpellCast {
			get;
			private set;
		}

		/// <summary>
		/// Caster can cast spell when not on cooldown
		/// </summary>
		public bool CanCastSpell {
			get {
				var dt = Time.time - _lastCastTime;
				return dt >= Spell.Cooldown;
			}
		}

		public void Interrupt() {
			// TODO: interrupt current spell cast (if any)
		}
		
		public bool TryCastSpell() {
			return TryCastSpell (Spell);
		}
		
		public bool TryCastSpell(Spell spell) {
			// TODO: Cast spell if possible
			return false;
		}
	}

}