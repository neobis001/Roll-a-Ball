using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//current organization so far
// GameManager is connected to PlayerController
// PlayerController is connected to various WeaponScript.cs instances


public class GameManager : MonoBehaviour {
	public Text winText;
	public Text loseText;
	public Text ammoText;
	public int health = 100;
	public Text healthText;
	public bool gameisOver; //the purpose of this flag is that so other scripts can see this and stop running when needed


	public GameObject turret; // could replace with an array of GameObjects if needed, C# is way more flexible than C++ with arrays
	public PlayerControllerScript pc;

	public Color originalWeaponC; //this and stuff below is for buttons
	public Color highlightedWeaponC;
	public Color originalDefenseC;
	public Color highlightedDefenseC;
	public Image[] weaponImages;
	public Image[] defenseImages; 

	private int weaponIndex = 0;
	private int defenseIndex = 0;

	void Start () {
		winText.text = "";
		loseText.text = "";
		healthText.text = "Health: " + health.ToString ();
		gameisOver = false;

		weaponIndex = 0;
		defenseIndex = 0;
		pc.changeWeapon (weaponIndex);
		pc.changeDefense (defenseIndex);
		ammoText.text = "Ammo: " + pc.currentWeaponScript.ammo.ToString (); //put after changeWeapon/changeDefense gets script

		changeWeaponIcon ();
		changeDefenseIcon ();
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

		if (Input.GetKeyDown (KeyCode.Q)) {
			weaponIndex--;
			if (weaponIndex < 0) {
				weaponIndex = pc.turretList.Length - 1;
			}
			pc.changeWeapon (weaponIndex);
			changeWeaponIcon ();
		} else if (Input.GetKeyDown (KeyCode.E)) {
			weaponIndex++;
			if (weaponIndex == pc.turretList.Length) {
				weaponIndex = 0;
			}
			pc.changeWeapon (weaponIndex);
			changeWeaponIcon ();
		} else if (Input.GetKeyDown(KeyCode.R)) {
			pc.currentWeaponScript.setAmmo("r");
			pc.currentWeaponScript.playReload ();
		}

		//TEMPORARY CODE, change this with a defenseList list later, and edit changeDefese to work with defenseList
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			defenseIndex--;
			if (defenseIndex < 0) {
				defenseIndex = defenseImages.Length - 1;
			}
			pc.changeDefense (defenseIndex);
			changeDefenseIcon ();
		} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
			defenseIndex++;
			if (defenseIndex == defenseImages.Length) {
				defenseIndex = 0;
			}
			pc.changeDefense (defenseIndex);
			changeDefenseIcon ();
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

	void changeWeaponIcon() {
		Image highlightedImg = weaponImages [weaponIndex];
		highlightedImg.color = highlightedWeaponC;
		foreach (Image i in weaponImages) {
			if (i != highlightedImg) {
				i.color = originalWeaponC;
			}
		}	
	}

	void changeDefenseIcon() {
		Image highlightedImg = defenseImages [defenseIndex];
		highlightedImg.color = highlightedDefenseC;
		foreach (Image i in defenseImages) {
			if (i != highlightedImg) {
				i.color = originalDefenseC;
			}
		}	
	}
}
