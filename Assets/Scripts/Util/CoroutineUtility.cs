using UnityEngine;
using System;
using System.Collections;

// see: http://answers.unity3d.com/questions/542115/is-there-any-way-to-use-coroutines-with-anonymous.html
public static class CoroutineUtility {
	
	/**
      * Usage: StartCoroutine(CoroutineUtils.Chain(...))
      * For example:
      *     StartCoroutine(CoroutineUtils.Chain(
      *         CoroutineUtils.Do(() => Debug.Log("A")),
      *         CoroutineUtils.WaitForSeconds(2),
      *         CoroutineUtils.Do(() => Debug.Log("B"))));
      */
//	public static IEnumerator Chain(params IEnumerator[] actions) {
//		foreach (IEnumerator action in actions) {
//			yield return GlobalManager.Instance.StartCoroutine(action);
//		}
//	}
	
	/**
      * Usage: StartCoroutine(CoroutineUtils.DelaySeconds(action, delay))
      * For example:
      *     StartCoroutine(CoroutineUtils.DelaySeconds(
      *         () => DebugUtils.Log("2 seconds past"),
      *         2);
              */
	public static IEnumerator DelaySeconds(float delay, Action action) {
		yield return new WaitForSeconds(delay);
		action();

	}
//	public static Coroutine StartDelaySeconds(float delay, Action action) {
//		return GlobalManager.Instance.StartCoroutine(DelaySeconds(delay, action));
//	}
	
//	public static IEnumerator WaitForSeconds(float time) {
//		yield return new WaitForSeconds(time);
//	}
	
	public static IEnumerator Do(Action action) {
		action();
		yield return 0;
	}
}