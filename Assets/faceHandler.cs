using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class faceHandler : MonoBehaviour {

	private Animator anim;
	private GameObject speechBubble;
	private GameObject notifyButton;

	Sprite[] IconAtlas;

	List<string> IconNames = new List<string>() {	"netrual",
													"happy",
													"chuffed",
													"bleeh",
													"wink",
													"crying",
													"dazed",
													"squee",
													"confused",
													"angry",
													"startled",
													"cry",
													"blushing",
													"wasted",
													"more_wasted",
													"dont",
													"know",
													"anymore"};

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();

		IconAtlas = Resources.LoadAll<Sprite>("expressions");

		notifyButton = GameObject.Find("NotifyButton");
		speechBubble = GameObject.Find("speech_bubble");
		notifyButton.SetActive(false);
		speechBubble.SetActive(false);
		GameObject.Find("NotificationText").gameObject.GetComponent<Text>().text = "";
	}

	// Update is called once per frame
	void Update () {

	}

	public int SetFaceTrigger(string _animName){

		Debug.Log("SetFaceTrigger: " + _animName);
		speechBubble.SetActive(false);
		GameObject.Find("NotificationText").gameObject.GetComponent<Text>().text = "";
		

		//anim.SetTrigger(_animName);
		// string iconName= IconNames[_animName];
		bool exists = false;
		for (int i = 0; i < IconNames.Count; i++) {
			if(IconNames[i] == _animName)
			exists = true;
		}
     
	 	if(exists){
			int iconIndex = IconNames.IndexOf(_animName);
			GetComponent<SpriteRenderer>().sprite = IconAtlas[iconIndex];

			if(_animName.Equals("Dead")){
				speechBubble.SetActive(true);
				GameObject.Find("NotificationText").gameObject.GetComponent<Text>().text = "Something has gone wrong!";
			}

			return 1;
		}

		return -1;

	}

	public int SetNotification(string _input){

		Debug.Log(_input);

		speechBubble.SetActive(true);
		GameObject.Find("NotificationText").gameObject.GetComponent<Text>().text = "";
		anim.SetTrigger("Notify");
		GameObject.Find("NotificationText").gameObject.GetComponent<Text>().text = _input;
		notifyButton.SetActive(true);
		return 1;
	}

	public void ClearNotification(){

		speechBubble.SetActive(false);
		GameObject.Find("NotificationText").gameObject.GetComponent<Text>().text = "";
		anim.SetTrigger("NotifyRead");
		notifyButton.SetActive(false);
	}


}
