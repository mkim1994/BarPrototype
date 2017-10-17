using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourSimulator : MonoBehaviour {

	private Vector3 startScale;
	[SerializeField]float yPosGrowthRate = 0.025f;
	[SerializeField]float scaleGrowthRate = 0.1f;

	private float scale; 
	private float yPos;
	// Use this for initialization
	void Start () {
		startScale = new Vector3 (0,transform.localScale.y,0);
		scale = 0;
		yPos = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void FillUp(){
		// scale += scaleGrowthRate * Time.deltaTime;
		yPos += yPosGrowthRate * Time.deltaTime;
		transform.position = new Vector3 (transform.position.x, yPos, transform.position.z);
		// transform.localScale = new Vector3(scale, transform.localScale.y, scale);
		Debug.Log("Filling up!");
	}
}
