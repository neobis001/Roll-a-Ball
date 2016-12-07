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
	public float speed;
	public int health = 100;
	public GameObject[] turretList;
	//offset turret from top of tank
	//reminder: y is up
	public float turretYOffset; 
	public GameObject[] defenseList;
	public Vector3 offset;
	//crosshair stuff
	public Texture2D t2d;

	private GameManager gm;
	private Rigidbody rb;
	private int debugCount;
	private GameObject currentTurret; 
	private int weaponIndex = 0;
	private PlayerWeaponScript currentWeaponScript; 
	private GameObject currentDefense;
	//layermask for player to not react to certain objects like scrambler
	//private LayerMask lm;
	//12/6/16 don't need currentDefenseScript right now
	//private PlayerDefenseScript currentDefenseScript;
	//3 crosshair variables below
	private Vector2 mouse; 
	private int w = 128; 
	private int h = 128; 


	//setup rb, crosshair, GameManager stuff
	//switch items early
	//update text early
	//setup layer mask to default layers w/o Ignore Raycast layer
	void Start() {
		rb = GetComponent<Rigidbody> ();
		Cursor.visible = false;
		GameObject gmObject = GameObject.FindGameObjectWithTag ("GameManager");
		gm = gmObject.GetComponent<GameManager>();
		changeWeapon ();
		changeDefense (0);
		gm.setAmmoText (currentWeaponScript.ammo.ToString ());
		gm.setHealthText (health.ToString ());

/*		string[] layerStrings = new string[8];
		for (int i = 0; i < 8; i++) {
			//Debug.Log ("Running layer for index: " + i.ToString ());
			if (i == 2) {
				continue;
			} else {
				layerStrings [i] = LayerMask.LayerToName (i);
			}
		}
		lm = LayerMask.GetMask (layerStrings); */

	}
		
	//move crosshair
	//make turret point as needed
	//add fire code
	//add code to switch currentTurret and currentDefense
	//move currentTurret and currentDefense as needed
	void Update ()
	{
		mouse = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);

		//moved this code below the switch weapon code so that rotate is done on the same frame as the new weapon, not the old one
		RaycastHit hit; 
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
			currentTurret.transform.LookAt (hit.point);
		}

		if (currentWeaponScript.ammo == 0) {
			if (Input.GetButtonDown ("Fire1")) {
				currentWeaponScript.playEmpty ();
			}
		} else {
			if (Input.GetButtonDown ("Fire1")) {
				RaycastHit hit2;
				if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit2)) {
					if (!hit.transform.CompareTag ("Player") && !hit.transform.CompareTag("Scrambler")) {
						currentWeaponScript.fireBeam (hit2);
						//a copy  of "ammo == 0" code is here so that it doesn't need to be run on every update
						//when the first "ammo == 0" line is passed 
						sendHealthAndAmmoData();
					}
				}
			}
		}

		if (Input.GetKeyDown (KeyCode.Q)) {
			weaponIndex--;
			if (weaponIndex < 0) {
				weaponIndex = turretList.Length - 1;
			}
			changeWeapon();
		} else if (Input.GetKeyDown (KeyCode.E)) {
			weaponIndex++;
			if (weaponIndex == turretList.Length) {
				weaponIndex = 0;
			}
			changeWeapon ();
		} else if (Input.GetKeyDown(KeyCode.R)) {
			currentWeaponScript.setAmmo("r");
			currentWeaponScript.playReload ();
			gm.setAmmoText (currentWeaponScript.ammo.ToString());
		}
			
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			changeDefense (0);
		} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
			changeDefense (1);
		} else if (Input.GetKeyDown (KeyCode.Alpha3)) {
			changeDefense (2);
		}

		//put transform code below so stuff is positioned after a changeWeapon for no frame oddities
		currentTurret.transform.position = transform.position + new Vector3 (0, turretYOffset, 0);
		currentDefense.transform.position = transform.position + offset;

	}

	//gets player input and converts it into movement for player rb
	void FixedUpdate() {
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		rb.AddForce (movement * speed);
	}

	//code for jumping, may remove later on
	void OnCollisionStay (Collision other) {
		if (Input.GetKeyDown (KeyCode.Space) && other.gameObject.CompareTag("Floor")) {
			rb.velocity += new Vector3(0,2,0);
		}
	}

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
			PlayerDefenseScript pds = go.GetComponent<PlayerDefenseScript> ();
			if (pds.eFlag) {
				return i;
			}
		}
		return -1;
	}

	//change GameObjects as needed to change a weapon
	//then have gm call changeWeaponIcon to work with UI
	public void changeWeapon() {
		currentTurret = turretList [weaponIndex]; 
		currentTurret.SetActive(true);
		currentTurret.transform.position = transform.position + new Vector3 (0, turretYOffset, 0);
		foreach (GameObject g in turretList) {
			if (g != currentTurret) { 
				g.SetActive (false);
			}
		}
			
		currentWeaponScript = currentTurret.GetComponent<PlayerWeaponScript> ();
		gm.changeWeaponIcon (weaponIndex);
		sendHealthAndAmmoData ();
	}

	//check if index has item available to switch too, if not, do nothing
	//or if object to switch to is already active, don't do anything
	//bypassCheck avoids same switch check, don't know if I need it 
	//creates a list of indices mapping each item to an active status to be passed onto gm and ui
	//active means it's the current object; enabled means whether it's accessible or not
	public void changeDefense(int index, bool bypassCheck = false) {
		GameObject go = defenseList [index];
		PlayerDefenseScript pds = go.GetComponent<PlayerDefenseScript> ();
		if (!pds.eFlag) {
			return;
		} else if (!bypassCheck) {
			if (go == currentDefense) {
				return;
			} 
		}

		currentDefense = defenseList[index];
		currentDefense.transform.position = transform.position + new Vector3 (0, turretYOffset, 0);
		PlayerDefenseScript currentPds = currentDefense.GetComponent<PlayerDefenseScript> ();
		//shouldn't need to set eFlag, that's for the other object's themselves
		currentPds.aFlag = true;


		string[] indices = new string[defenseList.Length];
		for (int i = 0; i < defenseList.Length; i++) {
			GameObject go2 = defenseList [i];
			PlayerDefenseScript pds2 = go2.GetComponent<PlayerDefenseScript> ();
			if (go2 != currentDefense) {
				pds2.aFlag = false;
				if (pds2.eFlag) {
					indices [i] = "inactiveEnabled";
				} else {
					//don't need a SetActive since this implies already set inactive
					indices [i] = "inactiveDisabled";
				}
			} else {
				indices [i] = "activeEnabled";
			}
		}

		gm.changeDefenseIcon (indices);
/*		string[] pdses = new string[defenseList.Length];
		for (int i = 0; i < defenseList.Length; i++) {
			pdses [i] = defenseList [i].GetComponent<PlayerDefenseScript> ().aFlag.ToString();
		}

		Debug.Log (pdses [0] + pdses [1]+ pdses [2]);*/

	}

	//decrease or increase player's health, then have gm reflect it in the ui
	//make it Game Over if health is 0.
	public void changeHealth(int hlth) {
		health += hlth;
		gm.setHealthText (health.ToString());

		if (health <= 0) {
			gm.gameOver ();
		}
	}

	//switches defense item if current item was the one that just got set inactive
	//if none are available for switching, make all buttons inactive
	//if currentDefense was the one that got set inactive, switch to the nearest one to the right
	//else if it wasn't, then just make sure it's set inactive 
	public void reactToDefenseDisabled(GameObject comparisonDefense) {
		int resIndex = checkAvailableDefenses ("right");
		if (resIndex == -1) {
			//Debug.Log ("if statement 1");
			string[] indices = new string[defenseList.Length];
			for (int i = 0; i < defenseList.Length; i++) {
				indices [i] = "inactiveDisabled";
			}
			gm.changeDefenseIcon (indices);
		} else if (comparisonDefense == currentDefense) {
			//Debug.Log ("if statement 2");
			changeDefense (resIndex);
		} else {
			//Debug.Log ("if statement 3");
			string[] indices2 = new string[defenseList.Length];
			for (int i = 0; i < defenseList.Length; i++) {
				GameObject go = defenseList [i];
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

	//if GameObject is the only one at the time it was made enabled, changeWeapon to it
	//else just make sure it's set enabled
	public void reactToDefenseEnabled(GameObject comparisonDefense) {
		int numEnabled = 0;
		int comparisonIndex = 0;
		for (int i = 0; i < defenseList.Length; i++) {
			GameObject go = defenseList [i];
			PlayerDefenseScript pds = go.GetComponent<PlayerDefenseScript> ();
			if (go == comparisonDefense) {
				numEnabled += 1;
				comparisonIndex = i;
			} else if (pds.eFlag) {
				numEnabled += 1;					
			}
		}

		if (numEnabled == 1) {
			changeDefense (comparisonIndex);
		} else {
			string[] indices2 = new string[defenseList.Length];
			for (int i = 0; i < defenseList.Length; i++) {
				GameObject go = defenseList [i];
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
			gm.changeDefenseIcon (indices2);
		}

	}

}
	