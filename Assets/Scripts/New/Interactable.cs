using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Interactable : MonoBehaviour {

	public Vector3 startRot;
	// Use this for initialization
	void Start () {
		startRot = transform.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {

 	}

	public void Pour(){
		transform.localEulerAngles = new Vector3(0, 75, 90f);
		// transform.DOLocalRotate(new Vector3 (0, 0, 90), 0.5f, RotateMode.Fast);
	} 

	public void StopPour(){
		transform.localEulerAngles = startRot;
	}
}
