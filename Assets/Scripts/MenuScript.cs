using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuScript : MonoBehaviour {
	public AudioSource menuMusic;
	public GameObject[] menuObjects;

	private GameManager gm;
	private bool menuIsOn;
	private PlayerControllerScript pcs = null;
	private AudioSource sceneMusic;

	// Use this for initialization
	void Start () {
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		if (player) {
			pcs = player.GetComponent<PlayerControllerScript> ();
		}
		gm = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ();
		sceneMusic = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<AudioSource> ();
		toggleMenu (false);

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			toggleMenu (!menuIsOn); //whatever is current menu status, do opposite of that
		}
	}

	public void toggleMenu(bool turnOn) {
		if (gm.lDone) { //if level done, prevent menu from showing up
			return;
		}

		if (turnOn) {
			foreach (GameObject i in menuObjects) {
				if (i.CompareTag ("MenuImage")) {
					i.GetComponent<Image> ().enabled = true;
				} else if (i.CompareTag ("MenuButton")) {
					i.GetComponent<Image> ().enabled = true;
					i.GetComponent<Button> ().enabled = true;
					i.GetComponentInChildren<Text> ().enabled = true;
				}
			}
			Time.timeScale = 0;
			menuMusic.Play ();
			sceneMusic.Pause ();

			menuIsOn = true;
			if (pcs) {
				pcs.menuOn = true;
			}

		} else {
			foreach (GameObject i in menuObjects) {
				if (i.CompareTag ("MenuImage")) {
					i.GetComponent<Image> ().enabled = false;
				} else if (i.CompareTag ("MenuButton")) {
					i.GetComponent<Image> ().enabled = false;
					i.GetComponent<Button> ().enabled = false;
					i.GetComponentInChildren<Text> ().enabled = false;
				}
			}
			Time.timeScale = 1;
			menuMusic.Stop ();
			sceneMusic.Play ();

			menuIsOn = false;
			if (pcs) {
				pcs.menuOn = false;
			}
		}
	}
}
