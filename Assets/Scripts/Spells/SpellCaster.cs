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
		public bool IsSpellReady {
			get {
				var dt = Time.time - lastCastTime;
				return dt >= Spell.Cooldown;
			}
		}

		public void Interrupt() {
			// interrupt current spell cast (if any)
			if (IsCasting) {
				SpellCast.Interrupt();
			}
		}
		
		public bool TryPrepareSpellCast(GameObject initialTarget) {
			InitializeSpellCast (Spell);
			return SpellCast.CanCastSpell (initialTarget, initialTarget.transform.position);
		}
		
		public bool TryCastSpell(GameObject initialTarget) {
			return TryCastSpell (Spell, initialTarget, initialTarget.transform.position);
		}
		
		public bool TryCastSpell(Spell spell, GameObject initialTarget) {
			return TryCastSpell (spell, initialTarget, initialTarget.transform.position);
		}
		
		public bool TryCastSpell(GameObject initialTarget, Vector3 initialTargetPosition) {
			return TryCastSpell (Spell, initialTarget, initialTargetPosition);
		}
		
		public bool TryCastSpell(Spell spell, GameObject initialTarget, Vector3 initialTargetPosition) {
			if (TryPrepareSpellCast (initialTarget)) {
				SpellCast.StartCasting (initialTarget, initialTargetPosition);
				return true;
			}
			return false;
		}
		
		void InitializeSpellCast(Spell spell) {
			// interrupt if already casting
			Interrupt();
			
			if (SpellCast == null || SpellCast.Status != SpellCastStatus.Unused) {
				SpellCast = GameObjectManager.Instance.AddComponent<SpellCast> (gameObject);
			}
			if (spell != SpellCast.SpellCastContext.Spell) {
				SpellCast.Initialize (spell);
			}
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