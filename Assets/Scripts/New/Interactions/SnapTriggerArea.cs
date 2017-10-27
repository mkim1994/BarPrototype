using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapTriggerArea : MonoBehaviour {

	private Text descriptionText;
	
	private FirstPersonUI hud;
 	private GameObject interactable;
	
	public int evaluateDrink;
	private Vector3 snapPos;
	private Vector3 snapRot;
	
	public Vector3 posOffset;
	public Vector3 rotOffset;
	public enum SnapTriggerAreaState{
		INTERACTABLE_IS_IN,
		INTERACTABLE_IS_OUT,
		INTERACTABLE_IS_POSITIONED

	}

	public SnapTriggerAreaState snapState;

	void Start () {
		snapPos = transform.parent.position;
		snapRot = transform.parent.eulerAngles;
		snapState = SnapTriggerAreaState.INTERACTABLE_IS_OUT;
		hud = GameObject.Find("FirstPersonCharacter").GetComponent<FirstPersonUI>();
  	}
	
	// Update is called once per frame
	void Update () {
		switch(snapState){
			case SnapTriggerAreaState.INTERACTABLE_IS_IN:
    			SnapToTarget();
    			break;
			case SnapTriggerAreaState.INTERACTABLE_IS_OUT:
			    break;
			case SnapTriggerAreaState.INTERACTABLE_IS_POSITIONED:
			    EvaluateDrink();
			    //Detect drink type.
			    break;
			default:
			    break;
		}
		
	}

	void OnTriggerEnter(Collider interactable_){
		if(interactable_.GetComponent<Interactable>() != null){
			interactable = interactable_.gameObject;
			snapState = SnapTriggerAreaState.INTERACTABLE_IS_IN;
 		}
	}

	void OnTriggerExit(Collider interactable_){
		if(interactable_.GetComponent<Interactable>() != null)
			interactable = null;
			snapState = SnapTriggerAreaState.INTERACTABLE_IS_OUT;
	}

	void SnapToTarget(){
		if(!interactable.GetComponent<Interactable>().isHeld){
			hud.HideDescriptionText();
			interactable.transform.position = snapPos + posOffset;
			// interactable.transform.eulerAngles = snapRot + rotOffset;
			// interactable.GetComponent<Rigidbody>().isKinematic = true;
			snapState = SnapTriggerAreaState.INTERACTABLE_IS_POSITIONED;
		}
	}

	void EvaluateDrink(){
		if(interactable.GetComponent<Base>() != null){
			Base thisDrink = interactable.GetComponent<Base>();
            if (thisDrink.baseType == Ingredients.BaseType.WHISKY &&
                thisDrink.GetComponent<Glass>() != null){
                evaluateDrink = 2;
            }
			else if(thisDrink.baseType == Ingredients.BaseType.WHISKY){
				evaluateDrink = 1;
				Debug.Log("Whiskey dropped!");
            }else{
				evaluateDrink = -1;
				Debug.Log("THIS IS NOT WHISKEY!");
            }
        } else if(interactable.GetComponent<Dilute>() != null && interactable.GetComponent<Base>() == null){
            evaluateDrink = -1;
        }
	}

}
