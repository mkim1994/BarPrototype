using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class DropzoneManager : MonoBehaviour {

	private GameObject player;
	//private Camera playerCam;
	[SerializeField]float myAngleToPlayer;
	Dropzone_Manager dropzone_Manager;
	public bool isOccupied;
	// Use this for initialization
	public virtual void Start () {
		isOccupied = false;
		// player = FindObjectOfType<FirstPersonController>().gameObject;
		//playerCam = player.GetComponentInChildren<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		myAngleToPlayer = AngleToPlayer();
	}

	void OnTriggerEnter(Collider collider){
		if(collider.GetComponent<Interactable>() != null){
			// Debug.Log(collider.name);
			isOccupied = true;
		}
	}

	void OnTriggerStay(Collider collider){
		if(collider.GetComponent<Interactable>() != null){
			isOccupied = true;
		}
	}

	void OnTriggerExit(Collider collider){
		if(collider.GetComponent<Interactable>() != null){
			isOccupied = false;	
		}
	}
	
	public void RevealNearestDropzone(){
		// Debug.Log("I'm the closest! " + gameObject.name);
		GetComponent<MeshRenderer>().material.color = Color.green;
	}

	public bool PlayerIsFacingDropzone(){
		bool playerIsFacing = false;
		float angleToDropzone = Vector3.Angle(player.transform.forward, (player.transform.position-transform.position));
		if(angleToDropzone <= 20f)
			playerIsFacing = true;
		return playerIsFacing;
	}

	public float AngleToPlayer(){
        if (gameObject.activeSelf)
        {
            float angleToPlayer = 0f;
            angleToPlayer = Vector3.Angle(Services.DayCycleManager.player.GetComponentInChildren<Camera>().transform.forward, (transform.position - Services.DayCycleManager.player.GetComponentInChildren<Camera>().transform.position));
            // Debug.Log("Nearest's angle to player is " + angleToPlayer);
            return angleToPlayer;
        }
        return 999999999f;
	}

	public float DistToPlayer(){
		float distToPlayer = 0f;
        distToPlayer = Vector3.Distance(Services.GameManager.currentCamera.transform.position, transform.position);
		return distToPlayer;
	}
}
