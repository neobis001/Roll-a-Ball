using UnityEngine;
using UnityEngine.UI;
using System.Collections;


//hold weapons/weapon switching here

//12/4/16
//when disabling an item, need it signal somehow for a switch in item highlight
//what about weapons, when disabling them, we should make an override function doing something about that
//same flag process though

//need to find a way to communicate to GameManager.cs that weaponIndex made a successful change
//and communicate a not successful one
//while at the same time, changing weapon

//get a list of all the available weapons
//if not even one of them has a "true can-be-active" flag, return a false bool
//else begin cycling through and trying to get the right weapon
//use ref to change weaponIndex argument in place

//then what's the bool for?
//if it's true, then we can set a changeWeaponIcon, else don't even try for it

//to do the enable/disable color stuff, we need to do update checks on all the items on every update frame

public class PlayerControllerScript: MonoBehaviour {
	public int armorDamageDrop = 20; //a percentage
	public int armorSpeedDrop = 30; //a percentage
	public int health = 100;
	public Vector3 offset;
	public float speed;
	public GameObject[] defenseList;
	public int movementUpgradeBoost = 200; //a percentage
	public Texture2D t2d; 	//crosshair stuff
	public GameObject[] weaponList;
	public float weaponYOffset; //reminder: y is up

	private bool reactiveArmor = false; //grabs from gm
	  //it's extra, but that's okay
	private GameObject currentDefense;
	private GameObject currentweapon;
	private PlayerWeaponScript currentWeaponScript; 
	private GameManager gm;
	private int h = 128; //crosshair height
	private PlayerJPHolderScript jPHolder;
	private Vector2 mouse; 
	private Rigidbody rb;
	private int w = 128; //crosshair width

	//	private int weaponIndex = 0;
	//layermask for player to not react to certain objects like scrambler
	//private LayerMask lm;
	//12/6/16 don't need currentDefenseScript right now
	//private PlayerDefenseScript currentDefenseScript;

	void Start() {
		rb = GetComponent<Rigidbody> (); //rb
		Cursor.visible = false; //cursor
		GameObject gmObject = GameObject.FindGameObjectWithTag ("GameManager"); //gm stuff
		gm = gmObject.GetComponent<GameManager>();
		gm.populateWeaponUpgrades (); //populate items/ui before running code
		populateWeaponUI ();
		gm.populateDefenseUpgrades ();
		populateDefenseUI ();
		if (gm.phlebotinumUpgrade) { //put function call here instead of gm Start so I know order
			turnOnPhlebotinum ();
		}
		if (gm.autoReloadUpgrade) { //same for auto reload
			turnOnAutoReload ();
		}
		if (gm.reactiveArmorUpgrade) {
			reactiveArmor = true;
			speed *= (1 - armorSpeedDrop/100f);
		}
		if (gm.moveSupportUpgrade) {
			speed *= (1 + movementUpgradeBoost / 100f);
		}
		if (gm.jetPackUpgrade) {
			jPHolder = gm.jPScriptHolder.GetComponent<PlayerJPHolderScript>();
			populateJetUI(); //makes sure button is set active
			gm.changeJetIcon(true); //putting it here by choice, didn't put it in JPHolderScript.cs
		}



		changeWeapon (0); //switch items early
		changeDefense (0); 
		gm.setAmmoText (currentWeaponScript.ammo.ToString ()); //update text early
		gm.setHealthText (health.ToString ());

	}


	void Update ()
	{
		mouse = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y); 	//move crosshair


		RaycastHit hit; //point weapon as needed
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {  //moved this code below the switch weapon code so that rotate is done on the same frame as the new weapon, not the old one
			currentweapon.transform.LookAt (hit.point);
		}
			
		if (currentWeaponScript.ammo == 0) { 	//add fire code
			if (Input.GetButtonDown ("Fire1") && !currentWeaponScript.Reloading) { //if no ammo, player attempts to fire, but is not reloading, then can play sound
				currentWeaponScript.playEmpty ();
			}
		} else {
			if (currentWeaponScript.aReload && !currentWeaponScript.Reloading) { //not for reload check, for telling if automatic or not
				if (Input.GetButton ("Fire1")) {
					RaycastHit hit2;
					if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit2)) {
						if (!hit.transform.CompareTag ("Player") && !hit.transform.CompareTag ("Scrambler")) {
							//Debug.Log ("attempting fire");
							currentWeaponScript.autoBeam (hit2); //a coroutine in script will handle delay
							if (currentWeaponScript.ammo == 0) {
								currentWeaponScript.setAmmo ("r");
							} else {
								sendHealthAndAmmoData ();
							}
						}
					}
				}
			} else {
				if (Input.GetButtonDown ("Fire1") && !currentWeaponScript.Reloading) { //if click and not reloading, then fire
					RaycastHit hit2;
					if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit2)) {
						if (!hit.transform.CompareTag ("Player") && !hit.transform.CompareTag ("Scrambler")) {
							currentWeaponScript.fireBeam (hit2); 
							sendHealthAndAmmoData ();
						}
					}
				}
			}
		}

		if (Input.GetKeyDown (KeyCode.Q) && !currentWeaponScript.Reloading) { 	//add code to switch currentweapon and currentDefense
			changeWeapon(0);
		} else if (Input.GetKeyDown (KeyCode.E) && !currentWeaponScript.Reloading) { //don't switch during reloading phase, messes up text
			changeWeapon (1);
		} else if (Input.GetKeyDown(KeyCode.R)) {
			currentWeaponScript.setAmmo("r"); //text set is also managed in this function
			//one more !isReloading check included for extra, and handles case where already have full ammo
		}
			
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			changeDefense (0);
		} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
			changeDefense (1);
		} else if (Input.GetKeyDown (KeyCode.Alpha3)) {
			changeDefense (2);
		}

		if (Input.GetKeyDown (KeyCode.M)) {
			jPHolder.startJetPack (); //this checks if can activate too
		}

		currentweapon.transform.position = transform.position + new Vector3 (0, weaponYOffset, 0); //move currentweapon and currentDefense as needed
		currentDefense.transform.position = transform.position + offset; //put transform code below so stuff is positioned after a changeWeapon for no frame oddities

	}

	void FixedUpdate() {
		float moveHorizontal = Input.GetAxis ("Horizontal"); //for moving player
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		//Vector3 movement = new Vector3(1,1,1);
		rb.AddForce (movement * speed);
	}

	/*
	//code for jumping, may remove later on
	//actually, may not, for jumping stuff
	void OnCollisionStay (Collision other) {
		if (Input.GetKeyDown (KeyCode.Space) && other.gameObject.CompareTag("Floor")) {
			rb.velocity += new Vector3(0,2,0);
		}
	} */

	//draws the crosshair that replaces the mouse on screen
	void OnGUI() {
		GUI.DrawTexture(new Rect(mouse.x - (w / 2), mouse.y - (h / 2), w, h), t2d);
	}

	//send ammo status for gm to use with ui
	void sendHealthAndAmmoData() {
		if (currentWeaponScript.ammo == 0) {
			gm.setAmmoText ("RELOAD");
		} else {
			gm.setAmmoText (currentWeaponScript.ammo.ToString ());
		}
	}

	//return earliest defenseIndex available to switch too, cycling via cycleDirection
	//else return -1 to say no indices available
	int checkAvailableDefenses(string cycleDirection) {
		for (int i = 0; i < defenseList.Length; i++) {
			GameObject go = defenseList [i];
			//check if populated
			if (!go) {
				continue;
			}

			PlayerDefenseScript pds = go.GetComponent<PlayerDefenseScript> ();
			if (pds.eFlag) {
				return i;
			}
		}
		return -1;
	}

	//by default, all buttons are disabled
	//so send button list for UI to place 	
	void populateDefenseUI() {
		foreach (GameObject i in defenseList) {
			if (i) {
				PlayerDefenseScript pds = i.GetComponent<PlayerDefenseScript> ();
				gm.populateDefenseUI (pds.button);
			}
		}	
	}

	void populateJetUI() {
		gm.populateJetUI (jPHolder.button);
	}

	void populateWeaponUI() {
		foreach (GameObject i in weaponList) {
			if (i) {
				PlayerWeaponScript pws = i.GetComponent<PlayerWeaponScript> ();
				gm.populateWeaponUI(pws.button);
			}
		}
	}

	public void changeDefense(int index, bool bypassCheck = false) {
		if (!defenseList [index]) { 	//check if index is populated, else do nothing
			return;
		}

		GameObject go = defenseList [index];
		PlayerDefenseScript pds = go.GetComponent<PlayerDefenseScript> ();
		if (!pds.eFlag) { 	//check if index has item available to switch too, if not, do nothing
			return;
		} else if (!bypassCheck) { 	//bypassCheck avoids same switch check, don't know if I need it 
			if (go == currentDefense) { //or if object to switch to is already active, don't do anything
				return;
			} 
		}

		currentDefense = defenseList[index]; //creates a list of indices mapping each item to an active status to be passed onto gm and ui
		currentDefense.transform.position = transform.position + new Vector3 (0, weaponYOffset, 0);
		PlayerDefenseScript currentPds = currentDefense.GetComponent<PlayerDefenseScript> ();
		currentPds.aFlag = true; //shouldn't need to set eFlag, that's for the other object's themselves


		string[] indices = new string[defenseList.Length];
		for (int i = 0; i < defenseList.Length; i++) {
			GameObject go2 = defenseList [i];
			if (go2) { 	//check if index populated in the first place
				PlayerDefenseScript pds2 = go2.GetComponent<PlayerDefenseScript> ();
				if (go2 != currentDefense) {
					pds2.aFlag = false;
					if (pds2.eFlag) { //eFlag is for if it's accessible or not
						indices [i] = "inactiveEnabled";
					} else {
						indices [i] = "inactiveDisabled"; //don't need a SetActive since this implies already set inactive
					}
				} else {
					indices [i] = "activeEnabled";
				}
			}
		}


		gm.changeDefenseIcon (indices);
	}

	public void changeHealth(int hlth) {
		float voi = (float)hlth;
		if (reactiveArmor) {
			health += (int) ((1 - armorDamageDrop / 100f) * voi);
		} else {
			health += (int) voi; //change health and update ui as needed
		}

		gm.setHealthText (health.ToString());

		if (health <= 0) {
			gm.gameOver (); 
		}
	}

	public void changeWeapon(int index) {
		//weaponIndex used to be here, do i need it now
		if (!weaponList [index]) { 
			return;
		}
		currentweapon = weaponList [index];
		currentweapon.SetActive(true);
		currentweapon.transform.position = transform.position + new Vector3 (0, weaponYOffset, 0);
		foreach (GameObject g in weaponList) {
			if (g && g != currentweapon) { //shouldn't need to add extra to check for empty slot here
				g.SetActive (false);
			}
		}

		currentWeaponScript = currentweapon.GetComponent<PlayerWeaponScript> ();
		gm.changeWeaponIcon (index);
		sendHealthAndAmmoData (); //note: changing weapon icon has nothing to do w/ health/ammo text
	}




	//switches defense item in response to it getting disabled if needed
	public void reactToDefenseDisabled(GameObject comparisonDefense) {
		int resIndex = checkAvailableDefenses ("right");
		if (resIndex == -1) {  //if no items available, make all inactive (or nothing if no item)
			string[] indices = new string[defenseList.Length]; 
			for (int i = 0; i < defenseList.Length; i++) {
				if (!defenseList [i]) { //if there's not even a GameObject, add "nothing" to list 
					indices [i] = "nothing";
				} else {
					indices [i] = "inactiveDisabled";
				}
			}
			gm.changeDefenseIcon (indices); 
		} else if (comparisonDefense == currentDefense) { 	//if currentDefense was the one that got set inactive, switch to the nearest one to the right
			changeDefense (resIndex);
		} else { 	//else if it wasn't, then just make sure it's set inactive
			string[] indices2 = new string[defenseList.Length];
			for (int i = 0; i < defenseList.Length; i++) {
				GameObject go = defenseList [i];
				if (!go) {
					indices2 [i] = "nothing";
					continue;
				}
				PlayerDefenseScript pds = go.GetComponent<PlayerDefenseScript> ();
				if (go == comparisonDefense) {
					indices2 [i] = "inactiveDisabled";
				} else if (go == currentDefense) {
					indices2 [i] = "activeEnabled";
				} else {
					if (pds.eFlag) {
						indices2 [i] = "inactiveEnabled";
					} else {
						indices2 [i] = "inactiveDisabled";
					}
				}
			}

			gm.changeDefenseIcon (indices2); 
		}
	}


	//runs when defense item is re-enabled, switching if needed
	public void reactToDefenseEnabled(GameObject comparisonDefense) {
		int numEnabled = 0;
		int comparisonIndex = 0;
		for (int i = 0; i < defenseList.Length; i++) { //check for number of items enabled for use in later if statement
			GameObject go = defenseList [i];
			if (!go) {
				continue;
			}
			PlayerDefenseScript pds = go.GetComponent<PlayerDefenseScript> ();
			if (go == comparisonDefense) {
				numEnabled += 1;
				comparisonIndex = i;
			} else if (pds.eFlag) {
				numEnabled += 1;					
			}
		}

		if (numEnabled == 1) { 	//if GameObject is the only one at the time it was made enabled, changeWeapon to it
			changeDefense (comparisonIndex);
		} else { 	//else just make sure it's set enabled
			string[] indices2 = new string[defenseList.Length];
			for (int i = 0; i < defenseList.Length; i++) {
				GameObject go = defenseList [i];
				if (!go) {
					indices2 [i] = "nothing";
					continue;
				}
				PlayerDefenseScript pds = go.GetComponent<PlayerDefenseScript> ();
				if (go == comparisonDefense) {
					indices2 [i] = "inactiveEnabled";
				} else if (go == currentDefense) {
					indices2 [i] = "activeEnabled";
				} else {
					if (pds.eFlag) {
						indices2 [i] = "inactiveEnabled";
					} else {
						indices2 [i] = "inactiveDisabled";
					}
				}
			}
			gm.changeDefenseIcon (indices2); //update ui
		}

	}

	public void populateDefense(GameObject go) {
		for (int i = 0; i < defenseList.Length; i++) {
			if (!defenseList [i]) {
				defenseList [i] = go;
				break;
			}
		}
	}

	public void populateWeapon(GameObject go) {
		for (int i = 0; i < weaponList.Length; i++) {
			if (!weaponList [i]) {
				weaponList [i] = go;
				break;
			}
		}
	}

	//decrease reload time too, but decrease magazine
	public void turnOnAutoReload() {
		foreach (GameObject i in weaponList) {
			if (i) {
				PlayerWeaponScript pws = i.GetComponent<PlayerWeaponScript> ();
				pws.aReload = true; 
			}
		}
	}

	//turn on damage boost and give reload boost
	public void turnOnPhlebotinum() {
		foreach (GameObject i in weaponList) {
			if (i) {
				PlayerWeaponScript pws = i.GetComponent<PlayerWeaponScript> ();
				pws.pBotinum = true; //implies reload decrease
			}
		}
	}

	public Rigidbody rbProp { //made property so jet pack script can access it
		get {return rb;}
	}
}
	