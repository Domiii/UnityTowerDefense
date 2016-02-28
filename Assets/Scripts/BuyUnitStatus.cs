using UnityEngine;
using System.Collections;


public class BuyUnitStatus {
	float lastBuyTime;
	UnitManager unitManager;

	public BuyUnitConfig Config {
		get;
		private set;
	}

	public BuyUnitStatus(UnitManager unitManager, BuyUnitConfig cfg) {
		lastBuyTime = Time.time;
		this.unitManager = unitManager;
		Config = cfg;
	}
	
	public float SecondsSinceLastBuy {
		get { return Time.time - lastBuyTime; }
	}

	public bool IsReady {
		get { return SecondsSinceLastBuy >= Config.CooldownSeconds; }
	}
	
	public bool HasSufficientFunds {
		get { 
			if (unitManager.Faction != null) {
				return unitManager.Faction.Credits >= Config.CreditCost;
			}
			return true;
		}
	}
	
	public bool CanBuy {
		get { return IsReady && HasSufficientFunds; }
	}
	
	public bool TryBuyUnit() {
		if (CanBuy) {
			BuyUnit();
			return true;
		}
		return false;
	}
	
	public void BuyUnit() {
		lastBuyTime = Time.time;
		if (unitManager.Faction != null) {
			unitManager.Faction.DeductCredits (Config.CreditCost);
		}
		unitManager.ProduceUnit(Config);
	}
}
