using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PlayerController : MonoBehaviour {
	public float speed;
	public Text winText;
	public Text loseText;
	public int beamTime = 2;
	public GameObject beamPrefab; //connected w/ LaserBeam.cs
	public GameObject beamForPlayer; //connected w/ LaserBeam2.cs
	public Texture2D t2d; //crosshair stuff
	public Vector2 mouse; //crosshair stuff
	public GameObject turretHolder;
	public Transform turretSpawner;
	public float turretYOffset; //reminder: y is up
	public int ammo;
	public int startingAmmo = 5;
	public Text ammoText;
	public AudioSource reloadSound;
	public AudioSource emptySound;

	private Rigidbody rb;
	private int debugCount;
	private int w = 128; //crosshair stuff
	private int h = 128; //crosshair stuff

	void Start() {
		rb = GetComponent<Rigidbody> ();
		winText.text = "";
		loseText.text = "";
		ammo = startingAmmo;
		ammoText.text = "Ammo: " + ammo.ToString ();

		UnityEngine.Cursor.visible = false;
		turretHolder.transform.position = transform.position + new Vector3 (0, turretYOffset, 0);
		StartCoroutine (Timer ());

		//this was for a count-based roll-a-ball, may use again?
		/*
		BoxCollider[] cset = cubeSet.GetComponentsInChildren<BoxCollider> ();
		for (int i = 0; i < cset.Length; i++) {
			if (cset [i].gameObject.activeSelf == true) { //checking active in case of testing
			}
		}*/


	}

	IEnumerator Timer() {
		while (true) {
			yield return new WaitForSeconds (beamTime);
			/*
			BoxCollider[] cset = cubeSet.GetComponentsInChildren<BoxCollider> (); //use any component as needed
			if (cset.Length != 0) {
				BoxCollider sel = cset [Random.Range (0, cset.Length)];
				Quaternion temp = Quaternion.identity;
				Instantiate (beamPrefab, sel.gameObject.transform.position, temp);
			} */ //this was for count-based roll-a-ball, may use again?
		}

	}

	void Update ()
	{
		turretHolder.transform.position = transform.position + new Vector3 (0, turretYOffset, 0);
		mouse = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
		// all turret point/fire code below. make cylinder, parent it, child empty, on click, rotate to spot, then fire
		RaycastHit hit;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
			turretHolder.transform.LookAt (hit.point);
		}

		if (Input.GetKeyDown(KeyCode.R)) {
			ammo = 5;
			ammoText.text = "Ammo: " + ammo.ToString ();
			reloadSound.Play ();
		}

		if (ammo == 0) {
			ammoText.text = "Ammo: RELOAD";
			if (Input.GetButtonDown ("Fire1")) {
				emptySound.Play ();
			}
		} else {
			if (Input.GetButtonDown ("Fire1")) {
				RaycastHit hit2;
				if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit2)) {
					/*GameObject bfp = Instantiate (beamForPlayer);
				bfp.transform.position = Camera.main.transform.position;
				bfp.transform.LookAt (hit2.point); */ //code for shooting from screen
					GameObject bfp = Instantiate (beamForPlayer);
					bfp.transform.position = turretSpawner.position;
					bfp.transform.LookAt (hit2.point);  //code for shooting from screen
					ammo -= 1;
					ammoText.text = "Ammo: " + ammo.ToString ();
				}
			}
		}

	}


	void FixedUpdate() {
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		rb.AddForce (movement * speed);

//		Debug.Log ("This is a test message. Should only happen once.");
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Pickup")) {
			//other.gameObject.SetActive (false);
			Destroy(other.gameObject);

		} else if (other.gameObject.CompareTag ("EnemyLaser")) {
			Destroy (gameObject);
			Destroy (other.gameObject);
			Camera.main.GetComponent<CameraController> ().enabled = false;
			loseText.text = "Game Over!";
		}
	}

	void OnCollisionStay (Collision other) {
		if (Input.GetKeyDown (KeyCode.Space) && other.gameObject.CompareTag("Floor")) {
			debugCount += 1;
			Debug.Log ("Space was pressed.");
			Debug.Log ("Space was pressed. Count: " + debugCount.ToString());
			rb.velocity += new Vector3(0,2,0);
		}
	}

	void OnGUI() {
		GUI.DrawTexture(new Rect(mouse.x - (w / 2), mouse.y - (h / 2), w, h), t2d);
	}
}

/*	void Update() {
//		Debug.DrawRay (Camera.main.ScreenPointToRay (Input.mousePosition).direction, Camera.main.transform.forward * 30);
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
//			Debug.Log ("Here");

			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit)) {
				agent.destination = hit.point;	
//				Debug.Log ("Here 2");
			}
		}
	}

}
*/	