using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class faceController : MonoBehaviour {


	public string lastMsg = "";
	private string currentMsg = "Notify:Hello World!";

	void Start () {




	}

// Update is called once per frame
	void Update () {

		if(lastMsg != currentMsg){
			// If it's a notification it will start with Notify, and the second half
			// / of the message will be the message of the notification
			if(currentMsg.StartsWith("Notify:")){

				string msg = currentMsg.Replace("Notify:", "");
				GameObject.Find("face").GetComponent<faceHandler>().SetNotification(msg);
				Debug.Log("Notification: " + msg);

				lastMsg = currentMsg;

			}
		}
		

	}

}
