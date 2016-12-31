using UnityEngine;
using UnityEngine.UI;
using System.Collections;


//as of now, if there are no weapons in the weapon list, will error
//just make sure at least basic cannon is in weapon list
//still I think it's possible to do null checks with GUI and other stuff
public class PlayerControllerScript: MonoBehaviour {
	public int armorDamageDrop = 20; //a percentage
	public int armorSpeedDrop = 30; //a percentage
	public int cyclopsBoost = 150; //a percentage, for speed increase
	public int health = 100;
	public float maxSpeed = 10; //used if cyclopsBoost is checked
	public float movementBoost = 200; //a percentage, for acc. increase
	public Vector3 offset;
	public float speed; //used when adding force
	public GameObject[] defenseList;
	public Texture2D t2d; 	//main crosshair 
	public Texture2D auto2d; //auto aim crosshair
	public GameObject[] weaponList; 
	public float weaponYOffset; //reminder: y is up
	  //relative to player Y, not world origin

	private bool aimUpgrade = false; //grabs from gm, this is okay extra
	private bool reactiveArmor = false; //grabs from gm
	  //it's extra, but that's okay
	private GameObject currentDefense;
	private GameObject currentweapon;
	private PlayerWeaponScript currentWeaponScript; 
/*	public Vector3 autoTarget = new Vector3(-10000, -10000,-10000); //for telling OnGUI whether to draw auto crosshair or not
	  //making default value this to act like false bool value */
	public GameObject autoTarget = null; //for telling OnGUI whether to draw auto crosshair or not
	  //needs to be GameObject to change hit2 GameObject to enemy GameObject instead of default point
	private GameManager gm;
	private int h = 128; //crosshair height, shared with autoaim cursor for now
	private PlayerJPHolderScript jPHolder;
	private Vector2 mouse; 
	private Rigidbody rb;
	private int w = 128; //crosshair width

	//	private int weaponIndex = 0;
	private LayerMask lm; //layermask specifically for case when want to ignore AutoTrigger layer
	//12/6/16 don't need currentDefenseScript right now
	//private PlayerDefenseScript currentDefenseScript;

	void Start() {
		rb = GetComponent<Rigidbody> (); //rb
		Cursor.visible = false; //cursor
		GameObject gmObject = GameObject.FindGameObjectWithTag ("GameManager"); //gm stuff
		gm = gmObject.GetComponent<GameManager>();
		gm.editModeItemLoad (); //for edit mode only when not loading save.txt, will get overriden by loadSaveFile if its code is allowed to run
		gm.loadSaveFile ();
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
			maxSpeed *= (1 - armorSpeedDrop/100f);
		}
		if (gm.cyclopsUpgrade) {
			maxSpeed *= (1 + cyclopsBoost / 100f);
		}
		if (gm.movementUpgrade) {
			speed *= (1 + movementBoost / 100f);
		}
		if (gm.jetPackUpgrade) {
			jPHolder = gm.jPScriptHolder.GetComponent<PlayerJPHolderScript>();
			populateJetUI(); //makes sure button is set active
			gm.changeJetIcon(true); //putting it here by choice, didn't put it in JPHolderScript.cs
		}
		if (gm.aimUpgrade) {
			aimUpgrade = true;
		}
			
		string[] layerStrings = new string[9]; //9 for number of default layers, 0-9 excluding 8: AutoTrigger
		int indexCounter = 0; //need to separate indexing from numbers to put in given index
		for (int i = 0; i < 10; i++) {
			if (i != 8) {
				layerStrings [indexCounter] = LayerMask.LayerToName (i);
				indexCounter++;
			}
		}
		lm = LayerMask.GetMask(layerStrings);

		changeWeapon (0); //switch items early
		changeDefense (0);
		gm.setAmmoText (currentWeaponScript.ammo.ToString ()); //update text early
		gm.setHealthText (health.ToString ());

	}


	void Update ()
	{
		mouse = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y); 	//move crosshair
		RaycastHit hit; //point weapon as needed

		//this code checkes where the currentWeapon should point at. it doesn't directly influence the fire location
		  //it's setting an autoTarget that influences the fire location, otherwise the normal fire location is unaffected
		//(exception: for the quad cannon, it does have influence)
		if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit)) {  //moved this code below the switch weapon code so that rotate is done on the same frame as the new weapon, not the old one
			if (aimUpgrade) {
				if (hit.transform.CompareTag ("AutoTrigger")) {
					autoTarget = hit.transform.GetComponentInParent<Transform> ().gameObject;
					currentweapon.transform.LookAt (autoTarget.transform);
				} else {
					autoTarget = null;
					currentweapon.transform.LookAt (hit.point);
				}
			} else {
				Debug.Log ("in here");
				if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, Mathf.Infinity, lm)) { //if not aimUpgrade
					//ignore AutoTrigger stuff when doing raycast, doing another raycast is repetitive, but only idea i have right now
					//need to reassign hit because we're considering physics w/o AutoTrigger now,unlike last if statement
					currentweapon.transform.LookAt (hit.point);
				}
			}
		}
			
		if (currentWeaponScript.ammo == 0) { 	//add fire code
			if (Input.GetButtonDown ("Fire1") && !currentWeaponScript.Reloading) { //if no ammo, player attempts to fire, but is not reloading, then can play sound
				currentWeaponScript.playEmpty ();
			}
		} else {
			if (currentWeaponScript.aReload && !currentWeaponScript.Reloading) { //aReload is not for reload check, for telling if auto reload or not
				if (Input.GetButtonDown ("Fire1")) {
					//Debug.Log ("in aReload if statement after Fire1 made");
					RaycastHit hit2;
					if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit2, Mathf.Infinity, lm)) { //with autoTarget if statement below 
						//from checking AutoTrigger above, just raycast with lm layermask
						if (!hit.transform.CompareTag ("Player") && !hit.transform.CompareTag ("Scrambler")) {
							if (autoTarget != null) { //if autoTarget found a hit from mouse code
								//Debug.Log("in autoTarget changing if statement");
								/*hit2.point = autoTarget; //autoBeam accepts only Raycasthit, so edit point first
							  //could theoretically just rewrite autoBeam script, maybe later
							currentWeaponScript.autoBeam (hit2); */
								currentWeaponScript.fireBeam (hit2, autoTarget); 
							} else {
								currentWeaponScript.fireBeam (hit2); //a coroutine in script will handle delay
							}
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
					if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, Mathf.Infinity, lm)) { //same comment as similar if statement above
						if (!hit.transform.CompareTag ("Player") && !hit.transform.CompareTag ("Scrambler")) {
							if (autoTarget != null) {  //if autoTarget found a hit from mouse code
								currentWeaponScript.fireBeam (hit, autoTarget);
							} else {
								currentWeaponScript.fireBeam (hit); //a coroutine in script will handle delay
							}
							sendHealthAndAmmoData ();
						}
					}
				}
			}
		}
			
		if (Input.GetKeyDown (KeyCode.Q) && !currentWeaponScript.Reloading) { 	//add code to switch currentweapon and currentDefense
			changeWeapon (0);
		} else if (Input.GetKeyDown (KeyCode.E) && !currentWeaponScript.Reloading) { //don't switch during reloading phase, messes up text
			changeWeapon (1);
		} else if (Input.GetKeyDown (KeyCode.R)) {
			currentWeaponScript.setAmmo ("r"); //text set is also managed in this function
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
			if (jPHolder != null) {
			jPHolder.startJetPack (); //this checks if can activate too
			}
		}

		if (currentweapon != null) {
			currentweapon.transform.position = transform.position + new Vector3 (0, weaponYOffset, 0); //move currentweapon and currentDefense as needed
		}
		if (currentDefense != null) {
			currentDefense.transform.position = transform.position + offset; //put transform code below so stuff is positioned after a changeWeapon for no frame oddities
		}
	}

	void FixedUpdate() {
		float moveHorizontal = Input.GetAxis ("Horizontal"); //for moving player
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		//Vector3 movement = new Vector3(1,1,1);

		rb.AddForce (movement * speed); //not exactly units accurate, but it works, more speed = more force

		if (rb.velocity.magnitude > maxSpeed) { //if velocity becomes greater than maxSpeed, use the distance formula
			  //to find a multiplier that'll make the velocity vector such that it's less than the maxSpeed
			  //make note that multipliers can increase maxSpeed by a lot
/*			Vector3 currentVel = rb.velocity;
			float dampDenom = Mathf.Pow (currentVel.x,2) + Mathf.Pow (currentVel.y,2) + Mathf.Pow (currentVel.z,2);
			  //wrote this denominator variable as separately because dampMultiplier formula getting big
			  //don't need a Mathf.Abs since this is always sure to be positive
			float dampMultiplier = Mathf.Sqrt (Mathf.Pow (maxSpeed, 2) / dampDenom);
			Vector3 newVel = currentVel * dampMultiplier;
			float test = maxSpeed / currentVel.magnitude; */
			float dampMultiplier = maxSpeed / rb.velocity.magnitude; //the formula found above simplifies to maxSpeed/current velocity
			Vector3 newVel = rb.velocity * dampMultiplier;
			rb.velocity = newVel;
		}
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

		if (autoTarget != null) { 
			Vector3 drawLocation = Camera.main.WorldToScreenPoint (autoTarget.transform.position); //not sure if screenpoint designed to be used with DrawTexture 
			float drawY = Camera.main.pixelHeight - drawLocation.y;
			  //drawtexture assumes top left is 0,0
			  //however, WorldToScreenPoint assumes bottom left is 0,0
			  //to do conversion, do camera height - drawLocation y
			GUI.DrawTexture (new Rect (drawLocation.x - (w / 2), drawY - (h / 2), w, h), auto2d);
		}
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

	public void changeDefense(int index) {
		if (!defenseList [index]) { 	//check if index is populated, else do nothing
			return;
		}

		GameObject go = defenseList [index];
		PlayerDefenseScript pds = go.GetComponent<PlayerDefenseScript> ();
		if (!pds.eFlag) { 	//check if index has item available to switch too, if not, do nothing
			return;
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


	public void populateDefense(GameObject go) {
		for (int i = 0; i < defenseList.Length; i++) {
			if (!defenseList [i]) {
				defenseList [i] = go;
				break;
			}
		}
	}

	public void populateWeapon(GameObject go, string id) {
		if (id == "quad") {
			for (int i = 0; i < weaponList.Length; i++) {
				if (weaponList [i]) {
					if (weaponList [i].GetComponent<PlayerWeaponScript> ().id == "cannon") {
						weaponList [i].SetActive (false);
						weaponList [i] = go;
						break;
					}
				}
			}
		} else {
			for (int i = 0; i < weaponList.Length; i++) {
				if (!weaponList [i]) {
					weaponList [i] = go;
					break;
				}
			}
		}

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

		Debug.Log ("numEnabled is " + numEnabled.ToString ());
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
	