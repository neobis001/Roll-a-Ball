using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuButtonScript : MonoBehaviour {

	private MenuScript ms;
	private string currentScene;

	void Start() {
		ms = GameObject.FindGameObjectWithTag ("Canvas").GetComponent<MenuScript> ();
		currentScene = SceneManager.GetActiveScene ().name;
	}

	public void continueGame() {
		ms.toggleMenu (false);
	}

	public void quitGame() {
		Debug.Log ("loading2");
		Application.Quit ();
	}

	public void restartLevel() {
		Debug.Log ("loading");
		SceneManager.LoadScene (currentScene);
	}
		



}
