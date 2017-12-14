using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
public class Dropzone_Manager : MonoBehaviour {

	private float angleToPlayerRange = 25f;
	public float maxDistToPlayer = 4f;
	public GameObject player;
	public DropzoneManager nearest;
	public List<DropzoneManager> dropzones = new List<DropzoneManager>();
	// Use this for initialization

	void Start () {
		// player = FindObjectOfType<FirstPersonController>().gameObject;
		dropzones.AddRange(FindObjectsOfType<DropzoneManager>());
	}

	public void FindPlayer(){
		player = FindObjectOfType<FirstPersonController>().gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		nearest = FindTheClosestFreeDropZone();
		HighlightNearest();
		// if(nearest != null)
			// Debug.Log(nearest.PlayerIsFacingDropzone());
			// Debug.Log()
	}

	public DropzoneManager FindTheClosestFreeDropZone(){
		DropzoneManager _nearest = dropzones[0];
		float shortestDist = Vector3.Distance(dropzones[0].transform.position, player.transform.position);
		for(int i = 0; i < dropzones.Count; i++){			
			if(Vector3.Distance(dropzones[i].transform.position, player.transform.position) <= shortestDist 
			&& !dropzones[i].isOccupied
			&& dropzones[i].AngleToPlayer() <= angleToPlayerRange){
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
