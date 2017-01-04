using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//in this code buttons is more Images, but just using it to differentiate from other UI graphics
public class UIScript : MonoBehaviour {
	//public Text ammoText;
	public int defenseOffset;
	public Color disabledDefenseC;
	public Color disabledJetC;
	public Color disabledWeaponC;
	public Text currentHealth;
	public GameObject[] gameOverButtons;
	public RectTransform healthBar;
	public Color highlightedDefenseC;
	public Color highlightedWeaponC;
	public Text loseText;
	public Color originalDefenseC;
	public Color originalJetC;
	public Color originalWeaponC;
	//public Vector2 startingDefenseLocation;
	//public Vector2 startingWeaponLocation;
	public Text slashBar;
	public Text totalHealth;
	public int weaponOffset;
	public Text winText;

	//private Vector2 currentDefenseLocation; 	//keeps track of location during defense placing
	//private Vector2 currentWeaponLocation;
	private Text currentAmmoText;
	private Image currentWeaponImage = null;
	private Image jetImage; //only one needed
	private Image[] defenseImages; //size is initialized based on one set in SphereTank
	private int originalHealth;
	private float originalHealthBarWidth;
	private Text[] weaponAmmoTexts;
	private Image[] weaponImages;

	// Use this for initialization
	void Start () {
		winText.text = "";
		loseText.text = "";
		/*currentDefenseLocation = startingDefenseLocation;
		currentWeaponLocation = startingWeaponLocation;*/

		foreach (GameObject i in gameOverButtons) {
			i.SetActive (false);
		}
	}

	//planned to be called from gm
	//difference between weapons and defenses is that not considering disabling/re-enabling now
	public void changeWeaponIcon(int weaponIndex) {
		if (!weaponImages [weaponIndex]) {
			return;
		}
			
		currentWeaponImage = weaponImages [weaponIndex];
		currentWeaponImage.color = highlightedWeaponC;
		currentAmmoText = weaponAmmoTexts [weaponIndex];
		currentAmmoText.gameObject.SetActive(true);


		for (int i = 0; i < weaponImages.Length; i++) { //assumed weaponImages.Length == weaponAmmoImages.Length
			Image ioi = weaponImages[i];
			if (!ioi) {
				continue;
			} else if (ioi != currentWeaponImage) { //if not currentWeaponImage, change to original color
				ioi.color = originalWeaponC;
				weaponAmmoTexts [i].gameObject.SetActive (false);
			}
		}
		/*
		foreach (Image i in weaponImages) {
			if (!i) {
				continue;
			} else if (i != currentWeaponImage) { //if not currentWeaponImage, change to original color
				i.color = originalWeaponC;
				i.GetComponentInChildren<Text> ().gameObject.SetActive (false);  //assumes has only one text object child 
			}
		} */
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

	public void changeJetIcon(bool isOn) {
		if (isOn) { //isOn is for if a new jet pack can be used or not
			jetImage.color = originalJetC;
		} else {
			jetImage.color = disabledJetC;
		}
	}

	public void initializeDefeneseList(int length) {
		defenseImages = new Image[length];
	}

	public void initializeHealth(int startingHealth) {
		originalHealthBarWidth = healthBar.rect.width;
		originalHealth = startingHealth;
		totalHealth.text = startingHealth.ToString ();

		setHealth (originalHealth);
	}

	public void initializeWeaponList(int length) {
		weaponImages = new Image[length];
		weaponAmmoTexts = new Text[length];
	}

	//populate defense UI stuff, and add to defense images list for later
	//position images based on an offset
	public void populateDefenseUI(GameObject upgrade, int keyNumber) {
		upgrade.SetActive (true); //by default, all buttons are inactive
		/*upgrade.GetComponent<RectTransform> ().anchoredPosition = currentDefenseLocation;
		currentDefenseLocation.x += defenseOffset; */
		Image img = upgrade.GetComponent<Image> ();
		/*for (int i = 0; i < defenseImages.Length; i++) {
			if (!defenseImages [i]) {
				defenseImages [i] = img;
				break;
			}
		} */
		defenseImages [keyNumber - 1] = img; //keys start at 1 
	}

	//unlike weapon and defense, this is only one button, so don't need much more code for this, and it's a bit different
	public void populateJetUI(GameObject upgrade) {
		jetImage = upgrade.GetComponent<Image> ();
		upgrade.SetActive (true);
		changeJetIcon (true);
	}

	public void populateWeaponUI(GameObject upgrade) {
		upgrade.SetActive (true);
		/*upgrade.GetComponent<RectTransform> ().anchoredPosition = currentWeaponLocation;
		currentWeaponLocation.x += weaponOffset; */
		Image img = upgrade.GetComponent<Image> ();
//		Debug.Log ("weapon image " + img.ToString ());
		Text ammoText = upgrade.GetComponentInChildren<Text> (includeInactive: true);
//		Debug.Log ("weapon ammo " + ammoText.ToString());
		for (int i = 0; i < weaponImages.Length; i++) {
			if (!weaponImages [i]) {
				weaponImages [i] = img;
				weaponAmmoTexts [i] = ammoText;
				break;
			}
		}
	}

	public void setAmmoReload(string txt, bool doneReloading) {
		//Debug.Log (currentWeaponImage);
		if (doneReloading) {
			currentWeaponImage.color = highlightedWeaponC;
		} else {
			currentWeaponImage.color = disabledWeaponC;
		}

		setAmmoText (txt);
	}

	public void setAmmoText(string txt) {
		//currentAmmoText.text = "Ammo: " + txt;
		currentAmmoText.text = txt;
	}

	public void setHealth(int hlth) {
		//Debug.Log (hlth.ToString() + " " + originalHealth.ToString() + " " + originalHealthBarWidth.ToString());
		healthBar.sizeDelta = new Vector2 ((int)( (float) hlth / originalHealth * originalHealthBarWidth) , healthBar.sizeDelta.y); //modify width only, not height
		 //have to float health first, else first division goes to 0 if dividing the two ints hlth < originalHealth

		currentHealth.text = hlth.ToString ();
	}
		
	public void setGameOverUi() {
		loseText.text = "Game Over!";
		currentHealth.text = "";
		totalHealth.text = "";
		currentAmmoText.text = "";
		slashBar.text = "";

		foreach (GameObject i in gameOverButtons) {
			i.SetActive (true);
		}
	}

	public void setGameWonText() {
		winText.text = "Level Complete!";
		currentHealth.text = "";
		totalHealth.text = "";
		currentAmmoText.text = "";
		slashBar.text = "";
	}

	public void cleanUpUi() { //most images in UI
		Image[] blist = GetComponentsInChildren<Image> (); //assumes script is in parent that carries button children
		foreach (Image i in blist) {
			i.gameObject.SetActive (false);
		}
	}

}
