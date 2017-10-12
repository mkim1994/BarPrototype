using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkControl : MonoBehaviour {

	private GameObject objectToPickUp;
	public KeyCode pickUpKey;
	// Use this for initialization

	public enum PickUpState{
		PICKED_UP,
		NOT_PICKED_UP
	}

	PickUpState pickUpState;
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		ShootRay();

	}

	public void ShootRay(){
		Ray ray = new Ray(transform.position, transform.forward);

		RaycastHit hit = new RaycastHit();

		if(Physics.Raycast(ray, out hit, Mathf.Infinity)){
			if(hit.transform.GetComponent<Interactable>() != null){
				objectToPickUp = hit.transform.gameObject;
				PickUp();
			}
		}
	}

	public void PickUp(){
		Rigidbody rb = objectToPickUp.GetComponent<Rigidbody>();
		if(Input.GetMouseButtonDown(0)){
			switch (pickUpState){
				case PickUpState.PICKED_UP:
					rb.useGravity = true;
					rb.freezeRotation = false;
					objectToPickUp.transform.SetParent(null);
					pickUpState = PickUpState.NOT_PICKED_UP;
					break;
				case PickUpState.NOT_PICKED_UP:
					rb.useGravity = false;
					rb.freezeRotation = true;
					objectToPickUp.transform.SetParent(this.gameObject.transform);
					pickUpState = PickUpState.PICKED_UP;
					break;
				default:
					break;
			}
		}
	}

}