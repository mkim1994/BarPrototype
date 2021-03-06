﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityStandardAssets.Characters.FirstPerson;

public class DayCycleManager : MonoBehaviour {

	public bool dayHasEnded;

    public bool switchOff;

    public List<Day> day;
    public GameObject player;
    public int numCustomersThatLeft;
    public int currentDay;

    public GameObject CustomerIvory;
    public GameObject CustomerSahana;

    public DialogueRunner dialogue;

    public GameObject blackPanel;

    public int numCurrentCustomers;

    public Vector3 resetPos;
    public Quaternion resetRot;

    private SnapTriggerArea[] snaps;

    AudioController audioController;
    public bool safeToEnableInteraction;

	// Use this for initialization
    public void Awake(){


    }
	public void Start () {
        audioController = FindObjectOfType<AudioController>();
        safeToEnableInteraction = false;
        snaps = FindObjectsOfType<SnapTriggerArea>();
        // player = GameObject.Find("FPS Controller");
        player = Instantiate(Services.Prefabs.Player, new Vector3(0, 4.16f, 8.35f), Quaternion.Euler(0,180,0));
        resetPos = player.transform.position;
        resetRot = player.transform.rotation;
        CustomerIvory = GameObject.Find("CustomerIvory");
        CustomerSahana = GameObject.Find("CustomerSahana");
        blackPanel = GameObject.Find("BarCanvas").transform.GetChild(2).gameObject;

        dialogue = FindObjectOfType<DialogueRunner>();
        CustomerIvory.SetActive(false);
        CustomerSahana.SetActive(false);
 		dayHasEnded = false;

        day = new List<Day>();
        day.Add(new Day(1)); //just one customer on the first day
        day.Add(new Day(2)); //two customers on the second day, etc.
        day.Add(new Day(1));

        numCustomersThatLeft = 0;
        currentDay = 0;
        switchOff = false;

        //currentNumCustomers = day[0].numCustomers;

	}

    public void ResetDay(){
        dialogue.dialogueCount = 0;
        numCustomersThatLeft = 0;
        numCurrentCustomers = 0;
        audioController.signhum.Stop();
        dayHasEnded = false;
       //safeToEnableInteraction = false;
        switchOff = false;
        blackPanel.SetActive(true);
        audioController.bgm1.Stop();
        audioController.bgm2.Stop();
        //Invoke("WaitTillNextDay", 5f);

        WaitTillNextDay();
    }

    public void Update(){
        if (currentDay < 3)
        {
            if (safeToEnableInteraction)
            {
                safeToEnableInteraction = false;
                DayCycleTrueReset();
            }
            if (!dayHasEnded)
            {

                Day(currentDay);

            }
            if (numCustomersThatLeft == day[currentDay].numCustomers)
            {
                numCustomersThatLeft = 0;
                currentDay++;
                dayHasEnded = true;
                Debug.Log(currentDay + ", " + dayHasEnded);

            }

          /*  if (dayHasEnded && switchOff)
            {
                ResetDay();
            }*/
        }
        if (dayHasEnded && switchOff)
        {
            ResetDay();
        }


    }

    private void Day(int today){
        switch(today){
            case 0:
                Day0();
                break;
            case 1:
                Day1();
                break;
        }
    }
    private void Day0(){
        
        if (numCurrentCustomers == 0 && numCustomersThatLeft == 0)
        {
            Ray ray = new Ray(player.GetComponentInChildren<Camera>().transform.position, player.GetComponentInChildren<Camera>().transform.forward);
            float rayDist = Mathf.Infinity;
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit, rayDist))
            {
                if (hit.transform.name.Contains("InitialCustomerTrigger"))
                {
                    //for snaptriggerarea: if day == 1
                    CustomerIvory.SetActive(true);
                    audioController.spotlightSfx.Play();
                    audioController.bgm1.Play();
                    dialogue.StartDialogue();
                    numCurrentCustomers++;
                }
            }
        }
        if(numCustomersThatLeft == 1){
            CustomerIvory.SetActive(false);
        }
    }
    private void Day1(){
        if (numCurrentCustomers == 0 && numCustomersThatLeft == 0)
        {
            Ray ray = new Ray(player.GetComponentInChildren<Camera>().transform.position, player.GetComponentInChildren<Camera>().transform.forward);
            float rayDist = Mathf.Infinity;
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit, rayDist))
            {
                if (hit.transform.name.Contains("SecondCustomerTrigger"))
                {
                    //for snaptriggerarea: if day == 2 & sahana is active
                    CustomerSahana.SetActive(true);
                    audioController.spotlightSfx.Play();

                    audioController.bgm2.Play();
                    dialogue.StartDialogue("StartSahanaDay2");
                    numCurrentCustomers++;
                }
            }
        } else if (numCustomersThatLeft == 1){ //after sahana leaves
            if (numCurrentCustomers == 0)
            {

                CustomerSahana.SetActive(false);
                Ray ray = new Ray(player.GetComponentInChildren<Camera>().transform.position, player.GetComponentInChildren<Camera>().transform.forward);
                float rayDist = Mathf.Infinity;
                RaycastHit hit = new RaycastHit();

                if (Physics.Raycast(ray, out hit, rayDist))
                {
                    if (hit.transform.name.Contains("InitialCustomerTrigger"))
                    {
                        //for snaptriggerarea: if day == 1
                        CustomerIvory.SetActive(true);
                        audioController.spotlightSfx.Play();
                        dialogue.StartDialogue("StartIvoryDay2");
                        numCurrentCustomers++;
                    }
                }
            }
        }
        if(numCustomersThatLeft == 2){
            CustomerIvory.SetActive(false);
        }
    }

    private void WaitTillNextDay(){
        dialogue.WaitSeconds(currentDay, 5f, player, resetPos,resetRot,blackPanel);

    }

    public void DayCycleTrueReset(){

        player.GetComponentInChildren<InteractionManager>().enabled = true;
        player.GetComponent<FirstPersonController>().enabled = true;
        audioController.signhum.Play();

    }




}
