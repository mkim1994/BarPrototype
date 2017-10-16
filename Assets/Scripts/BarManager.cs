using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarManager : MonoBehaviour {

    public Light[] spotlights;
    public Transform spotlightDir;
    public Transform tableDir;
	// Use this for initialization
	void Start () {
        /* foreach (Light sp in spotlights)
         {
             Ray ray = new Ray(sp.transform.position, spotlightDir.position);
             RaycastHit hit = new RaycastHit();
             if (Physics.Raycast(ray, out hit, Mathf.Infinity))
             {
                 Vector3 target = hit.point;
                 sp.transform.LookAt(target);
             }
         }*/
        foreach(Light sp in spotlights){
            sp.transform.LookAt(spotlightDir.position);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Tab)){
            foreach(Light sp in spotlights){
                
                sp.transform.LookAt(tableDir);
            }
        }
	}


}
