using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Interactable : MonoBehaviour {

	// public Material[] materials;

	public Transform child;
	public enum IsBeingLookedAtState {
		LOOKED_AT,
		NOT_LOOKED_AT
	}

	public Ingredients.BaseType baseType;
 
	public IsBeingLookedAtState isBeingLookedAtState;

	public Vector3 startRot;
	// Use this for initialization
	void Start () {
		// child = gameObject.transform.Find("MouseOverBottle");
 		child = transform.GetChild(0);
 		startRot = transform.eulerAngles;
		isBeingLookedAtState = IsBeingLookedAtState.NOT_LOOKED_AT;
	}
	
	// Update is called once per frame
	void Update () {
		switch(isBeingLookedAtState){
			case IsBeingLookedAtState.LOOKED_AT:
				child.GetComponent<MeshRenderer>().enabled = true;
  				break;
			case IsBeingLookedAtState.NOT_LOOKED_AT:
				child.GetComponent<MeshRenderer>().enabled = false;
 				break;
			default:
				break;
		}

		ChangeMaterialOnRaycastMiss();
 	}

	public void Pour(){
		transform.localEulerAngles = new Vector3(0, 25, 90f);
		//drink level UI updating could also happen here?
	} 

	public void StopPour(){
		transform.localEulerAngles = startRot;
	}

	public void ChangeMaterialOnRaycastHit(){
		isBeingLookedAtState = IsBeingLookedAtState.LOOKED_AT;
	}

	public void ChangeMaterialOnRaycastMiss(){
		isBeingLookedAtState = IsBeingLookedAtState.NOT_LOOKED_AT;
 	}
}
