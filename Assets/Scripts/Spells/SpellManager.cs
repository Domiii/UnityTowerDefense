using UnityEngine;
using System.Collections.Generic;

namespace Spells {
	public class SpellManager : MonoBehaviour {
		public static SpellManager Instance {
			get;
			private set;
		}

		public GameObject DefaultPhasePrefab;

		public SpellManager() {
			Instance = this;
		}


	}
}