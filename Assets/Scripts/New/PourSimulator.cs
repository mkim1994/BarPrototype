using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourSimulator : MonoBehaviour {


	private Vector3 startScale;
	[SerializeField]float yPosGrowthRate = 0.025f;
	[SerializeField]float scaleGrowthRate = 0.1f;

	private float scale; 
	private float yPos;
	private Color baseColor;
	private Color diluteColor;

	//add quads for the toppings?

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

	public void FillUpWithBase(Ingredients.BaseType baseType){
		// scale += scaleGrowthRate * Time.deltaTime;
		
		switch (baseType){
			case Ingredients.BaseType.GIN:
 				myMesh.material.color = Color.white;
			break;
			
			case Ingredients.BaseType.WHISKY:
 				myMesh.material.color = Color.yellow;
			break;
			
			case Ingredients.BaseType.RUM:
 				myMesh.material.color = Color.red;
			break;
			
			default:
			break;
		}

		if(yPos <= 0.25f){
			yPos += yPosGrowthRate * Time.deltaTime;
		}
 		transform.position = new Vector3 (transform.position.x, yPos, transform.position.z);
		// transform.localScale = new Vector3(scale, transform.localScale.y, scale);
	}

	public void FillUpWithDilute(Ingredients.DiluteType diluteType){

		switch (diluteType){
			case Ingredients.DiluteType.SODA:
 				myMesh.material.color = Color.red;
			break;
			
			case Ingredients.DiluteType.JUICE:
 				myMesh.material.color = Color.magenta;
			break;
			
			case Ingredients.DiluteType.TONIC_WATER:
 				myMesh.material.color = Color.white;
			break;
			
			default:
			break;
		}
		// scale += scaleGrowthRate * Time.deltaTime;
		if(yPos <= 0.25f){
			yPos += yPosGrowthRate * Time.deltaTime;
		}
 		transform.position = new Vector3 (transform.position.x, yPos, transform.position.z);
	}
}
