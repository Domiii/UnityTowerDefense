using UnityEngine;
using System.Collections;


[System.Serializable]
public class UnitPurchaseOption {
	public int CreditCost;
	public float CooldownSeconds;
	public GameObject UnitPrefab;

	internal float LastPurchaseTime;

	public float SecondsSinceLastPurchase {
		get { return Time.time - LastPurchaseTime; }
	}

	public bool IsReady {
		get { return SecondsSinceLastPurchase >= CooldownSeconds; }
	}

	public bool HasSufficientFunds {
		get { return GameManager.Instance.Credits >= CreditCost; }
	}
}
