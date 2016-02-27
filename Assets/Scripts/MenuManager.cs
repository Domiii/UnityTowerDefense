using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MenuManager : MonoBehaviour {
	public static MenuManager Instance {
		get;
		private set;
	}

	[HideInInspector]
	public int startIndex;

	public Camera _camera;
	public int StartIndex {
		get {
			return startIndex;
		}
		set {
			startIndex = value;
		}
	}

	public Canvas[] Canvases {
		get;
		private set;
	}

	public Camera Camera {
		get {
			if (_camera != null) {
				return _camera;
			}
			return Camera.main;
		}
	}

	public MenuManager() {
		Instance = this;
	}

	public IEnumerable<Canvas> GetCanvases() {
		Object[] objs = GameObject.FindObjectsOfType<Canvas>();
		return objs.Select(obj => (Canvas)obj);
	}

	// Use this for initialization
	void Awake () {
		Canvases = GetCanvases ().ToArray();
		if (StartIndex < 0 || StartIndex >= Canvases.Length) {
			StartIndex = 0;
		}
		GoToCanvas (Canvases[StartIndex]);
	}
	
	public void GoToCanvas(string name) {
		//var camera = Camera;
		var canvas = Canvases.Where (c => c.name == name).FirstOrDefault();
		if (canvas != null) {
			GoToCanvas(canvas);
		}
	}
	
	public void GoToCanvas(Canvas canvas) {
		var camera = Camera;
		camera.transform.position = canvas.transform.position + Vector3.back;
	}
}
