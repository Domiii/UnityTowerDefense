using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HasLoot : MonoBehaviour {
	public int MinLootCredits, MaxLootCredits;
	private Text lootText;

	void OnDeath(DamageInfo damageInfo) {
		var faction = FactionManager.GetFaction(damageInfo.SourceFactionType);
		if (faction != null) {
			// give credits to killer
			var lootCredits = Random.Range (MinLootCredits, MaxLootCredits);
			faction.GainCredits (lootCredits, transform.position);
		}
	}
}
