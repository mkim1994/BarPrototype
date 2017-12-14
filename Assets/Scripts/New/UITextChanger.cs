using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextChanger : MonoBehaviour {

	Text uiText; 
	
	// Use this for initialization
	void Start () {
		uiText = GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		uiText.text = "Go Fuck YourSelf";
	}

	void FindObjectForText(){
		Ray ray = new Ray(transform.position, transform.forward);
		float rayDist = Mathf.Infinity;
		RaycastHit hit = new RaycastHit();

		if(Physics.Raycast(ray, out hit, rayDist)){
			GameObject hitObj = hit.transform.gameObject;
			if(hitObj.GetComponent<Interactable>() != null){
				Interactable thisObj = hitObj.GetComponent<Interactable>();
				
			}
			//only check if you're looking at a coaster if you're holding something
		} else {
			//if no hit
		}
	}
}
