using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;

public class QuizDialog : MonoBehaviour {
	public QuizQuestion Question;

	public UnityEvent OnRightAnswer;
	public UnityEvent OnWrongAnswer;

	Text questionText;
	List<Text> answerTexts;
	List<GameObject> answerButtons;

	void Start() {
		// find question text, all answer buttons and texts
		questionText = transform.FindFirstDescendantByName<Text> ("QuestionText");
		answerTexts = transform.FindDescendantsByName<Text> ("AnswerText");
		answerButtons = transform.FindDescendantsByName<Text> ("AnswerButtonText");

	}
}
