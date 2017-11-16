using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SwitchEngine : MonoBehaviour {


	public GameObject button;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
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
}
