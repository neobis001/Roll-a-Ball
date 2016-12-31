using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class OkScript : MonoBehaviour {

	private GameManager gm;
	private UpgradeManager um;

	void Start() {
		gm = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ();
		um = GameObject.FindGameObjectWithTag ("UpgradeManager").GetComponent<UpgradeManager> ();
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
