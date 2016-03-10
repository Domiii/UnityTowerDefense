using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;

public class QuizDialog : MonoBehaviour {
	public QuestionSet Questions;

	public UnityEvent OnRightAnswer;
	public UnityEvent OnWrongAnswer;

	QuizQuestion Question;
	Text questionText;
	List<Text> answerTexts;
	List<Transform> answerButtons;


	public void Start() {
		// find question text, all answer buttons and texts
		questionText = transform.FindFirstDescendantByName<Text> ("QuestionText");
		answerTexts = transform.FindDescendantsByName<Text> ("AnswerText");
		answerButtons = transform.FindDescendantsByName<Transform> ("AnswerButton");

		PickRandomQuestion ();
		FillInQuestionData ();
	}


	public void PickRandomQuestion () {
		if (Questions.Questions.Length <= 0) {
			return;
		}

		Question = Questions.Questions[Random.Range (0, Questions.Questions.Length)];
	}


	public void FillInQuestionData() {
		if (Question == null) {
			PickRandomQuestion ();
		}

		questionText.text = Question.QuestionText;
		var minCount = System.Math.Min (Question.Answers.Length, answerButtons.Count);
		minCount = System.Math.Min (minCount, answerTexts.Count);
		for (var i = 0; i < minCount; ++i) {
			var ans = Question.Answers[i];
			answerTexts[i].text = ans.AnswerText;

			var btn = answerButtons[i].GetComponent<QuizAnswerButton>();
			if (btn == null) {
				btn = answerButtons[i].gameObject.AddComponent<QuizAnswerButton>();
			}

			btn.Dialog = this;
			btn.Answer = ans;
		}
	}


	public void SubmitAnswer(QuizAnswer ans) {
		if (ans.IsCorrect) {
			OnRightAnswer.Invoke ();
		} else {
			OnWrongAnswer.Invoke();
		}
	}
}
