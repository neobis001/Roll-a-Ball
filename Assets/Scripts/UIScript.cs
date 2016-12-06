using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {
	public Text winText;
	public Text loseText;
	public Text ammoText;
	public Text healthText;

	public Color originalWeaponC; 
	public Color highlightedWeaponC;
	public Color disabledWeaponC;
	public Color originalDefenseC;
	public Color highlightedDefenseC;
	public Color disabledDefenseC;
	public Image[] weaponImages;
	public Image[] defenseImages; 

	// Use this for initialization
	void Start () {
		winText.text = "";
		loseText.text = "";

	}

	//pure code to change weapon UI buttons
	//planned to be called from gm
	public void changeWeaponIcon(int weaponIndex) {
		Image highlightedImg = weaponImages [weaponIndex];
		highlightedImg.color = highlightedWeaponC;
		foreach (Image i in weaponImages) {
			if (i != highlightedImg) {
				i.color = originalWeaponC;
			}
		}
	}

	//pure code to change defense UI buttons
	//planned to be called from gm
	public void changeDefenseIcon(string[] defenseIndexList) {
		for (int i = 0; i < defenseIndexList.Length; i++) {
			Image img = defenseImages [i];
			string colorCase = defenseIndexList [i];
			//Debug.Log ("The i value is " + i.ToString () + ". The list value is " + colorCase);
			switch (colorCase) {
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
/*		Image highlightedImg = defenseImages [defenseIndex];
		highlightedImg.color = highlightedDefenseC;
		foreach (Image i in defenseImages) {
			if (i != highlightedImg) {
				i.color = originalDefenseC;
			}
		}	*/
	}

	//set health
	public void setHealthText(string hlth) {
		healthText.text = "Health: " + hlth;
	}

	//set what's needed on game over
	public void setGameOverText() {
		loseText.text = "Game Over!";
		healthText.text = "";
		ammoText.text = "";
	}

	//set ammo
	public void setAmmoText(string txt) {
		ammoText.text = "Ammo: " + txt;
	}
}
