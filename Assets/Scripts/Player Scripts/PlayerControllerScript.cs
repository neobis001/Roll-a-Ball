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
	public float turretYOffset; //reminder: y is up
	public GameObject[] defenseList;
	public Vector3 offset;
	public Texture2D t2d; //crosshair stuff

	private GameManager gm;
	private Rigidbody rb;
	private int debugCount;
	private GameObject currentTurret; 
	private int weaponIndex = 0;
	private PlayerWeaponScript currentWeaponScript; 
	private GameObject currentDefense;
	private int defenseIndex = 0;
	//12/5/16 don't need this line right now, comment it out for now
//	private PlayerDefenseScript currentDefenseScript;
	//3 crosshair variables below
	private Vector2 mouse; 
	private int w = 128; 
	private int h = 128; 

	void Start() {
		rb = GetComponent<Rigidbody> ();
		Cursor.visible = false;
		GameObject gmObject = GameObject.FindGameObjectWithTag ("GameManager");
		gm = gmObject.GetComponent<GameManager>();
		changeWeapon ();
		changeDefense ("right", true);
		gm.setAmmoText (currentWeaponScript.ammo.ToString ());
		gm.setHealthText (health.ToString ());
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
					if (!hit.transform.CompareTag ("Player") && !hit.transform.CompareTag("Temporary")) {
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
			/*defenseIndex--;
			if (defenseIndex < 0) {
				defenseIndex = defenseList.Length - 1;
			}*/
			changeDefense ("left");
		} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
			/*defenseIndex++;
			if (defenseIndex == defenseList.Length) {
				defenseIndex = 0;
			}*/
			changeDefense ("right");
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
	//defenseIndex changed here if an index is available
	//else return -1 to say no indices available
	int checkAvailableDefenses(string cycleDirection) {
		int tempIndex = defenseIndex;
		while (true) {
			if (cycleDirection == "left") {
				//Debug.Log ("cycling left");
				tempIndex--;
				if (tempIndex < 0) {
					tempIndex = defenseList.Length - 1;
				} 
			} else {
				//Debug.Log ("cycling right");
				tempIndex++;
				if (tempIndex == defenseList.Length) {
					tempIndex = 0;
				}
			}
				


			if (tempIndex == defenseIndex) {
				return -1;
			}

			GameObject go = defenseList [tempIndex];
			PlayerDefenseScript pds = go.GetComponent<PlayerDefenseScript> ();
			if (pds.aFlag) {
				//defenseIndex = tempIndex;
				return tempIndex;
			}

		}
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

	//bypassCheck is for Start function, when we're sure we don't need a check
	//if bypassCheck though, checks if there's even items available for switching, else doesn't do anything
	//creates a list of indices mapping each item to an active status to be passed onto gm and ui
	public void changeDefense(string cycleDirection, bool bypassCheck = false) {
		if (!bypassCheck) {
			int possibleIndex = checkAvailableDefenses(cycleDirection);
			if (possibleIndex == -1) {
				return;
			} else {
				defenseIndex = possibleIndex;
			}
		}

		currentDefense = defenseList[defenseIndex];
		currentDefense.SetActive(true);
		currentDefense.transform.position = transform.position + new Vector3 (0, turretYOffset, 0);
		//12/5/16 don't need this line, just comment it out for now
		//currentDefenseScript = currentDefense.GetComponent<PlayerDefenseScript> ();

		string[] indices = new string[defenseList.Length];
		for (int i = 0; i < defenseList.Length; i++) {
			GameObject go = defenseList [i];
			PlayerDefenseScript pds = go.GetComponent<PlayerDefenseScript> ();
			if (go != currentDefense) {
				if (pds.aFlag) {
					indices [i] = "inactiveEnabled";
					go.SetActive (false);
				} else {
					indices [i] = "inactiveDisabled";
				}
			} else {
				indices [i] = "activeEnabled";
			}
		}
/*		foreach (GameObject g in defenseList) {
			if (g != currentDefense) { 
				g.SetActive (false);
			}
		} */

		gm.changeDefenseIcon (indices);
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
	//if none are available for switching, or if the current item isn't equal to the one that just got set inactive
	//don't do anything
	public void reactToDefenseInactive(GameObject comparisonDefense) {
		if (checkAvailableDefenses ("right") == -1 || comparisonDefense != currentDefense) {
			return;
		} else {
			changeDefense ("right");
		}
	} 
}
	