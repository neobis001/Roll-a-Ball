﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class OkScript : MonoBehaviour {

	private Button btn;
	private GameManager gm;
	private UpgradeManager um;

	void Start() {
		gm = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ();
		um = GameObject.FindGameObjectWithTag ("UpgradeManager").GetComponent<UpgradeManager> ();
	}

	public void setUpButton() { //so button is set up before interactable check
		btn = GetComponent<Button> ();
	}

	public void setUpInteractable(bool isOn) {
		if (isOn) {
			btn.interactable = true;
		} else {
			btn.interactable = false;
		}
	}

	public void toNextScene() {
		if (gm.nextScene == "exit") { //for testing purposes
			Application.Quit ();
		} else if (gm.nextScene == "write") {
			um.writeGmBool ();
			Debug.Log ("wrote to save.txt");
		} else {
			um.writeGmBool ();
			SceneManager.LoadScene (gm.nextScene);
		}
	}

}
