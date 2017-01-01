using UnityEngine;
using System.Collections;

public class MenuButtonScript : MonoBehaviour {

	private MenuScript ms;

	void Start() {
		ms = GameObject.FindGameObjectWithTag ("Canvas").GetComponent<MenuScript> ();
	}

	public void quitGame() {
		Application.Quit ();
	}

	public void continueGame() {
		ms.toggleMenu (false);
	}

}
