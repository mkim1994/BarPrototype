using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager {



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public void Update () {
        Ray ray = Services.GameManager.currentCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, Services.Main.groundLayer))
		{
            Vector3 target = hit.point;
            Services.Main.Spotlight.transform.LookAt(target);
		}
	}
}
