using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QuizAnswerButton : MonoBehaviour {
	public QuizDialog Dialog;
	public QuizAnswer Answer;

	// Use this for initialization
	void Start () {
		var btn = GetComponent<Button>();
		if (btn != null) {
			btn.onClick.AddListener(OnMouseDown);
		}
	}
	
	// Update is called once per frame
	void OnMouseDown() {
		Dialog.SubmitAnswer (Answer);
	}
}
