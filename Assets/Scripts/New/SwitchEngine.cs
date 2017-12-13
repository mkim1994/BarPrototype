using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SwitchEngine : MonoBehaviour {

	private bool isBeingLookedAt;
	public GameObject button;
	private GameObject mouseOver;
	// Use this for initialization
	void Start () {
		//mouseOver = transform.GetChild(1).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if(isBeingLookedAt){
			ShowMouseOverObject();
		} else{
			HideMouseOverObject();
		}
	}

	public void AnimateButtonPress(){
		Debug.Log("Animating button!");
		button.transform.DOLocalMoveY(-0.20f, 0.3f, false);
		StartCoroutine(AnimateButtonUp(0.3f));
	}
	IEnumerator AnimateButtonUp(float delay){
		yield return new WaitForSeconds(delay);
		button.transform.DOLocalMoveY(0, 0.3f, false);
	}
	
	public void ShowMouseOverObject(){
		//mouseOver.SetActive(true);
	}

	public void HideMouseOverObject(){
		//mouseOver.SetActive(false);
	}
}
