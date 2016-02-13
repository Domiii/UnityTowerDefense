using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Spells {
	[System.Serializable]
	public class AuraEffectCollection : ScriptableObject, IEnumerable<AuraEffect> {
		public AuraEffect[] Effects;

		public IEnumerator<AuraEffect> GetEnumerator() {
			return Effects.AsEnumerable().GetEnumerator();
		}
		
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return Effects.GetEnumerator();
		}
	}
}