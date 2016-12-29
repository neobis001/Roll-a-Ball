using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class GameManager : MonoBehaviour {
	public bool aimUpgrade = false; //for auto aim assist upgrade
	public bool autoReloadUpgrade = false;
	public bool cyclopsUpgrade = false; //cyclops motor upgrade, to increase speed
	public GameObject[] defenseUpgradeList;
	public bool gameIsOver; 	//the purpose of this flag is that so other scripts can see this and stop running when needed
	public bool jetPackUpgrade = false;
	public GameObject jPScriptHolder;
	public bool movementUpgrade = false; //used for gyroscopic movement upgrade (more acceleration)
	public bool phlebotinumUpgrade = false;
	public PlayerControllerScript pcs; 	//one player
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

	public void gameOver() { //player is destroyed separately before calling this
		GameObject[] oList = FindObjectsOfType<GameObject>();
		foreach (GameObject i in oList) {
			if (i.layer == 0) { //destroy all objects in default layer
				Destroy (i);
			}
		}
		ui.setGameOverText ();
		Camera.main.GetComponent<CameraController> ().enabled = false;
		Cursor.visible = true;

	}

	public void loadSaveFile() {
		return;
		Debug.Log ("attempting to load save file");
		if (File.Exists ("save.txt")) {
			string[] upList = File.ReadAllLines ("save.txt");
			foreach (string i in upList) {
				char[] tabSplit = new char[]{ '\t' };
				string[] inpt = i.Split (tabSplit);
				string varname = inpt [0];
				string textVal = inpt [1];
				bool val = false;
				if (textVal == "True") { //writing bools give capital True/False
					val = true;
				} else {
					val = false;
				}

				switch (varname) {
				case "aimUpgrade":
					aimUpgrade = val;
					break;
				case "autoReloadUpgrade":
					autoReloadUpgrade = val;
					break;
				case "cyclopsUpgrade":
					cyclopsUpgrade = val;
					break;
				case "jetPackUpgrade":
					jetPackUpgrade = val;
					break;
				case "movementUpgrade":
					movementUpgrade = val;
					break;
				case "phlebotinumUpgrade":
					phlebotinumUpgrade = val;
					break;
				case "reactiveArmorUpgrade":
					reactiveArmorUpgrade = val;
					break;
				}
			}
		} else { //if file doesn't exist, write to file with default false bool
			string[] varNameList = new string[] {"aimUpgrade", "autoReloadUpgrade", "cyclopsUpgrade", "jetPackUpgrade", 
				"movementUpgrade", "phlebotinumUpgrade", "reactiveArmorUpgrade"};
			foreach (string i in varNameList) {
				string txt = i; //can't augment directly to i
				switch (txt) {
				case "aimUpgrade":
					aimUpgrade = false;
					txt += "\t" + aimUpgrade.ToString();
					break;
				case "autoReloadUpgrade":
					autoReloadUpgrade = false;
					txt += "\t" + autoReloadUpgrade.ToString();
					break;
				case "cyclopsUpgrade":
					cyclopsUpgrade = false;
					txt += "\t" + cyclopsUpgrade.ToString();
					break;
				case "jetPackUpgrade":
					jetPackUpgrade = false;
					txt += "\t" + jetPackUpgrade.ToString();
					break;
				case "movementUpgrade":
					movementUpgrade = false;
					txt += "\t" + movementUpgrade.ToString() ;
					break;
				case "phlebotinumUpgrade":
					phlebotinumUpgrade = false;
					txt += "\t" + phlebotinumUpgrade.ToString();
					break;
				case "reactiveArmorUpgrade":
					reactiveArmorUpgrade = false;
					txt += "\t" + reactiveArmorUpgrade.ToString();
					break;
				}
				File.AppendAllText ("save.txt", txt + "\n");
			}
		}
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
				pds.aFlag = false; //in the case where the upgrade isn't active, it won't be affected by player on changeDefesne
				  //changeDefense only affects aFlag on item it switches to and nothing else
				  //it does affect all eFlags though
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
