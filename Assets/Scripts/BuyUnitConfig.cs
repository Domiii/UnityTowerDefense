using UnityEngine;
using System.Collections;

[System.Serializable]
public class BuyUnitConfig {
	public bool Enabled = true;
	public int CreditCost;
	public GameObject UnitPrefab;
	public float CooldownSeconds;
	public Sprite PreviewIcon;
}
