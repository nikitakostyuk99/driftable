using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	[Header("Objects")]
	public BarrierGenerator creator;
	public PhysicsPlayer player;
	public ScreenEdge screenEdge;

	[Header("Control Settings")]
	public KeyCode driftBtn; 

	private void Awake(){
		InitControl ();
		InitEvents ();
	}

	private void Start(){
		StartCoroutine ("CreateBarrier");
	}
		

	private void InitControl(){
		player.DriftBtn = driftBtn;
	}

	private void InitEvents(){
		screenEdge.OnContatctWithScreenEdge = new ScreenEdge.ContactWithScreenEdge ();
		screenEdge.OnContatctWithScreenEdge.AddListener(creator.HideBarrier);
		player.OnContactWithBarrier.AddListener (() => SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex));
		player.OnContactWithScreenEdge.AddListener (() => SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex));
	}

	IEnumerator CreateBarrier(){
		while (true) {
			creator.GetBarrier ();
			yield return new WaitForSeconds (1.5f);
		}
	}
}
