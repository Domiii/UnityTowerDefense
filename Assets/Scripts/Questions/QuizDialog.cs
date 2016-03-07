using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class QuizDialog : MonoBehaviour {
	public QuizQuestion Question;

	public UnityEvent OnRightAnswer;
	public UnityEvent OnWrongAnswer;

	void Start() {
		// find question text
		
		// find all answer buttons and texts
		transform.FindFirstDescendantByName
	}
}
