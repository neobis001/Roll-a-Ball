using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//current organization so far
// GameManager is connected to PlayerController
// PlayerController is connected to various WeaponScript.cs instances


public class GameManager : MonoBehaviour {
	public Text winText;
	public Text loseText;
	public int ammo = 5;
	public Text ammoText;
	public PlayerController pc;
	public int health = 100;
	public Text healthText;
	public bool gameisOver; //the purpose of this flag is that so other scripts can see this and stop running when needed
	public GameObject turret; // could replace with an array of GameObjects if needed, C# is way more flexible than C++ with arrays

	// Use this for initialization
	void Start () {
		winText.text = "";
		loseText.text = "";
		healthText.text = "Health: " + health.ToString ();
		ammoText.text = "Ammo: " + ammo.ToString ();
		gameisOver = false;

	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0 && gameisOver == false) {
			gameOver ();
			gameisOver = true;
		}

		if (pc.currentWeaponScript.ammo == 0) {
			ammoText.text = "Ammo: RELOAD";
		} else {
			ammoText.text = "Ammo: " + pc.currentWeaponScript.ammo.ToString ();
		}
	}

	public void changeHealth(int h) {
		health += h;
		healthText.text = "Health: " + health.ToString ();
	}

	public void gameOver() {
		GameObject player = pc.gameObject;
		Destroy (player);
		Destroy (turret);
		Camera.main.GetComponent<CameraController> ().enabled = false;
		loseText.text = "Game Over!";
		healthText.text = "";
		ammoText.text = "";
		Cursor.visible = true;
	}
}
