using UnityEngine;
using UnityEditor;

using System.Linq;

[CustomEditor(typeof(ImprovedPolygonCollider2D))]
public class ImprovedPolygonCollider2DEditor : Editor
{
	public override void OnInspectorGUI() {
		base.OnInspectorGUI ();
		
		var theTarget = (ImprovedPolygonCollider2D)target;
		var spriteRenderer = theTarget.GetComponent<SpriteRenderer> ();
		var collider = theTarget.GetComponent<PolygonCollider2D> ();
		
		if (spriteRenderer != null && collider != null && GUILayout.Button("Reset"))
		{
			ReplaceWithSimpleBox();
		}
	}
	
	void ReplaceWithSimpleBox() {
		var theTarget = (ImprovedPolygonCollider2D)target;
		var spriteRenderer = theTarget.GetComponent<SpriteRenderer> ();
		var collider = theTarget.GetComponent<PolygonCollider2D> ();

		// get bounds
		var bounds = spriteRenderer.bounds;
		Vector2 p1 = bounds.min;
		Vector2 p3 = bounds.max;
		Vector2 p2 = new Vector2(p1.x, p3.y);
		Vector2 p4 = new Vector2(p3.x, p1.y);
		collider.points = new Vector2[] { p1, p2, p3, p4 };

		// convert to object space
		collider.points = collider.points.Select(p => (Vector2)collider.transform.InverseTransformPoint (p)).ToArray();
		
		EditorUtility.SetDirty(target);
	}
}