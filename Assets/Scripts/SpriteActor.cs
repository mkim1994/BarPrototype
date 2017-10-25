using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class SpriteActor : MonoBehaviour {

    private Transform player;

    public DialogueRunner dr;

	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player").transform;

	}
	
	// Update is called once per frame
	void Update () {
        RotateSpriteTowardPlayer();
	}

    void RotateSpriteTowardPlayer(){
        transform.LookAt(player);
    }

}
