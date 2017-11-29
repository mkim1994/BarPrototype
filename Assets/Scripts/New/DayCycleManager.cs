using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycleManager : MonoBehaviour {

	public bool dayHasEnded;

    public List<Day> day;

    public int numCustomersLeft;
    public int currentDay;

	// Use this for initialization
	public void Start () {
 		dayHasEnded = false;

        day = new List<Day>();
        day.Add(new Day(1)); //just one customer on the first day
        day.Add(new Day(2)); //two customers on the second day, etc.

        numCustomersLeft = 0;
        currentDay = 0;
        //currentNumCustomers = day[0].numCustomers;
	}

    public void ResetDay(){
        dayHasEnded = false;
    }

    public void Update(){
        if(numCustomersLeft == day[currentDay].numCustomers){
            numCustomersLeft = 0;
            currentDay++;
            dayHasEnded = true;

        }
    }




}
