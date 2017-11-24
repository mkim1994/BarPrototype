using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass : MonoBehaviour {
	Interactable interactable;
	private Vector3 startRot;

	public bool isDirty;

	public GameObject stains;
	public string glassName = "glass";
	// Use this for initialization
	void Start () {
		startRot = transform.localEulerAngles;		
		interactable = GetComponent<Interactable>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void EmptyGlass(){
		transform.localEulerAngles = new Vector3(0, 25, 90f);
		GetComponentInChildren<PourSimulator>().Empty();
	}

	public void StopEmptyGlass(){
		transform.localEulerAngles = startRot;
	}

	public void ShowStains(){
		stains.SetActive(true);
	}

}
