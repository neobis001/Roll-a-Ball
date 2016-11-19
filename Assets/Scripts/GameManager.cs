using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {
	public Text winText;
	public Text loseText;
	public int ammo = 5;
	public Text ammoText;
	public PlayerController pc;
	public int health = 100;
	public Text healthText;
	public bool gameisOver; //the purpose of this flag is that so other scripts can see this and stop running when needed
	public GameObject turret;

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
	}

	public void setAmmo(string flag) {
		if (flag == "d") {
			ammo--;
			ammoText.text = "Ammo: " + ammo.ToString ();	
		} else if (flag == "r") {
			ammo = 5;
			ammoText.text = "Ammo: " + ammo.ToString ();	
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
