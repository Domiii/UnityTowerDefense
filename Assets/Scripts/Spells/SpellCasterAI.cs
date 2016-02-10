using UnityEngine;
using System.Collections;

namespace Spells {
	[RequireComponent(typeof(SpellCaster))]
	public class SpellCasterAI : MonoBehaviour {
		public SpellCaster SpellCaster {
			get {
				return GetComponent<SpellCaster>();
			}
		}
	}

}