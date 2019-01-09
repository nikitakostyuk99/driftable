using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScreenEdge : MonoBehaviour {
	internal class ContactWithScreenEdge:UnityEvent<Transform>{
		
	}

	internal ContactWithScreenEdge OnContatctWithScreenEdge;
	void OnTriggerEnter2D(Collider2D collider){
		if (OnContatctWithScreenEdge == null) {
			OnContatctWithScreenEdge = new ContactWithScreenEdge ();
		}
		OnContatctWithScreenEdge.Invoke (collider.transform.parent);
	}
}
