using UnityEngine;
using System.Collections;

public class FactionMember : MonoBehaviour {
	public static bool AreHostile(GameObject obj1, GameObject obj2) {
		return GetFaction(obj1) != GetFaction(obj2);
	}

	public static FactionType GetFaction(GameObject obj) {
		var factionMember = obj.GetComponent<FactionMember> ();
		return factionMember != null ? factionMember.FactionType : default(FactionType);
	}
	
	public static void SetFaction(GameObject dest, GameObject src) {
		SetFaction(dest, FactionMember.GetFaction (src));
	}

	public static void SetFaction(GameObject dest, FactionType type) {
		// make sure, projectile has faction
		if (dest.GetComponent<FactionMember> () == null) {
			dest.AddComponent (typeof(FactionMember));
		}
		dest.GetComponent<FactionMember>().FactionType = type;
	}

	public FactionType FactionType;
}
