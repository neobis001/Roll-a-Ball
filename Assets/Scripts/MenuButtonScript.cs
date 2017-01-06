using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuButtonScript : MonoBehaviour {

	public AudioSource selectSound; //grab from prefab

	private MenuScript ms;
	private string currentScene;

	void Start() {
		ms = GameObject.FindGameObjectWithTag ("Canvas").GetComponent<MenuScript> ();
		currentScene = SceneManager.GetActiveScene ().name;
	}

	public void continueGame() {
		ms.toggleMenu (false);
		selectSound.Play ();
	}

	public void quitGame() {
		Debug.Log ("loading2");
		selectSound.Play ();
		Application.Quit ();
	}

	public void restartLevel() {
		Debug.Log ("loading");
		selectSound.Play ();
		SceneManager.LoadScene (currentScene);
	}
		



}
