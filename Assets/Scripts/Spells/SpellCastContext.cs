using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Spells {
	public class SpellCastContext : ISpellObject {
		public Spell Spell;
		public GameObject Caster;
		public Transform InitialTarget;
		public Vector3 InitialTargetPosition;

		Dictionary<string, object> spellCastData;

		public SpellCastContext() {
		}
		
		public Dictionary<string, object> SpellCastData {
			get {
				if (spellCastData == null) {
					spellCastData = new Dictionary<string, object> ();
				}
				return spellCastData;
			}
			set {
				spellCastData = value;
			}
		}
	}
}