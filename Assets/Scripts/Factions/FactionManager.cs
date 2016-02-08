using UnityEngine;
using System.Collections;
using System.Linq;

public class FactionManager : MonoBehaviour {
	#region Static Methods
	static FactionManager[] _factionManagers;
	
	public static FactionManager GetFactionManager(FactionType factionType) {
		return _factionManagers [(int)factionType];
	}

	public static bool AreHostile(GameObject obj1, GameObject obj2) {
		return GetFaction(obj1) != GetFaction(obj2);
	}

	public static FactionType GetFaction(GameObject obj) {
		var factionMember = obj.GetComponent<FactionMember> ();
		return factionMember != null ? factionMember.FactionType : default(FactionType);
	}
	
	public static void SetFaction(GameObject dest, GameObject src) {
		SetFaction(dest, FactionManager.GetFaction (src));
	}

	public static void SetFaction(GameObject dest, FactionType type) {
		// make sure, projectile has faction
		if (dest.GetComponent<FactionMember> () == null) {
			dest.AddComponent (typeof(FactionMember));
		}
		dest.GetComponent<FactionMember>().FactionType = type;
	}

	static FactionManager() {
		var maxFactionTypeValue = (int)System.Enum.GetValues(typeof(FactionType)).Cast<FactionType>().Max();
		
		_factionManagers = new FactionManager[maxFactionTypeValue+1];
	}
	#endregion
	
	
	public UnitPurchaseOption[] PurchaseOptions;
	public Transform SpawnPoint;
	public WavePath Path;
	public WavePath.FollowDirection Direction;
	FactionType _factionType;

	protected FactionManager(FactionType factionType) {
		Debug.Assert (_factionManagers [(int)factionType] == null, "FactionManager registered twice: " + factionType);

		_factionManagers [(int)factionType] = this;
		_factionType = factionType;
	}

	
	// Use this for initialization
	void Start () { 
		if (SpawnPoint == null || Path == null || PurchaseOptions == null) {
			Debug.LogErrorFormat(this, "Invalid {0}FactionManager is missing SpawnPoint, Path or PurchaseOptions.", _factionType);
			return;
		}
	}
	
	// Update is called once per frame
	void Update () {  }


	#region Unit Purchasing
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
			PurchaseUnit(purchaseChoice);
			return true;
		}
		return false;
	}
	
	public bool TryPurchaseUnit(UnitPurchaseOption purchaseChoice) {
		if (CanPurchase (purchaseChoice)) {
			PurchaseUnit(purchaseChoice);
			return true;
		}
		return false;
	}
		
	public void PurchaseUnit(UnitPurchaseOption purchaseChoice) {
		GameManager.Instance.DeductCredits(purchaseChoice.CreditCost);
		purchaseChoice.LastPurchaseTime = Time.time;
		ProduceUnit(purchaseChoice);
	}
	
	void ProduceUnit(UnitPurchaseOption purchaseChoice) {
		var unitPrefab = purchaseChoice.UnitPrefab;
		
		// create new unit at current position
		var go = (GameObject)Instantiate(unitPrefab, SpawnPoint.position, Quaternion.identity);
		
		// set path
		var pathFollower = go.GetComponent<PathFollower>();
		pathFollower.Path = Path;
		pathFollower.PathDirection = Direction;
		
		// add faction
		FactionManager.SetFaction (go, FactionType.Player);
	}
	#endregion
}
