using UnityEngine;
using System.Collections;

namespace Spells {
	public class SpellCaster : MonoBehaviour {
		public Spell Spell;

		public SpellCast SpellCast;

		/// <summary>
		/// How often this spell caster can cast the spell.
		/// </summary>
		public int Charges;

		/// <summary>
		/// Set when cast phase finished
		/// </summary>
		float _lastCastTime;

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
		
		public bool TryCastSpell(Transform initialTarget, ref Vector3 initialTargetPosition) {
			return TryCastSpell (Spell, initialTarget, ref initialTargetPosition);
		}
		
		public bool TryCastSpell(Spell spell, Transform initialTarget, ref Vector3 initialTargetPosition) {
			SpellCast = SpellGameObjectManager.Instance.AddComponent<SpellCast> (gameObject);
			return SpellCast.StartCasting (gameObject, spell, initialTarget, ref initialTargetPosition);
		}

		void Update() {
			if (SpellCast != null && !SpellCast.IsCasting) {
				// finished casting
				SpellCast = null;
				_lastCastTime = Time.time;
			}
		}
	}

}