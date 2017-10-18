using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourSimulator : MonoBehaviour {

	private Vector3 startScale;
	[SerializeField]float yPosGrowthRate = 0.025f;
	[SerializeField]float scaleGrowthRate = 0.1f;

	private float scale; 
	private float yPos;
	private Color diluteColor;

	private MeshRenderer myMesh;
	// Use this for initialization
	void Start () {
		startScale = new Vector3 (0,transform.localScale.y,0);
		scale = 0;
		yPos = transform.position.y;
		myMesh = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
 
	}

	public void FillUpWithBase(){
		// scale += scaleGrowthRate * Time.deltaTime;
		if(yPos <= 0.25f){
			yPos += yPosGrowthRate * Time.deltaTime;
		}
		Debug.Log(yPos);
		transform.position = new Vector3 (transform.position.x, yPos, transform.position.z);
		// transform.localScale = new Vector3(scale, transform.localScale.y, scale);
		Debug.Log("Filling up!");
	}

	public void FillUpWithDilute(){
		// scale += scaleGrowthRate * Time.deltaTime;
		if(yPos <= 0.25f){
			yPos += yPosGrowthRate * Time.deltaTime;
		}
		Debug.Log(yPos);
		transform.position = new Vector3 (transform.position.x, yPos, transform.position.z);
		
		myMesh.material.color = Color.red; 
	}
}
