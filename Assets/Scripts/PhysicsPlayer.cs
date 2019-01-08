using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicsPlayer : MonoBehaviour {

	public UnityEvent OnContactWithBarrier;
	public UnityEvent OnContactWithScreenEdge;
	public AnimationCurve curve;

	public KeyCode DriftBtn{ get; set;}

	private float torqueFactor = -130;//-95f;
	private float velocityFactor = 7;

	private Rigidbody2D rb;
	private Transform t;
	void Awake(){
		//cache some player components
		rb = this.GetComponent<Rigidbody2D> ();
		t = this.transform;
		DetectDirection ();
		rb.velocity = velocityFactor * t.up;
	}


	private float timer = 0;
	void Update(){
		if (Input.GetKeyDown (DriftBtn)) {
			lerpRotate = false;
			DetectDirection ();
			LerpRotate(-direction*90);
			//rb.rotation = -direction*90;
			//rb.angularVelocity = direction * torqueFactor;
			LerpVelocity (direction* Vector2.right * velocityFactor);
			//rb.AddForce (50*Vector2.up, ForceMode2D.Impulse);
			//Debug.LogWarning (rb.velocity);
		}
			
		if (Input.GetKeyUp (DriftBtn)) {
			//rb.angularVelocity = 0;
			//rb.rotation = 0;

			LerpRotate (0);
			LerpVelocity (Vector2.up * velocityFactor,3f);

		}

		//rb.velocity = velocityFactor*t.up;
		Debug.Log (rb.velocity);
		//rotation lerp 
		if (lerpRotate) {
			timer += Time.deltaTime;
			t.rotation = Quaternion.LerpUnclamped (
				t.rotation, 
				Quaternion.AngleAxis (
					lerpAngle/* + direction * curve.Evaluate (timer)*/,
					Vector3.forward), 
				lerpRotationSpeed * Time.deltaTime);
		}
		//-------------
		if (lerpVelocity) {
			rb.velocity = Vector2.Lerp (
				rb.velocity, 
				newVelocity,
				lerpVelocitySpeed * Time.deltaTime);
		}

	}
	private bool lerpVelocity;
	private Vector2 newVelocity;
	private float lerpVelocitySpeed = 0.8f;
	private void LerpVelocity(Vector2 toVelocity, float speed = 0.9f){
		lerpVelocity = true;
		newVelocity = toVelocity;
		lerpVelocitySpeed = speed;
	}



	private bool lerpRotate;
	private int lerpAngle;
	private float lerpRotationSpeed = 3f;
	private void LerpRotate(int angle, float speed = 3f){
		lerpRotate = true;
		lerpAngle = angle;
		timer = 0;
		lerpRotationSpeed = speed;
	}

	private int direction;
	//detect direction of drift
	private void DetectDirection(){
		if (t.position.x > 0) {
			direction = -1;
		} else {
			direction = 1;
		}
	}

	//if player contact with barrier or screen edge then restart the scene
	private void OnTriggerEnter2D(Collider2D collider){
		switch (collider.tag) {
			case "Barriers":
				{
					if (OnContactWithBarrier == null) {
						OnContactWithBarrier = new UnityEvent ();
					}
					OnContactWithBarrier.Invoke ();
					break;
				}
			case "ScreenEdges":
				{
					if (OnContactWithScreenEdge == null) {
						OnContactWithScreenEdge = new UnityEvent ();
					}
					OnContactWithScreenEdge.Invoke ();
					break;
				}
			default:
				break;
		}

	}
		
}
