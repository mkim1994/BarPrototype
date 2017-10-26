﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class CameraController : MonoBehaviour {

    public LayerMask characterActorLayer;
    public GameObject spotlight;

    public AudioController audioController;

    private bool characterIsVisible;
    public bool safeToInteract;

    public DialogueRunner dialogueRunner;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!characterIsVisible)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            float rayDist = Mathf.Infinity;

            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit, rayDist, characterActorLayer))
            {
                spotlight.SetActive(true);
                audioController.spotlightSfx.Play();
                characterIsVisible = true;
                Invoke("StartDialogue", 2f);
            }
        }
	}

    void StartDialogue(){
        safeToInteract = true;
        dialogueRunner.StartDialogue();
    }
}
