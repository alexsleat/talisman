using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wordClock : MonoBehaviour {

	public string style = "number"; // "number" or "word"

	/// <summary>
	/// Start.
	/// </summary>
	void Start () {

	}

	/// <summary>
	/// Update.
	/// </summary>
	void Update () {

		System.DateTime now = System.DateTime.Now;
		if(style == "number"){
			string datePatt = @"M/d/yyyy hh:mm:ss tt";
			string dtString = now.ToString(datePatt);
			this.GetComponent<Text>().text = dtString;
		}
		else{
			string day = now.DayOfWeek.ToString();
			string timeofday = "";
			int hour = now.Hour;

			if(hour > 5 && hour <= 11){
				timeofday = "Morning";
			}
			else if(hour > 11 && hour <= 14){
				timeofday = "Lunch Time";
			}
			else if(hour > 14 && hour <= 17){
				timeofday = "Afternoon";
			}
			else if(hour > 17 && hour <= 21){
				timeofday = "Evening";
			}
			else if((hour > 21 && hour <= 24) || (hour > 0 && hour <= 5)){
				timeofday = "Night";
			}
			else{
				timeofday = "error";
			}

			this.GetComponent<Text>().text = "It's " + day + " " + timeofday;
		}


	}
}
