using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	public bool aimUpgrade = false; //for auto aim assist upgrade
	public bool autoReloadUpgrade = false;
	public bool cyclopsUpgrade = false; //cyclops motor upgrade, to increase speed
	public GameObject[] defenseUpgradeList;
	public bool fieldUpgrade = false;
	public bool jetPackUpgrade = false;
	public GameObject jPScriptHolder;
	public int levelOverTime = 3; //time delay before next scene load if level won
	public bool missileUpgrade = false;
	public bool movementUpgrade = false; //used for gyroscopic movement upgrade (more acceleration)
	public string nextScene;
	public bool phlebotinumUpgrade = false;
	public PlayerControllerScript pcs; 	//one player
	public bool quadUpgrade = false;
	public bool reactiveArmorUpgrade = false;
	public bool repairUpgrade = false;
	public bool scramblerUpgrade = false;
	public UIScript ui; 	//ui on screen
	public GameObject[] weaponUpgradeList;

	private int enemyCount = 0;
	private bool scramblerFlag = false; //for scrambler

	void Start () {
		enemyCount = GameObject.FindGameObjectsWithTag ("Enemy").Length;
	}

	PlayerItemScript getItemScript(string upgradeType, string id) {
		if (upgradeType == "weapon") {
			foreach (GameObject i in weaponUpgradeList) {
				PlayerItemScript pis = i.GetComponent<PlayerItemScript> ();
				if (pis.id == id) {
					return pis;
				}
			}
		} else {
			foreach (GameObject i in defenseUpgradeList) {
				PlayerItemScript pis = i.GetComponent<PlayerItemScript> ();
				if (pis.id == id) {
					return pis;
				}
			}
		}
		return null; //need this else get "not all code paths return value error"
	}

	IEnumerator LevelOverTimer() {
		yield return new WaitForSeconds (levelOverTime);
		SceneManager.LoadScene (nextScene);
	}

	void setItemVal(string upgradeType, string id, bool val, bool setVal = true) {
		if (!setVal) {
			return;
		}
		PlayerItemScript pis = getItemScript (upgradeType, id);
		pis.unlocked = val; 
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

	public void decreaseEnemyCount() {
		enemyCount--;
		if (enemyCount == 0) {
			gameWon ();
		}
	}


	public void editModeItemLoad() { //for edit mode purposes only
		bool[] itemBools = new bool[] {scramblerUpgrade, fieldUpgrade, missileUpgrade, repairUpgrade, quadUpgrade};
		string[] itemIds = new string[] { "scrambler", "field", "missile", "repairKit", "quad"};
		string[] upgradeTypes = new string[] {"defense", "defense", "weapon", "defense", "weapon" };
		for (int i = 0; i < itemBools.Length; i++) {
			PlayerItemScript pis = getItemScript (upgradeTypes [i], itemIds [i]);
			pis.unlocked = itemBools [i];
		}
	}

	public void gameOver() { //player is destroyed separately before calling this
		GameObject[] oList = FindObjectsOfType<GameObject>();
		foreach (GameObject i in oList) {
			if (i.layer == 0) { //destroy all objects in default layer
				Destroy (i);
			}
		}
		ui.setGameOverText ();
		ui.turnOffAllButtons ();
		Camera.main.GetComponent<CameraController> ().enabled = false;
		Cursor.visible = true;
	}

	public void gameWon() {
		GameObject[] oList = FindObjectsOfType<GameObject>();
		foreach (GameObject i in oList) {
			if (i.layer == 0) { //destroy all objects in default layer
				Destroy (i);
			}
		}
		ui.setGameWonText ();
		ui.turnOffAllButtons ();
		pcs.enabled = false;
		Camera.main.GetComponent<CameraController> ().enabled = false;
		StartCoroutine (LevelOverTimer ());
	}

	public void loadSaveFile(bool setVal = true) { //setVal should be false because when called in UpgradeManager
		  //there's no item to set values to, only gm bools
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
				case "fieldUpgrade":
					fieldUpgrade = val;
					setItemVal ("defense", "field", fieldUpgrade, setVal); //string is returned, just didn't use it
					break;
				case "jetPackUpgrade":
					jetPackUpgrade = val;
					break;
				case "missileUpgrade":
					missileUpgrade = val;
					setItemVal ("weapon", "missile", missileUpgrade, setVal);
					break;
				case "movementUpgrade":
					movementUpgrade = val;
					break;
				case "phlebotinumUpgrade":
					phlebotinumUpgrade = val;
					break;
				case "quadUpgrade":
					quadUpgrade = val;
					setItemVal ("weapon", "quad", quadUpgrade, setVal);
					break;
				case "reactiveArmorUpgrade":
					reactiveArmorUpgrade = val;
					break;
				case "repairUpgrade":
					repairUpgrade = val;
					setItemVal ("defense", "repairKit", repairUpgrade, setVal);
					break;
				case "scramblerUpgrade":
					scramblerUpgrade = val;
					setItemVal ("defense", "scrambler", scramblerUpgrade, setVal);
					break;
				}
			}
		} else { //if file doesn't exist, write to file with default false bool
			string[] varNameList = new string[] {"aimUpgrade", "autoReloadUpgrade", "cyclopsUpgrade", "fieldUpgrade", "jetPackUpgrade", 
				"missileUpgrade", "movementUpgrade", "phlebotinumUpgrade", "quadUpgrade", "reactiveArmorUpgrade", "repairUpgrade", "scramblerUpgrade"};
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
				case "fieldUpgrade":
					fieldUpgrade = false;
					setItemVal ("defense", "field", fieldUpgrade, setVal);
					txt += "\t" + fieldUpgrade.ToString();
					break;
				case "jetPackUpgrade":
					jetPackUpgrade = false;
					txt += "\t" + jetPackUpgrade.ToString();
					break;
				case "missileUpgrade":
					missileUpgrade = false;
					setItemVal ("weapon", "missile", missileUpgrade, setVal);
					txt += "\t" + missileUpgrade.ToString ();
					break;
				case "movementUpgrade":
					movementUpgrade = false;
					txt += "\t" + movementUpgrade.ToString() ;
					break;
				case "phlebotinumUpgrade":
					phlebotinumUpgrade = false;
					txt += "\t" + phlebotinumUpgrade.ToString();
					break;
				case "quadUpgrade":
					quadUpgrade = false;
					txt += "\t" + phlebotinumUpgrade.ToString ();
					break;
				case "reactiveArmorUpgrade":
					reactiveArmorUpgrade = false;
					txt += "\t" + reactiveArmorUpgrade.ToString();
					break;
				case "repairUpgrade":
					repairUpgrade = false;
					setItemVal ("defense", "repairKit", repairUpgrade, setVal);
					txt += "\t" + repairUpgrade.ToString();
					break;
				case "scramblerUpgrade":
					scramblerUpgrade = false;
					txt += "\t" + scramblerUpgrade.ToString ();
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
				pcs.populateWeapon (i, pws.id); //unlike defense, need id to check quad override
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
				  //so do a manual turn off here for assurance
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
