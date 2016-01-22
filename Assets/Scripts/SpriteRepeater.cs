 using UnityEngine;
using System.Collections;

public class SpriteRepeater : MonoBehaviour {

	// Use this for initialization
	void Start () {
		var spriteRenderer = GetComponent<SpriteRenderer> ();
		spriteRenderer.sprite.texture.wrapMode = TextureWrapMode.Repeat;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
