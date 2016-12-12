using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {
	public bool autoReloadUpgrade = false;
	public GameObject[] defenseUpgradeList;
	public bool gameIsOver; 	//the purpose of this flag is that so other scripts can see this and stop running when needed
	public PlayerControllerScript pcs; 	//one player
	public bool phlebotinumUpgrade = false;
	public UIScript ui; 	//ui on screen
	public GameObject[] weaponUpgradeList;

	private bool scramblerFlag = false; //for scrambler

	void Start () {
		gameIsOver = false;
	}
		
	public void setHealthText(string hlth) {
		ui.setHealthText (hlth);
	}


	public void gameOver() {
		GameObject player = pcs.gameObject; //include player stuff here
		Destroy (player);
		ui.setGameOverText ();
		Camera.main.GetComponent<CameraController> ().enabled = false;
		Cursor.visible = true;
		
	}

	
	//messenger
	public void changeWeaponIcon(int weaponIndex) {	
		ui.changeWeaponIcon (weaponIndex);
	}
	
	//messenger
	public void changeDefenseIcon(string[] defenseIndexList) {
		ui.changeDefenseIcon(defenseIndexList);
	}
	
	//messenger
	public void setAmmoText(string txt) {
		ui.setAmmoText (txt);
	}
		
	//the difference between weapons and defenses is that defenses have one more layer of scripts
	public void populateWeaponUpgrades() {
		foreach (GameObject i in weaponUpgradeList) {
			PlayerWeaponScript pws = i.GetComponent<PlayerWeaponScript> ();
			if (pws.unlocked) {
				pcs.populateWeapon (i);
			} else {
				pws.gameObject.SetActive (false);
			}
		}
		ui.initializeWeaponList (pcs.weaponList.Length);
	}
		
	//planned to be called from player
	public void populateDefenseUpgrades() {
		foreach (GameObject i in defenseUpgradeList) {
			PlayerDefenseScript pds = i.GetComponent<PlayerDefenseScript> ();
			if (pds.unlocked) {
				pcs.populateDefense (i);
			}
		}
		ui.initializeDefeneseList (pcs.defenseList.Length);
	}

	//messenger
	public void populateDefenseUI(GameObject button) {
		ui.populateDefenseUI (button);
	}

	//messenger
	public void populateWeaponUI(GameObject button) {
		ui.populateWeaponUI (button);
	}


	//to access scramblerFlag
	public bool sFlag {
		get {return scramblerFlag; }
		set {scramblerFlag = value; }
	}
		
}
