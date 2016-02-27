using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuyUnitButton : MonoBehaviour {
	public BuyUnitMenu Menu;
	public int UnitStatusIndex;

	bool _isShowingCooldown;
	Transform _cooldownOverlay;
	Transform _disabledOverlay;
	SpriteRenderer _progressBar;
	Text _costText;
	
	public BuyUnitStatus Status {
		get {
			if (Menu.UnitManager) {
				return Menu.UnitManager.GetStatus(UnitStatusIndex);
			}
			return null;
		}
	}

	// Use this for initialization
	public void Start () {
		_isShowingCooldown = true;

		_cooldownOverlay = transform.FindFirstDescendantByName("CooldownOverlay");
		if (_cooldownOverlay == null) {
			Debug.LogError("BuyUnitButton is missing child \"CooldownOverlay\"", this);
			return;
		}
		
		_disabledOverlay = transform.FindFirstDescendantByName("DisabledOverlay");
		if (_cooldownOverlay == null) {
			Debug.LogError("BuyUnitButton is missing child \"DisabledOverlay\"", this);
			return;
		}

		_progressBar = _cooldownOverlay.FindFirstDescendantByName<SpriteRenderer>("CooldownProgress");
		if (_progressBar == null) {
			Debug.LogError("BuyUnitButton is missing SpriteRenderer child \"CooldownProgress\"", this);
			return;
		}

		_costText = transform.FindFirstDescendantByName<Text> ("CostText");
		if (_costText == null) {
			Debug.LogError("BuyUnitButton is missing Text child \"CostText\"", this);
			return;
		}

		_costText.text = Status.Config.CreditCost.ToString ();

		// sync cooldown overlay status initially
		ToggleCooldownOverlay ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Status == null) {
			return;
		}

		Debug.Assert (_cooldownOverlay != null && _progressBar != null);

		_disabledOverlay.gameObject.SetActive (!Status.CanBuy);

		var isReady = Status.IsReady;
		if (isReady == _isShowingCooldown) {
			ToggleCooldownOverlay();
		}
		if (!isReady) {
			UpdateCooldownProgress();
		}
	}

	void ToggleCooldownOverlay() {
		_cooldownOverlay.gameObject.SetActive(_isShowingCooldown = !Status.IsReady);
	}

	void UpdateCooldownProgress() {
		var progress = Status.SecondsSinceLastBuy / Status.Config.CooldownSeconds;
		_progressBar.transform.localScale = new Vector2(progress, 1);
	}

	
	void OnMouseDown() {
		if (Status != null && Status.CanBuy) {
			Status.BuyUnit();
		}
	}
}
