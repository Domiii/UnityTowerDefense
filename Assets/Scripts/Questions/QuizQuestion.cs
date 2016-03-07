using UnityEngine;
using System.Collections;

[System.Serializable]
public class QuizQuestion {
	public string QuestionText;
	public QuizAnswer[] Answers = new QuizAnswer[0];
}

[System.Serializable]
public class QuizAnswer {
	public string AnswerText;
	public bool IsCorrect;
}