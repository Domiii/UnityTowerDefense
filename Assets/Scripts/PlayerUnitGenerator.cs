using UnityEngine;
using System.Collections;

public class PlayerUnitGenerator : MonoBehaviour {
	public UnitPurchaseOption[] PurchaseOptions;
	public WavePath Path;
	public WavePath.FollowDirection Direction;

	// Use this for initialization
	void Start () {  }
	
	// Update is called once per frame
	void Update () {  }

	public bool CanPurchase(UnitPurchaseOption purchaseChoice) {
		return purchaseChoice.IsReady && purchaseChoice.HasSufficientFunds;
	}

	public void PurchaseUnit(int unitIndex) {
		TryPurchaseUnit (unitIndex);
	}

	public bool TryPurchaseUnit(int unitIndex) {
		if (unitIndex < 0 || unitIndex > PurchaseOptions.Length) {
			// invalid index
			return false;
		}

		// select prefab
		var purchaseChoice = PurchaseOptions [unitIndex];
		if (CanPurchase (purchaseChoice)) {
			GameManager.Instance.DeductCredits(purchaseChoice.CreditCost);
			purchaseChoice.LastPurchaseTime = Time.time;
			ProduceUnit(purchaseChoice);
			return true;
		}
		return false;
	}
	
	void ProduceUnit(UnitPurchaseOption purchaseChoice) {
		var unitPrefab = purchaseChoice.UnitPrefab;

		// create new unit at current position
		var go = (GameObject)Instantiate(unitPrefab, transform.position, Quaternion.identity);
		
		// set path
		var pathFollower = go.GetComponent<PathFollower>();
		pathFollower.Path = Path;
		pathFollower.PathDirection = Direction;
		
		// add faction
		FactionMember.SetFaction (go, FactionType.Player);
	}
}
