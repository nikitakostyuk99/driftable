using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public Transform target;

	private Transform t;
	private Vector3 targetPosition;
	private void Awake(){
		t = this.transform;
	}
	
	private void LateUpdate () {
		targetPosition.Set (0, target.position.y, -10);
		t.position = targetPosition;
	}
}
