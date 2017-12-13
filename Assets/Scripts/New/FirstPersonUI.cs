using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstPersonUI : MonoBehaviour {
	Image reticle;
	public Text descriptionText;

	private InteractionManager interactionManager;
	// Use this for initialization
	void Start () {
		interactionManager = FindObjectOfType<InteractionManager>();
		reticle = GetComponentInChildren<Image>();
		descriptionText = GetComponentInChildren<Text>();
		HideDescriptionText();
	}

	void Update(){
		if(interactionManager.interactableCurrentlyInRangeAndLookedAt != null || interactionManager.lookingAtCoaster){
			reticle.rectTransform.localScale = new Vector3 (2, 2, 2); 
			reticle.color = Color.green;
		} else {
			reticle.rectTransform.localScale = new Vector3 (1, 1, 1); 
			reticle.color = Color.white;
		}
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
