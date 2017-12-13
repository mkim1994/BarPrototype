using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchControl : MonoBehaviour {

	public AudioController audioController;
	//private GameObject[] lights;
	private CameraController cameraController;
	// Use this for initialization
	void Start () {
		//lights = GameObject.FindGameObjectsWithTag("Lights");
		cameraController = GetComponent<CameraController>();
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = new Ray(transform.position, transform.forward);
		RaycastHit hit = new RaycastHit();
		if(Physics.Raycast(ray, out hit, Mathf.Infinity)){
			if(hit.transform.GetComponent<SwitchEngine>() != null){
				SwitchEngine switchEngine = hit.transform.GetComponent<SwitchEngine>();
				// switchEngine.ShowMouseOverObject();
				if(Input.GetKeyDown(KeyCode.Joystick1Button5) || Input.GetKeyDown(KeyCode.Joystick1Button4)){
					if(Services.DayCycleManager.dayHasEnded){
						switchEngine.AnimateButtonPress();
						audioController.spotlightSfx.PlayScheduled(AudioSettings.dspTime);
						// switchEngine.HideMouseOverObject();
						/*foreach(GameObject light in lights){
							light.SetActive(false);
						}*/
						this.enabled = false;
					} else {
						Debug.Log("Some customers still need you.");
					}
				}
			} else {
			}
		}		
	}
}
