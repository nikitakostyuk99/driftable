using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierGenerator : MonoBehaviour {

	[Header("Prefabs")]
	public GameObject barrierEntity;

	public Transform creationPoint;
	public Transform barriersParent;

	private List<Transform> barriers;
	private List<bool> usedBarriersState;

	private Transform lastCreatedBarrier;
	private Vector3 minDistanceBetweenBarriers = new Vector3(0, 10,0);

	private void Awake(){
		barriers = new List<Transform> ();
		usedBarriersState = new List<bool> ();

		GenerateBarriers (7);
	}

	//make free barrier visible to player
	public void GetBarrier(){
		var barrier = GetFirstFreeBarrier ();
		if (lastCreatedBarrier != null)
			barrier.position = lastCreatedBarrier.position + minDistanceBetweenBarriers;
		else
			barrier.position = creationPoint.position;

		barrier.gameObject.SetActive (true);
		lastCreatedBarrier = barrier;
	}
	//if barrier if contact with screen edge then hide this barrier(make barrier free)
	public void HideBarrier(Transform barrier){
		usedBarriersState [barriers.IndexOf (barrier)] = false;
		barrier.gameObject.SetActive (false);
	}



	//generate some barriers and add there to barriers list. The number of barriers is count parameter 
	private void GenerateBarriers(int count){
		for (var i = 0; i < count; i++) {
			var barrier = Instantiate (
				barrierEntity, 
				creationPoint.position, 
				Quaternion.identity,
				barriersParent);
			barrier.SetActive (false);
			barriers.Add (barrier.transform);
			usedBarriersState.Add (false);
		}
	}
	//if fre barrier is exist then return the first free barrier else expand barriers list by generating some new barriers 	
	private Transform GetFirstFreeBarrier(){
		for (var i = 0; i < usedBarriersState.Count; i++) {
			if (!usedBarriersState [i]) {
				usedBarriersState [i] = true;
				return barriers [i];
			}
		}
		return ExpandBarriersList ();//if free barrier not exist
	}
	//expand barriers list by generating some new barriers
	private Transform ExpandBarriersList(){
		GenerateBarriers(3);
		return barriers[barriers.Count-1];
	}




}
