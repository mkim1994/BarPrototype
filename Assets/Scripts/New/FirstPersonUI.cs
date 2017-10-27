using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstPersonUI : MonoBehaviour {

	public Text descriptionText;
	// Use this for initialization
	void Start () {
		descriptionText = GetComponentInChildren<Text>();
		HideDescriptionText();
	}

	
	public void UpdateDescriptionText(string description){
		descriptionText.enabled = true;
		descriptionText.text = description;
		// Debug.Log("updating description text");
	}

	public void HideDescriptionText(){
		descriptionText.enabled = false;
		// Debug.Log("hiding description text!");
	}
}
