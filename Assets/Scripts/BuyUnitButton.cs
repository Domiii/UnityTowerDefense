using UnityEngine;
using System.Collections;

public class BuyUnitButton : MonoBehaviour {
	public BuyUnitMenu _menu;
	public	UnitPurchaseOption _purchaseOption;

	bool _isShowingCooldown;
	Transform _cooldownOverlay;
	SpriteRenderer _progressBar;
	
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

		_progressBar = _cooldownOverlay.FindFirstDescendantByName<SpriteRenderer>("CooldownProgress");
		if (_progressBar == null) {
			Debug.LogError("BuyUnitButton is missing child \"CooldownProgress\"", this);
			return;
		}

		// sync cooldown overlay status initially
		ToggleCooldownOverlay ();
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Assert (_cooldownOverlay != null && _progressBar != null);

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
		Debug.Log(_cooldownOverlay.gameObject.activeInHierarchy);
	}

	void UpdateCooldownProgress() {
		var progress = _purchaseOption.SecondsSinceLastPurchase / _purchaseOption.CooldownSeconds;
		_progressBar.transform.localScale = new Vector2(progress, 1);
	}

	
	void OnMouseDown() {
		if (!_purchaseOption.IsReady) {
			return;
		}

		_menu.FactionManager.TryPurchaseUnit (_purchaseOption);
	}
}
