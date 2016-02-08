using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;
using System.Linq;

[CustomEditor(typeof(BuyUnitMenu))]
public class BuyUnitMenuEditor : Editor {
	static Sprite CreateUnitPreview(Object asset, Bounds worldBounds) {
		var previewTexture = AssetPreview.GetAssetPreview (asset);

		// compute pixels per unit size to fill given bounds while keeping aspect ratio
		var texW = previewTexture.width;
		var texH = previewTexture.height;
		var worldW = worldBounds.max.x - worldBounds.min.x;
		var worldH = worldBounds.max.y - worldBounds.min.y;
		var pixelsPerUnit = Mathf.Min (texW / worldW, texH / worldH);

		// create sprite
		Sprite sprite = new Sprite ();
		var previewSpriteRect = new Rect(new Vector2(0,0), new Vector2(texW, texH));
		var pivot = new Vector2 (0.5f, 0.5f);
		sprite = Sprite.Create (previewTexture, previewSpriteRect, pivot, pixelsPerUnit);

		return sprite;
	}

	void CreateButton(int index, UnitPurchaseOption purchaseOption) {
		var menu = (BuyUnitMenu)target;

		// create button
		var btn = (GameObject)PrefabUtility.InstantiatePrefab (menu.ButtonPrefab);
		var btnSettings = btn.GetComponent<BuyUnitButton> ();
		btnSettings.Menu = menu;
		btnSettings.PurchaseOption = purchaseOption;

		// add button to menu
		btn.transform.position = menu.transform.position;
		btn.transform.SetParent (menu.transform, true);

		var buttonRenderer = btn.GetComponent<SpriteRenderer> ();

		// move to correct position
		// make sure the min of the first button starts at the menu pivot point
		var buttonXMin = buttonRenderer.bounds.min.x;
		var buttonXMax = buttonRenderer.bounds.max.x;

		var w = buttonXMax - buttonXMin;
		var xOffset = menu.transform.position.x - buttonXMin;
		btn.transform.Translate (new Vector2(xOffset + w * index, 0));

		var previewObject = btn.transform.FindChild ("Preview");
		if (previewObject == null) {
			// just ignore it, for now
			//Debug.LogError("ButtonPrefab is missing \"Preview\" child in object hierarchy.", this);
		}
		else {
			// create preview image
			var previewRenderer = previewObject.GetComponent<SpriteRenderer> ();
			var previewBounds = previewRenderer.bounds;
			previewRenderer.sprite = CreateUnitPreview(purchaseOption.UnitPrefab, previewBounds);

			var previewSize = previewBounds.max - previewBounds.min;
			var newSize = previewRenderer.bounds.max - previewRenderer.bounds.min;
			var scale = previewRenderer.transform.localScale;

			// make sure, new sprite fills out the entire space
			previewRenderer.transform.localScale =  new Vector2(scale.x * previewSize.x / newSize.x, scale.y * previewSize.y / newSize.y);
		}
	}

	void CreateButtons() {
		var menu = (BuyUnitMenu)target;

		var buttonPrefab = menu.ButtonPrefab;
		var buttonPrefabRenderer = buttonPrefab != null ? buttonPrefab.GetComponent<SpriteRenderer> () : null;
		var purchaseOptions = menu.FactionManager.PurchaseOptions;

		if (buttonPrefabRenderer == null) {
			Debug.LogError("ButtonPrefab of BuyUnitMenu is missing SpriteRenderer component.", this);
			return;
		}

		if (purchaseOptions == null) {
			return;
		}

		for (int i = 0; i < purchaseOptions.Length; ++i) {
			var purchaseOption = purchaseOptions [i];
			CreateButton (i, purchaseOption);
		}
	}

	void DeleteAllButtons() {
		var menu = (BuyUnitMenu)target;
		for (var i = menu.transform.childCount-1; i >= 0; --i) {
			var child = menu.transform.GetChild(i);
			GameObject.DestroyImmediate(child.gameObject);
		}
	}

	public override void OnInspectorGUI () {
		DrawDefaultInspector();

		if (GUILayout.Button ("Create Buttons")) {
			// delete all existing children
			DeleteAllButtons();

			// create buttons
			CreateButtons();
		}
		
		if (GUILayout.Button ("Delete Buttons")) {
			DeleteAllButtons();
		}

		EditorUtility.SetDirty(target);
	}

	// Use this for initialization
	void OnEnable  () {
		
	}
}
