using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundHandler : MonoBehaviour {

	private Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();

	}

	// Update is called once per frame
	void Update () {

	}

	public int SetWeather(string _weather){

		anim.SetTrigger(_weather);
		return 1;
	}
}
