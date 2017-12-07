using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropzone_Manager : MonoBehaviour {

	public GameObject player;
	public DropzoneManager nearest;
	private List<DropzoneManager> dropzones = new List<DropzoneManager>();
	// Use this for initialization

	void Start () {
		player = GameObject.Find("FPSController");
		dropzones.AddRange(FindObjectsOfType<DropzoneManager>());
	}
	
	// Update is called once per frame
	void Update () {
		nearest = FindTheClosestFreeDropZone();
		HighlightNearest();
	}

	public DropzoneManager FindTheClosestFreeDropZone(){
		DropzoneManager _nearest = dropzones[0];
		float shortestDist = Vector3.Distance(dropzones[0].transform.position, player.transform.position);
		for(int i = 0; i < dropzones.Count; i++){			
			if(Vector3.Distance(dropzones[i].transform.position, player.transform.position) <= shortestDist && !dropzones[i].isOccupied){
				shortestDist = Vector3.Distance(dropzones[i].transform.position, player.transform.position);
				_nearest = dropzones[i];
			}
		}
		return _nearest;
	}

	public void HighlightNearest(){
		for(int i = 0; i<dropzones.Count; i++){
			if(dropzones[i] == nearest){
				dropzones[i].GetComponent<MeshRenderer>().material.color = Color.green;	
			} else if (dropzones[i].isOccupied){
				dropzones[i].GetComponent<MeshRenderer>().material.color = Color.blue;
			} else if (!dropzones[i].isOccupied){
				dropzones[i].GetComponent<MeshRenderer>().material.color = Color.red;
			}
		}
	}
}
