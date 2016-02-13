using UnityEngine;
using System.Collections.Generic;

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
		float lastCastTime;

		public bool IsCasting {
			get {
				return SpellCast != null && SpellCast.IsCasting;
			}
		}

		/// <summary>
		/// Caster can cast spell when not on cooldown
		/// </summary>
		public bool CanCastSpell {
			get {
				var dt = Time.time - lastCastTime;
				return dt >= Spell.Cooldown;
			}
		}

		public void Interrupt() {
			// interrupt current spell cast (if any)
			if (SpellCast != null) {
				SpellCast.Interrupt();
			}
		}
		
		public bool TryCastSpell(GameObject initialTarget, ref Vector3 initialTargetPosition) {
			return TryCastSpell (Spell, initialTarget, ref initialTargetPosition);
		}
		
		public bool TryCastSpell(Spell spell, GameObject initialTarget, ref Vector3 initialTargetPosition) {
			SpellCast = SpellGameObjectManager.Instance.AddComponent<SpellCast>(gameObject);
			return SpellCast.StartCasting (gameObject, spell, initialTarget, ref initialTargetPosition);
		}

		void Start() {

		}

		void Update() {
			if (SpellCast != null && SpellCast.Status != SpellCastStatus.Casting && SpellCast.Status != SpellCastStatus.Failed) {
				// casting phase is over and spell was not interrupted -> Reset cast time
				OnCastFinished();
			}
		}
		
		
		void OnCastFinished() {
			SpellCast = null;
			lastCastTime = Time.time;
		}

	}

}