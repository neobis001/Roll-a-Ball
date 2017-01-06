using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;

public class OkScript : MonoBehaviour {
	public AudioSource selectSound;

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
		selectSound.Play ();

		if (gm.nextScene == "exit") { //for testing purposes
			Application.Quit ();
		} else if (gm.nextScene == "write") {
			um.writeGmBool ();
			Debug.Log ("wrote to saveStats.txt");
		} else {
			um.writeGmBool ();
			File.WriteAllText ("saveLocation.txt", gm.nextScene);
			SceneManager.LoadScene (gm.nextScene);
		}

	}

}
