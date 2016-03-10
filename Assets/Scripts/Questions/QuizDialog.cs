using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;

public class QuizDialog : MonoBehaviour {
	static int ExtraCreditsOnStart;

	public static int GetExtraStartCredits() {
		var credit = ExtraCreditsOnStart;
		ExtraCreditsOnStart = 0;
		return credit;
	}

	public QuestionSet Questions;
	public int RightAnswerCreditReward;
	public GameObject RightPrefab, WrongPrefab;

	[HideInInspector]
	public string NextScene;

	//public UnityEvent OnRightAnswer;
	//public UnityEvent OnWrongAnswer;

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
		GameObject nextPrefab;

		if (ans.IsCorrect) {
			//OnRightAnswer.Invoke ();
			ExtraCreditsOnStart = RightAnswerCreditReward;
			nextPrefab = RightPrefab;

		} else {
			ExtraCreditsOnStart = 0;
			nextPrefab = WrongPrefab;
			//OnWrongAnswer.Invoke();
		}

		if (nextPrefab == null) {
			Application.LoadLevel (NextScene);
		} else {
			var go = (GameObject)Instantiate(nextPrefab);
			var nextSceneButton = go.transform.FindFirstDescendantByName("NextSceneButton");
			if (nextSceneButton != null) {
				var switchScene = nextSceneButton.GetComponent<SwitchSceneButton>();
				if (switchScene == null) {
					switchScene = nextSceneButton.gameObject.AddComponent<SwitchSceneButton>();
				}

				switchScene.SceneName = NextScene;
			}
		}
	}
}
