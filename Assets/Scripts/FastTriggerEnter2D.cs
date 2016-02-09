﻿using UnityEngine;
using System.Collections;
using System.Linq;


/// <summary>
/// 2D adaption of the famous DontGoThroughThings component (http://wiki.unity3d.com/index.php?title=DontGoThroughThings).
/// Uses raycasting to trigger OnTriggerEnter2D events when hitting something.
/// </summary>
/// <see cref="http://stackoverflow.com/a/29564394/2228771"/>
public class FastTriggerEnter2D : MonoBehaviour {
	public enum TriggerTarget {
		None = 0,
		Self = 1,
		Other = 2,
		Both = 3
	}

	/// <summary>
	/// The layers that can be hit by this object.
	/// Defaults to "Everything" (-1).
	/// </summary>
	public LayerMask layerMask = -1;

	/// <summary>
	/// Where to send the hit event message to.
	/// </summary>
	public TriggerTarget triggerTarget = TriggerTarget.Both;

	/// <summary>
	/// How much of momentum is transfered upon impact.
	/// If set to 0, no force is applied.
	/// If set to 1, the entire momentum of this object is transfered upon the first collider and this object stops dead.
	/// If set to anything in between, this object will lose some velocity and transfer the corresponding momentum onto every collided object.
	/// </summary>
	public float momentumTransferFraction = 0;
	
	private float minimumExtent;
	private float sqrMinimumExtent;
	private Vector2 previousPosition;
	private Rigidbody2D myRigidbody;
	private Collider2D myCollider;

	
	//initialize values 
	void Awake()
	{
		myRigidbody = GetComponent<Rigidbody2D>();
		myCollider = GetComponents<Collider2D> ().FirstOrDefault();
		if (myCollider == null || myRigidbody == null) {
			Debug.LogError("DontGoThroughThings is missing Collider2D or Rigidbody2D component", this);
			enabled = false;
			return;
		}

		previousPosition = myRigidbody.transform.position;
		minimumExtent = Mathf.Min(myCollider.bounds.extents.x, myCollider.bounds.extents.y);
		sqrMinimumExtent = minimumExtent * minimumExtent;
	}
	
	void FixedUpdate()
	{
		//have we moved more than our minimum extent? 
		Vector2 movementThisStep = (Vector2)transform.position - previousPosition;
		float movementSqrMagnitude = movementThisStep.sqrMagnitude;
		
		if (movementSqrMagnitude > sqrMinimumExtent) {
			float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
			
			//check for obstructions we might have missed 
			RaycastHit2D[] hitsInfo = Physics2D.RaycastAll(previousPosition, movementThisStep, movementMagnitude, layerMask.value);
			
			//Going backward because we want to look at the first collisions first. Because we want to destroy the once that are closer to previous position
			for (int i = 0; i < hitsInfo.Length; ++i) {
				var hitInfo = hitsInfo[i];
				if (hitInfo && hitInfo.rigidbody != myRigidbody) {
					// apply force
					if (momentumTransferFraction != 0) {
						// When using impulse mode, the force argument is actually the amount of instantaneous momentum transfered.
						// Quick physics refresher: F = dp / dt = m * dv / dt
						// Note: dt is the amount of time traveled (which is the time of the current frame and is taken care of internally, when using impulse mode)
						// For more info, go here: http://forum.unity3d.com/threads/rigidbody2d-forcemode-impulse.213397/
						var dv = movementThisStep;
						var m = myRigidbody.mass;
						var dp = dv * m;
						var impulse = momentumTransferFraction * dp;
						hitInfo.rigidbody.AddForceAtPosition(impulse, hitInfo.point, ForceMode2D.Impulse);

						if (momentumTransferFraction < 1) {
							// also apply force to self (in opposite direction)
							var impulse2 = (1-momentumTransferFraction) * dp;
							hitInfo.rigidbody.AddForceAtPosition(-impulse2, hitInfo.point, ForceMode2D.Impulse);
						}
					}

					// send hit messages
					if (((int)triggerTarget & (int)TriggerTarget.Other) != 0) {
						hitInfo.collider.SendMessage("OnTriggerEnter2D", myCollider, SendMessageOptions.DontRequireReceiver);
					}
					if (((int)triggerTarget & (int)TriggerTarget.Self) != 0) {
						SendMessage("OnTriggerEnter2D", hitInfo.collider, SendMessageOptions.DontRequireReceiver);
					}
				}
			}
		}
		
		previousPosition = transform.position;
	}
}