using UnityEngine;
using UnityEngine.UI;
using System.Collections;


//hold weapons/weapon switching here

public class PlayerControllerScript: MonoBehaviour {
	public float speed;
	//public int beamTime = 2;
	//public GameObject beamForPlayer; //connected w/ DefaultBeam.cs
	public Texture2D t2d; //crosshair stuff
	public GameObject[] turretList;
	public float turretYOffset; //reminder: y is up

	[Header("--Not for editing--")]
	public PlayerWeaponScript currentWeaponScript; //this and the other two below were supposed to be private, but made public for GameManager.cs

	private GameObject currentTurret; 
	private Vector2 mouse; //crosshair stuff
	private Rigidbody rb;
	private int debugCount;
	private int w = 128; //crosshair stuff
	private int h = 128; //crosshair stuff

	void Start() {
		rb = GetComponent<Rigidbody> ();
		Cursor.visible = false;

		//letting GameManager call changeWeapon and changeDefense
	}

	void Update ()
	{
		currentTurret.transform.position = transform.position + new Vector3 (0, turretYOffset, 0);
		mouse = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
		// all turret point/fire code below. make cylinder, parent it, child empty, on click, rotate to spot, then fire


		RaycastHit hit; //moved this code below the switch weapon code so that rotate is done on the same frame as the new weapon, not the old one
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
					currentWeaponScript.fireBeam (hit2);
				}
			}
		}
	}

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

	void OnGUI() {
		GUI.DrawTexture(new Rect(mouse.x - (w / 2), mouse.y - (h / 2), w, h), t2d);
	} 

	//may be able to make this general for both weapon and defense item, will need to code in defense first
	//else just make another function
	public void changeWeapon(int weaponIndex) {
		currentTurret = turretList [weaponIndex]; //this line and ones below could be made into a function
		currentTurret.SetActive(true);
		currentTurret.transform.position = transform.position + new Vector3 (0, turretYOffset, 0);
		foreach (GameObject g in turretList) {
			if (g != currentTurret) { 
				g.SetActive (false);
			}
		}
			
		currentWeaponScript = currentTurret.GetComponent<PlayerWeaponScript> ();


	}

	public void changeDefense(int defenseIndex) {
		/*currentTurret = turretList [weaponIndex]; //this line and ones below could be made into a function
		currentTurret.SetActive(true);
		currentTurret.transform.position = transform.position + new Vector3 (0, turretYOffset, 0);
		foreach (GameObject g in turretList) {
			if (g != currentTurret) { 
				g.SetActive (false);
			}
		}

		Transform[] ts = currentTurret.GetComponentsInChildren<Transform> ();
		foreach (Transform t in ts) {
			if (t.gameObject.CompareTag ("TurretSpawner")) {
				currentSpawner = t.gameObject;
				break;
			} 
		}
		currentWeaponScript = currentTurret.GetComponent<WeaponScript> (); */

	}
		
}
	