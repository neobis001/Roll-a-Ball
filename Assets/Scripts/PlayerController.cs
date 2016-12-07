using UnityEngine;
using UnityEngine.UI;
using System.Collections;


//hold weapons/weapon switching here

public class PlayerController : MonoBehaviour
{
	public float speed;
	public int beamTime = 2;
	public GameObject beamForPlayer;
	//connected w/ DefaultBeam.cs
	public Texture2D t2d;
	//crosshair stuff
	public GameObject[] turretList;
	public float turretYOffset;
	//reminder: y is up
	public AudioSource reloadSound;
	public AudioSource emptySound;
	public GameManager gm;
	public WeaponScript currentWeaponScript;
	//this and the other two below were supposed to be private, but made public for GameManager.cs
	public GameObject currentSpawner;
	public GameObject currentTurret;
	public Color originalWeaponC;
	//this and stuff below is for buttons
	public Color highlightedWeaponC;
	public Color originalDefenseC;
	public Color highlightedDefenseC;
	public Image[] weaponImages;
	public Image[] defenseImages;
	public GameObject firePrefab;


	private int defenseIndex = 0;
	private int weaponIndex = 0;
	private Vector2 mouse;
	//crosshair stuff
	private Rigidbody rb;
	private int debugCount;
	private int w = 128;
	//crosshair stuff
	private int h = 128;
	//crosshair stuff

	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
		//Cursor.visible = false;
		changeWeapon ();
		changeDefense (); //edit this later
		StartCoroutine (Timer ());
	}

	IEnumerator Timer ()
	{
		while (true) {
			yield return new WaitForSeconds (beamTime);
		}
	}

	void Update ()
	{
		currentTurret.transform.position = transform.position + new Vector3 (0, turretYOffset, 0);
		mouse = new Vector2 (Input.mousePosition.x, Screen.height - Input.mousePosition.y);
		// all turret point/fire code below. make cylinder, parent it, child empty, on click, rotate to spot, then fire

		if (Input.GetKeyDown (KeyCode.Q)) {
			weaponIndex--;
			if (weaponIndex < 0) {
				weaponIndex = turretList.Length - 1;
			}
			changeWeapon ();
		} else if (Input.GetKeyDown (KeyCode.E)) {
			weaponIndex++;
			if (weaponIndex == turretList.Length) {
				weaponIndex = 0;
			}
			changeWeapon ();
		} else if (Input.GetKeyDown (KeyCode.R)) {
			currentWeaponScript.setAmmo ("r");
			reloadSound.Play ();
		}

		//TEMPORARY CODE, change this with a defenseList list later, and edit changeDefese to work with defenseList
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			defenseIndex--;
			if (defenseIndex < 0) {
				defenseIndex = defenseImages.Length - 1;
			}
			changeDefense ();
		} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
			defenseIndex++;
			if (defenseIndex == defenseImages.Length) {
				defenseIndex = 0;
			}
			changeDefense ();
		}

		RaycastHit hit; //moved this code below the switch weapon code so that rotate is done on the same frame as the new weapon, not the old one
		if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit)) {
			currentTurret.transform.LookAt (hit.point);
		}

		if (currentWeaponScript.ammo == 0) {
			if (Input.GetButtonDown ("Fire1")) {
				emptySound.Play ();
			}
		} else {
			if (Input.GetButtonDown ("Fire1")) {
				RaycastHit hit2;
				if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit2)) {
					GameObject bfp = Instantiate (beamForPlayer);
					bfp.transform.position = currentSpawner.transform.position;
					bfp.transform.LookAt (hit2.point);

					Instantiate (firePrefab, currentSpawner.transform.position, currentSpawner.transform.rotation);

					currentWeaponScript.setAmmo ("d");
				}
			}
		}

	}

	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		rb.AddForce (movement * speed);
	}

	//code for jumping, may remove later on
	void OnCollisionStay (Collision other)
	{
		if (Input.GetKeyDown (KeyCode.Space) && other.gameObject.CompareTag ("Floor")) {
//			debugCount += 1;
//			Debug.Log ("Space was pressed.");
//			Debug.Log ("Space was pressed. Count: " + debugCount.ToString());
			rb.velocity += new Vector3 (0, 2, 0);
		}
	}

	void OnGUI ()
	{
		GUI.DrawTexture (new Rect (mouse.x - (w / 2), mouse.y - (h / 2), w, h), t2d);
	}

	//may be able to make this general for both weapon and defense item, will need to code in defense first
	//else just make another function
	void changeWeapon ()
	{
		currentTurret = turretList [weaponIndex]; //this line and ones below could be made into a function
		currentTurret.SetActive (true);
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
		currentWeaponScript = currentTurret.GetComponent<WeaponScript> ();
		Image highlightedImg = weaponImages [weaponIndex];
		highlightedImg.color = highlightedWeaponC;
		foreach (Image i in weaponImages) {
			if (i != highlightedImg) {
				i.color = originalWeaponC;
			}
		}

	}

	void changeDefense ()
	{
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
		Image highlightedDefense = defenseImages [defenseIndex];
		highlightedDefense.color = highlightedDefenseC;
		foreach (Image i in defenseImages) {
			if (i != highlightedDefense) {
				i.color = originalDefenseC;
			}
		}

	}

}
	