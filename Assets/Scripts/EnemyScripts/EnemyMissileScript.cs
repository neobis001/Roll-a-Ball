using UnityEngine;
using System.Collections;

public class EnemyMissileScript : MonoBehaviour {
	public int speed = 5;
	public float upTime = 2;
	public int damage = 50;
	public GameObject fireSound;
	public GameObject destroyPs;
	public AudioSource scrambledSound;
	//gets one corner of map for scrambler repositioning
	public Vector3 leftMapCorner;
	//gets another corner
	public Vector3 rightMapCorner;

	private GameObject player;
	private float timeMarker;
//	private Vector3 forwardBeforeScramble;
	private bool isScrambled;
	private Vector3 ghostLocation;
	//if scrambler's on
	private Vector3 scrambledTarget;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		timeMarker = Time.time;
		//setting this vector to an impossible to get random vector for comparison later on
//		forwardBeforeScramble = new Vector3 (-100000, -100000, -100000); 
		//starting w/ an impossible vector as a flag
		scrambledTarget = new Vector3 (-10000, -10000, -10000);
		GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		//can't put play sound in update because it runs after upTime flight
		if (gm.sFlag) {
			isScrambled = true;
			scrambledSound.Play ();

		}
		Instantiate (fireSound, transform.position, Quaternion.identity);
		StartCoroutine (ghostTimer ());
		Destroy (gameObject, 15);
	}

	//used to create a location of previous player position for missile to lower accuracy
	IEnumerator ghostTimer() {
		while (true) {
			yield return new WaitForSeconds (.5f);
			ghostLocation = player.transform.position;
		}
	}
	
	// Update is called once per frame
	//goes up while time traveled is less than upTime
	//goes on scramble mode if isScrambled is on
	//targets player while still active
	//else just keeps moving forward
	void Update () {
		if (Time.time - timeMarker < upTime) {
			transform.Translate (transform.forward * speed * Time.deltaTime, Space.World);
		} else if (isScrambled) {
			/*if (forwardBeforeScramble == new Vector3 (-100000, -100000, -100000)) {
				forwardBeforeScramble = transform.forward;
			}
			transform.forward = -forwardBeforeScramble;
			transform.Translate (transform.forward * speed * Time.deltaTime, Space.World); */
			if (scrambledTarget == new Vector3 (-10000, -10000, -10000)) {
				float newX = Random.Range (leftMapCorner.x, rightMapCorner.x);
				//reversed order here because of how axes work
				float newZ = Random.Range (rightMapCorner.z, leftMapCorner.z);
				//just by choice chose leftMapCorner
				float newY = leftMapCorner.y;
				scrambledTarget = new Vector3 (newX, newY, newZ);
				transform.LookAt (scrambledTarget);
			}
			transform.Translate (transform.forward * speed * Time.deltaTime, Space.World);
		} else if (player) {
			Quaternion currentR = transform.rotation;
			transform.LookAt (ghostLocation);
			Quaternion targetR = transform.rotation;
			Quaternion newRotation = Quaternion.Lerp (currentR, targetR, .04f);
			transform.rotation = newRotation;
			transform.Translate (transform.forward * speed * Time.deltaTime, Space.World);
		} else {
			transform.Translate (transform.forward * speed * Time.deltaTime, Space.World);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			Destroy (gameObject);
			other.GetComponent<PlayerControllerScript> ().changeHealth (-damage);
		} else if (other.gameObject.CompareTag("Scrambler")) {
			isScrambled = true;
			scrambledSound.Play ();
		} else {
			string[] checkList = new string[]{"Enemy", "EnemyMissile", "AutoTrigger"};
			foreach (string tag in checkList) {
				if (other.gameObject.CompareTag (tag)) {
					return;
				}
			}
			Destroy (gameObject);
		}
			
	}

	void OnDestroy() {
		Instantiate (destroyPs, transform.position, Quaternion.identity);
	}

	public void setIsScrambled(bool flag) {
		isScrambled = flag;
	}

}

