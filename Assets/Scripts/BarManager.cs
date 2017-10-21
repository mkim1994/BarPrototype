using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarManager : MonoBehaviour {

    public GameObject[] spotlights;
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
      /*  foreach(Light sp in spotlights){
            sp.transform.LookAt(spotlightDir.position);
        }*/
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Tab)){

            if(Input.GetKeyUp(KeyCode.Alpha1)){
                spotlights[0].SetActive(true);
            } if(Input.GetKeyUp(KeyCode.Alpha2)){
                spotlights[1].SetActive(true);
            }if(Input.GetKeyUp(KeyCode.Alpha3)){
                spotlights[2].SetActive(true);
            }if(Input.GetKeyUp(KeyCode.Alpha4)){
                spotlights[3].SetActive(true);
            }if(Input.GetKeyUp(KeyCode.Alpha5)){
                spotlights[4].SetActive(true);
            }if(Input.GetKeyUp(KeyCode.Alpha6)){
                spotlights[5].SetActive(true);
            }if(Input.GetKeyUp(KeyCode.Alpha7)){
                spotlights[6].SetActive(true);
            }if(Input.GetKeyUp(KeyCode.Alpha8)){
                spotlights[7].SetActive(true);
            }if(Input.GetKeyUp(KeyCode.Alpha9)){
                spotlights[8].SetActive(true);
            }if(Input.GetKeyUp(KeyCode.Alpha0)){
                spotlights[9].SetActive(true);
            } else if(Input.GetKeyUp(KeyCode.Space)){
                foreach(GameObject sp in spotlights){
                    sp.SetActive(false);
                }
            }
        }
	}


}
