using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Interactable : MonoBehaviour {

	public Material[] materials;

	public enum IsBeingLookedAtState {
		LOOKED_AT,
		NOT_LOOKED_AT
	}

	public IsBeingLookedAtState isBeingLookedAtState;

	public Vector3 startRot;
	// Use this for initialization
	void Start () {
		startRot = transform.eulerAngles;
		isBeingLookedAtState = IsBeingLookedAtState.NOT_LOOKED_AT;
	}
	
	// Update is called once per frame
	void Update () {
		switch(isBeingLookedAtState){
			case IsBeingLookedAtState.LOOKED_AT:
			GetComponent<MeshRenderer>().material = materials[0];
				break;
			case IsBeingLookedAtState.NOT_LOOKED_AT:
			GetComponent<MeshRenderer>().material = materials[1];
				break;
			default:
				break;
		}
		ChangeMaterialOnRaycastMiss();
 	}

	public void Pour(){
		transform.localEulerAngles = new Vector3(0, 25, 90f);
		// transform.DOLocalRotate(new Vector3 (0, 0, 90), 0.5f, RotateMode.Fast);
	} 

	public void StopPour(){
		transform.localEulerAngles = startRot;
	}

	public void ChangeMaterialOnRaycastHit(){
		isBeingLookedAtState = IsBeingLookedAtState.LOOKED_AT;
	}

	public void ChangeMaterialOnRaycastMiss(){
		isBeingLookedAtState = IsBeingLookedAtState.NOT_LOOKED_AT;
		// GetComponent<MeshRenderer>().material = materials[1];
	}
}
