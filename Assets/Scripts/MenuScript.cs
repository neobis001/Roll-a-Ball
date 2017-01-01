using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {
	public AudioSource menuMusic;
	public GameObject[] menuObjects;

	private bool menuIsOn;
	private PlayerControllerScript pcs = null;
	private AudioSource sceneMusic;

	// Use this for initialization
	void Start () {
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		if (player) {
			pcs = player.GetComponent<PlayerControllerScript> ();
		}
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
		if (turnOn) {
			foreach (GameObject i in menuObjects) {
				i.SetActive (true);
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
				i.SetActive (false);
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
