using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicsPlayer : MonoBehaviour {

	public UnityEvent OnContactWithBarrier;
	public UnityEvent OnContactWithScreenEdge;
	public AnimationCurve curve;

	[Header("Drift Settings")]
	[Tooltip("The higher frictionFactor the drift is less")]
	[Range(0.1f,1.5f)]
	public float frictionFactor;

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

	private Vector2 GetVectorOfDirection(float angle){
		float tan = Mathf.Tan (angle*Mathf.PI/180.0f);
		float x = Mathf.Sqrt (1.0f / (1 + Mathf.Pow (tan, 2)));
		return new Vector2 (x * tan, x);
	}

	private float timer = 0;
	private float timeOfDrift = 0;
	void Update(){
		if (Input.GetKeyDown (DriftBtn)) {
			DetectDirection ();
			LerpRotate(-direction*70);
			LerpVelocity (direction*  Vector2.right * velocityFactor,frictionFactor);
			timeOfDrift = 0;
		}

		if (Input.GetKey (DriftBtn)) {
			timeOfDrift += Time.deltaTime;
		}
			
		if (Input.GetKeyUp (DriftBtn)) {
			LerpRotate (0);
			LerpVelocity (Vector2.up * velocityFactor,2.3f);
		}

		//rotation lerp 
		if (lerpRotate) {
			timer += Time.deltaTime;
			t.rotation = Quaternion.LerpUnclamped (
				t.rotation, 
				Quaternion.AngleAxis (
					lerpAngle + direction * curve.Evaluate (timer)*timeOfDrift,
					Vector3.forward), 
				lerpRotationSpeed * Time.deltaTime);
		}
		//-------------

		//velocity lerp
		if (lerpVelocity) {
			x = Mathf.Lerp (x, newVelocity.x, (lerpVelocitySpeed + speedDif) * Time.deltaTime);
			y = Mathf.Lerp (y, newVelocity.y, lerpVelocitySpeed * Time.deltaTime);
			rb.velocity = new Vector2 (x, y);

			/*rb.velocity = Vector2.Lerp (
			rb.velocity, 
			newVelocity,
			lerpVelocitySpeed * Time.deltaTime);*/
		}
		//--------------
	}
	private bool lerpVelocity;
	private Vector2 newVelocity;
	private float lerpVelocitySpeed = 0.8f;

	private float speedDif = 0;
	private float x, y;
	private void LerpVelocity(Vector2 toVelocity, float speed = 0.9f){
		lerpVelocity = true;
		newVelocity = toVelocity;
		lerpVelocitySpeed = speed;
		speedDif = 0;

		x = rb.velocity.x;
		y = rb.velocity.y;

		if (toVelocity.x == 0)
			speedDif = 1;

	}



	private bool lerpRotate;
	private int lerpAngle;
	private float lerpRotationSpeed = 3f;
	private void LerpRotate(int angle, float speed = 5f){
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
