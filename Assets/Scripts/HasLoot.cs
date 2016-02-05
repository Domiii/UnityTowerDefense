using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HasLoot : MonoBehaviour {
	public int MinLootCredits, MaxLootCredits;
	private Text _lootText;

	void OnDeath(DamageInfo damageInfo) {
		if (damageInfo.SourceFaction == FactionType.Player) {
			// give credits to player
			var lootCredits = Random.Range (MinLootCredits, MaxLootCredits);
			GameManager.Instance.GainCredits (lootCredits, transform.position);
		}
	}
}
