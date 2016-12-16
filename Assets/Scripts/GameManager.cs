using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {
	public bool aimUpgrade = false; //for auto aim assist upgrade
	public bool autoReloadUpgrade = false;
	public GameObject[] defenseUpgradeList;
	public bool gameIsOver; 	//the purpose of this flag is that so other scripts can see this and stop running when needed
	public GameObject jPScriptHolder;
	public bool jetPackUpgrade = false;
	public PlayerControllerScript pcs; 	//one player
	public bool cyclopsUpgrade = false; //cyclops motor upgrade, to increase speed
	public bool movementUpgrade = false; //used for gyroscopic movement upgrade (more acceleration)
	public bool phlebotinumUpgrade = false;
	public bool reactiveArmorUpgrade = false;
	public UIScript ui; 	//ui on screen
	public GameObject[] weaponUpgradeList;

	private bool scramblerFlag = false; //for scrambler

	void Start () {
		gameIsOver = false;
	}
		

	//messenger
	public void changeDefenseIcon(string[] defenseIndexList) {
		ui.changeDefenseIcon(defenseIndexList);
	}

	//messenger
	public void changeJetIcon(bool isOn) {
		ui.changeJetIcon (isOn);
	}

	//messenger
	public void changeWeaponIcon(int weaponIndex) {	
		ui.changeWeaponIcon (weaponIndex);
	}

	public void gameOver() {
		GameObject player = pcs.gameObject; //include player stuff here
		Destroy (player);
		ui.setGameOverText ();
		Camera.main.GetComponent<CameraController> ().enabled = false;
		Cursor.visible = true;

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
			} else {
				pds.aFlag = false; //in the case where the upgrade isn't active, it won't be affected by player on changeWeapon
				  //so do a manual turn off here
			}
		}
		ui.initializeDefeneseList (pcs.defenseList.Length);
	}

	//messenger
	public void populateDefenseUI(GameObject button) {
		ui.populateDefenseUI (button);
	}

	//messenger
	public void populateJetUI(GameObject button) {
		ui.populateJetUI (button);
	}

	//messenger
	public void populateWeaponUI(GameObject button) {
		ui.populateWeaponUI (button);
	}

	//messenger
	public void setAmmoText(string txt) {
		ui.setAmmoText (txt);
	}

	public void setHealthText(string hlth) {
		ui.setHealthText (hlth);
	}


	//to access scramblerFlag
	public bool sFlag {
		get {return scramblerFlag; }
		set {scramblerFlag = value; }
	}
		
}
