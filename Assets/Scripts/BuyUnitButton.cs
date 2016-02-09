using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuyUnitButton : MonoBehaviour {
	public BuyUnitMenu _menu;
	public	UnitPurchaseOption _purchaseOption;

	bool _isShowingCooldown;
	Transform _cooldownOverlay;
	Transform _disabledOverlay;
	SpriteRenderer _progressBar;
	Text _costText;
	
	public BuyUnitMenu Menu {
		get {
			return _menu;
		}
		set {
			_menu = value;
		}
	}
	
	public UnitPurchaseOption PurchaseOption {
		get {
			return _purchaseOption;
		}
		set {
			_purchaseOption = value;
		}
	}

	// Use this for initialization
	void Start () {
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

		_costText.text = _purchaseOption.CreditCost.ToString ();

		// sync cooldown overlay status initially
		ToggleCooldownOverlay ();
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Assert (_cooldownOverlay != null && _progressBar != null);

		_disabledOverlay.gameObject.SetActive (!_purchaseOption.IsReady || !_purchaseOption.HasSufficientFunds);

		var isReady = _purchaseOption.IsReady;
		if (isReady == _isShowingCooldown) {
			ToggleCooldownOverlay();
		}
		if (!isReady) {
			UpdateCooldownProgress();
		}
	}

	void ToggleCooldownOverlay() {
		_cooldownOverlay.gameObject.SetActive(_isShowingCooldown = !_purchaseOption.IsReady);
	}

	void UpdateCooldownProgress() {
		var progress = _purchaseOption.SecondsSinceLastPurchase / _purchaseOption.CooldownSeconds;
		_progressBar.transform.localScale = new Vector2(progress, 1);
	}

	
	void OnMouseDown() {
		if (!_purchaseOption.IsReady || !_purchaseOption.HasSufficientFunds) {
			return;
		}

		_menu.FactionManager.TryPurchaseUnit (_purchaseOption);
	}
}
