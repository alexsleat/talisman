using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sunmoon : MonoBehaviour {

	private int old_time = 0;
	private GameObject sun;
	private GameObject moon;

	/// <summary>
	/// Start.
	/// </summary>
	void Start () {

		sun = GameObject.Find("sun");
		moon = GameObject.Find("moon");

	}

	/// <summary>
	/// Update.
	/// </summary>
	void Update () {

		System.DateTime now = System.DateTime.Now;
		int hour = now.Hour;
		int min = now.Minute;

		if(hour >= 5 && hour <= 19){
			sun.SetActive(true);
			moon.SetActive(false);
		}
		else{
			sun.SetActive(false);
			moon.SetActive(true);
		}

		float rot = ( ((hour * 15)*-1) + 90) + (min * -0.25F) ;

		transform.rotation = Quaternion.Euler(0, 0, rot);
		old_time = hour;


	}
}
