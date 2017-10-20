using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstPersonUI : MonoBehaviour {

	private Text descriptionText;
	// Use this for initialization
	void Start () {
		descriptionText = GetComponentInChildren<Text>();
	}

	
	public void UpdateDescriptionText(string description){
		descriptionText.enabled = true;
		descriptionText.text = description;
	}

	public void HideDescriptionText(){
		descriptionText.enabled = false;
	}
}
