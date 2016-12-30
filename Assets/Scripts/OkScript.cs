using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class OkScript : MonoBehaviour {

	private GameManager gm;

	void Start() {
		gm = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ();
	}

	public void toNextScene() {
		if (gm.nextScene == "exit") { //for testing purposes
			Application.Quit ();
		} else {
			SceneManager.LoadScene (gm.nextScene);
		}
	}

}
