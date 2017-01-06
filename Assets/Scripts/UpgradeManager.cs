using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class UpgradeManager : MonoBehaviour {

	public CanvasScaler cs;
	public GameObject[] descriptionImages;
	public Text descriptionMain;
	public Text descriptionStats;
	public Text descriptionTitle;
	public Text descriptionType;
	public AudioSource hoverSound;
	public AudioSource toggleSound;
	public OkScript okButton;
	public Color originalNormalC; //plan is to make normal and highlighted color the same
	//whether toggled on or off 
	public Color notInteractableC; //not interactable implies upgrade already there
	public Color selectionNotSelectedC;
	public Color selectionSelectedC; 
	public UpgradeButton[] upgradeButtons;

	private GameManager gm;

	// Use this for initialization
	void Start () {
		Cursor.visible = true;
		gm = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ();
		gm.loadSaveFile (setVal: false);
		okButton.setUpButton (); //done here before checkOk()
		turnOffDescription();
		
		foreach (UpgradeButton i in upgradeButtons) {
			i.startSetUp ();
			if (getGmBool (i.gmBool)) { //if gmBool gives true, that means already upgraded
				  //so want to turn off interactivity here
				i.setUpInteractable (false);
			} else {
				i.setUpInteractable (true);
			}
			i.toggle ("normal");
		}


		checkOk ();
	}

	/*
	void Update() {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			cycleSelection ("normal", "none");
		}
	} */

	public void checkOk() {
		int numInteractable = 0;
		foreach (UpgradeButton i in upgradeButtons) {
			if (!i.interactable) {
				numInteractable++;
			}
			if (i.selSelected == "selectionSelected") {
				okButton.setUpInteractable (true);
				return;
			}
		}

		if (numInteractable == upgradeButtons.Length) { //if no button is interactable, that means all upgrades gotten
			  //so make button clickable
			  //an alternative would be to cycle through gm, optional but more direct
			okButton.setUpInteractable (true);
			return;
		}

		okButton.setUpInteractable (false);
	}

	public void cycleSelection(string mode, string gmBool) { //"gmBool = "none"" means all buttons are changed to "mode"
		foreach (UpgradeButton i in upgradeButtons) {
			if (i.gmBool != gmBool) {
				i.toggle (mode);
			}
		}
	}

	public bool getGmBool(string gmBool) { //this is assuming loadSaveFile already ran
		bool res = false;
		switch (gmBool) {
		case "aimUpgrade":
			res = gm.aimUpgrade;
			break;
		case "autoReloadUpgrade":
			res = gm.autoReloadUpgrade;
			break;
		case "cyclopsUpgrade":
			res = gm.cyclopsUpgrade;
			break;
		case "fieldUpgrade":
			res = gm.fieldUpgrade;
			break;
		case "jetPackUpgrade":
			res = gm.jetPackUpgrade;
			break;
		case "missileUpgrade":
			res = gm.missileUpgrade;
			break;
		case "movementUpgrade":
			res = gm.movementUpgrade;
			break;
		case "phlebotinumUpgrade":
			res = gm.phlebotinumUpgrade;
			break;
		case "quadUpgrade":
			res = gm.quadUpgrade;
			break;
		case "reactiveArmorUpgrade":
			res = gm.reactiveArmorUpgrade;	
			break;
		case "repairUpgrade":
			res = gm.repairUpgrade;
			break;
		case "scramblerUpgrade":
			res = gm.scramblerUpgrade;
			break;
		default:
			Debug.LogWarning ("switch statement in getGmBool didn't read anything");
			break;
		}
		return res;
	}

	public void playHover() {
		hoverSound.Play ();
	}

	public void playToggle() {
		toggleSound.Play ();
	}

	public void turnOffDescription() {
		foreach (GameObject i in descriptionImages) {
			i.SetActive (false);
		}
		descriptionTitle.text = "";
		descriptionType.text = "";
		descriptionStats.text = "";
		descriptionMain.text = "";
	}

	public void turnOnDescription(string title, string type, string stats, string main) {
		foreach (GameObject i in descriptionImages) {
			i.SetActive (true);
		}
		descriptionTitle.text = title;
		descriptionType.text = type;
		descriptionStats.text = stats;
		descriptionMain.text = main;

	}

	public void writeGmBool() {
		string[] varNameList = new string[] {"aimUpgrade", "autoReloadUpgrade", "cyclopsUpgrade", "fieldUpgrade", "jetPackUpgrade", 
			"missileUpgrade", "movementUpgrade", "phlebotinumUpgrade", "quadUpgrade", "reactiveArmorUpgrade", "repairUpgrade", "scramblerUpgrade"};
		string res = "";

		foreach (string i in varNameList) {
			foreach (UpgradeButton j in upgradeButtons) {
				if (j.gmBool == i) {
					if (j.selSelected == "selectionSelected") {
						res += i + "\t" + "True\n";
					} else { //if not selectionSelected, just write everything to default value
						res += i + "\t" + getGmBool (i).ToString () + "\n";
					}
					break;
				}
			}
		}
		File.WriteAllText ("saveStats.txt", res);
	}

}