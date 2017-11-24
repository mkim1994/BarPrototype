using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Interactable : MonoBehaviour {

	// public Material[] materials;

	//offset for when Interactable is dropped onto a SnapTriggerArea. This will vary from model to model.
	[SerializeField] Vector3 myPourRotation;
	[SerializeField]Vector3 myStopPourRotation;

	public Vector3 dropOffset;
	public Vector3 rotOffset;

	private Rigidbody rb;
	public bool isHeld;
	public Transform child;
	public enum IsBeingLookedAtState {
		LOOKED_AT,
		NOT_LOOKED_AT
	}

	public Ingredients.BaseType baseType;
 
	public IsBeingLookedAtState isBeingLookedAtState;

	public Vector3 startRot;
	public Vector3 startPos;
	// Use this for initialization
	void Start () {
		// child = gameObject.transform.Find("MouseOverBottle");
		rb = GetComponent<Rigidbody>();
 		child = transform.GetChild(0);
		startPos = transform.position;
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
		transform.DOLocalRotate(new Vector3(0, 25, 90f), 0.75f, RotateMode.Fast);
		// transform.localEulerAngles = new Vector3(0, 25, 90f);
		//drink level UI updating could also happen here?
	} 

	public void StopPour(){
		// transform.localEulerAngles = startRot;
		// StartCoroutine(StopPourDelayed(0.76f));		
		transform.DOLocalRotate(startRot, 0.3f, RotateMode.Fast);
	}
	IEnumerator StopPourDelayed(float delay){
		yield return new WaitForSeconds(delay);
		// transform.localEulerAngles = startRot;
		transform.DOLocalRotate(startRot, 0.3f, RotateMode.Fast);
	}
	public void ChangeMaterialOnRaycastHit(){
		isBeingLookedAtState = IsBeingLookedAtState.LOOKED_AT;
	}

	public void ChangeMaterialOnRaycastMiss(){
		isBeingLookedAtState = IsBeingLookedAtState.NOT_LOOKED_AT;
 	}

	void OnTriggerEnter(Collider coll){
		//check if Trigger is a SnapTriggerArea
		if(coll.GetComponent<SnapTriggerArea>()!= null){
			coll.GetComponent<SnapTriggerArea>().posOffset = dropOffset;
		}
		//check if trigger is the floor
		
	}

	void OnCollisionEnter(Collision coll){
		if(coll.transform.tag == "Floor"){
			rb.isKinematic = true;
			transform.position = startPos;
			transform.eulerAngles = startRot;
			rb.isKinematic = false;
		}
	}
}
