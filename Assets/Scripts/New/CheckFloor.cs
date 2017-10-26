using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFloor : MonoBehaviour {

	public bool canSeeFloor;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		CheckFloorWithRay();
	}

	public void CheckFloorWithRay(){
		Ray ray = new Ray(transform.position, Vector3.down);
		Debug.DrawRay(transform.position, Vector3.down * 10f, Color.red);
		float rayDist = Mathf.Infinity;

		RaycastHit hit = new RaycastHit();

		if(Physics.Raycast(ray, out hit, rayDist)){
			if(hit.transform.tag == "Floor"){
				canSeeFloor = true;
				//Debug.Log("FLoor is in sight");
			} else {
				canSeeFloor = false;
				//Debug.Log("not the floor");
			}
		}
	}


}
