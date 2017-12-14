using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DayCycleManager : MonoBehaviour {

	public bool dayHasEnded;

    public List<Day> day;
    public GameObject player;
    public int numCustomersThatLeft;
    public int currentDay;

    public GameObject CustomerIvory;
    public GameObject CustomerSahana;

    private DialogueRunner dialogue;

    private int numCurrentCustomers;

	// Use this for initialization
    public void Awake(){


    }
	public void Start () {
        player = Instantiate(Services.Prefabs.Player, new Vector3(0, 4.16f, 8.35f), Quaternion.Euler(0,180,0));
        CustomerIvory = GameObject.Find("CustomerIvory");
        CustomerSahana = GameObject.Find("CustomerSahana");
        dialogue = FindObjectOfType<DialogueRunner>();
        CustomerIvory.SetActive(false);
        CustomerSahana.SetActive(false);
 		dayHasEnded = false;

        day = new List<Day>();
        day.Add(new Day(1)); //just one customer on the first day
        day.Add(new Day(2)); //two customers on the second day, etc.

        numCustomersThatLeft = 0;
        currentDay = 0;

        //currentNumCustomers = day[0].numCustomers;

	}

    public void ResetDay(){
        dayHasEnded = false;
    }

    public void Update(){
        if(!dayHasEnded){

            Day(currentDay);
        }
        if(numCustomersThatLeft == day[currentDay].numCustomers){
            numCustomersThatLeft = 0;
            currentDay++;
            dayHasEnded = true;

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
        
        if (numCurrentCustomers == 0)
        {
            Ray ray = new Ray(Services.GameManager.currentCamera.transform.position, Services.GameManager.currentCamera.transform.forward);
            float rayDist = Mathf.Infinity;
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit, rayDist))
            {
                if (hit.transform.name.Contains("InitialCustomerTrigger"))
                {
                    CustomerIvory.SetActive(true);
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
        
    }






}
