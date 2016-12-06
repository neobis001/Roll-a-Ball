using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//current organization so far
// GameManager is connected to PlayerController
// PlayerController is connected to various WeaponScript.cs instances

//12/4/16
//make all items all part of a superclass with a function that sets a flag saying whether to disable/enable an item
//in order to do this, all derived classes should stay active, it should be a private bool and one function

//goals: disabling items
//when an item is disabled, it should be made possible to reenable it
//when an item is disabled, the highlighted item should now be the next one on the right
//if there are no items available, don't do anything. wait til an item is highlighted
//if multiple items are highlighted at the same time, choose the most left item


public class GameManager : MonoBehaviour {
//	public Text winText;
//	public Text loseText;
//	public Text ammoText;
//	public int health = 100;
//	public Text healthText;
	public bool gameisOver; //the purpose of this flag is that so other scripts can see this and stop running when needed

//	public GameObject turret; // could replace with an array of GameObjects if needed, C# is way more flexible than C++ with arrays
	public PlayerControllerScript pc;
	public UIScript ui;

//	public Color originalWeaponC; //this and stuff below is for buttons
//	public Color highlightedWeaponC;
//	public Color originalDefenseC;
//	public Color highlightedDefenseC;
//	public Image[] weaponImages;
//	public Image[] defenseImages; 

//	private int weaponIndex = 0;
//	private int defenseIndex = 0;

	void Start () {
		gameisOver = false;
	}
		
	//call ui's setHealthText to do ui health code stuff
	public void setHealthText(string hlth) {
		ui.setHealthText (hlth);
	}
	
	//manage all the Game Over stuff in general for everything
	//player game over stuff included here as well
	public void gameOver() {
		GameObject player = pc.gameObject;
		Destroy (player);
		ui.setGameOverText ();
		Camera.main.GetComponent<CameraController> ().enabled = false;
		Cursor.visible = true;
		
	}

	
	//call ui's changeWeaponIcon to do the actual ui code stuff
	public void changeWeaponIcon(int weaponIndex) {	
		ui.changeWeaponIcon (weaponIndex);
	}
	
	//call ui's changeDefenseIcon to do the actual ui code stuff
	public void changeDefenseIcon(string[] defenseIndexList) {
		ui.changeDefenseIcon(defenseIndexList);
	}
	
	//call ui's setAmmoText to do actual ui ammo stuff
	public void setAmmoText(string txt) {
		ui.setAmmoText (txt);
	}

}
