using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {
	public Text ammoText;
	public Color disabledDefenseC;
	public Color disabledWeaponC;
	public Text healthText;
	public Color highlightedDefenseC;
	public Color highlightedWeaponC;
	public Text loseText;
	public Color originalWeaponC;
	public Color originalDefenseC;
	public Vector2 startingDefenseLocation;
	public Vector2 startingWeaponLocation;
	public int defenseOffset;
	public int weaponOffset;
	public Text winText;

	private Vector2 currentDefenseLocation; 	//keeps track of location during defense placing
	private Vector2 currentWeaponLocation;
	private Image[] defenseImages; //size is initialized based on one set in SphereTank
	private Image[] weaponImages;

	// Use this for initialization
	void Start () {
		winText.text = "";
		loseText.text = "";
		currentDefenseLocation = startingDefenseLocation;
		currentWeaponLocation = startingWeaponLocation;
	}
		
	//planned to be called from gm
	//difference between weapons and defenses is that not considering disabling/re-enabling now
	public void changeWeaponIcon(int weaponIndex) {
		if (!weaponImages [weaponIndex]) {
			return;
		}
		Image highlightedImg = weaponImages [weaponIndex];
		highlightedImg.color = highlightedWeaponC;
		foreach (Image i in weaponImages) {
			if (!i) {
				continue;
			} else if (i != highlightedImg) { //if not highlightedImg, change to original color
				i.color = originalWeaponC;
			}
		}
	}

	//planned to be called from gm
	public void changeDefenseIcon(string[] defenseIndexList) {
		for (int i = 0; i < defenseIndexList.Length; i++) {
			Image img = defenseImages [i];
			string colorCase = defenseIndexList [i];

			switch (colorCase) {
			case "nothing":
				break;
			case "activeEnabled":
				//Debug.Log ("in activeEnabled case");
				img.color = highlightedDefenseC;
				break;
			case "inactiveEnabled":
				//Debug.Log ("in inactiveEnabled case");
				img.color = originalDefenseC;
				break;
			case "inactiveDisabled":
				//Debug.Log ("in inactiveDisabled case");
				img.color = disabledDefenseC;
				break;
			}
		}
	}

	public void initializeDefeneseList(int length) {
		defenseImages = new Image[length];
	}

	public void initializeWeaponList(int length) {
		weaponImages = new Image[length];
	}

	//populate defense UI stuff, and add to defense images list for later
	//position images based on an offset
	public void populateDefenseUI(GameObject button) {
		button.SetActive (true); //by default, all buttons are inactive
		button.GetComponent<RectTransform> ().anchoredPosition = currentDefenseLocation;
		currentDefenseLocation.x += defenseOffset;
		Image img = button.GetComponent<Image> ();
		for (int i = 0; i < defenseImages.Length; i++) {
			if (!defenseImages [i]) {
				defenseImages [i] = img;
				break;
			}
		}
	}

	public void populateWeaponUI(GameObject button) {
		button.SetActive (true);
		button.GetComponent<RectTransform> ().anchoredPosition = currentWeaponLocation;
		currentWeaponLocation.x += weaponOffset;
		Image img = button.GetComponent<Image> ();
		for (int i = 0; i < weaponImages.Length; i++) {
			if (!weaponImages [i]) {
				weaponImages [i] = img;
				break;
			}
		}
	}


	public void setAmmoText(string txt) {
		ammoText.text = "Ammo: " + txt;
	}

	public void setHealthText(string hlth) {
		healthText.text = "Health: " + hlth;
	}
		
	public void setGameOverText() {
		loseText.text = "Game Over!";
		healthText.text = "";
		ammoText.text = "";
	}

}
