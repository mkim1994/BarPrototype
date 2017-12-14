using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SwitchEngine : Interactable {

	private bool isBeingLookedAt;
	public GameObject button;
	private GameObject mouseOver;
	// Use this for initialization
	public override void Start () {
		//mouseOver = transform.GetChild(1).gameObject;
	}
	
	// Update is called once per frame
	public override void Update () {
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

	public override void TweenToHand(Vector3 _handPos){
	}

	public override void TweenToTable(Vector3 _tablePos){
	}

	public override void ReturnToInitHandPos(Vector3 _initHandPos, Vector3 _initHandRot){
		
	}
}
