using UnityEngine;
using System.Collections;

public class EnemyMissileScript : MonoBehaviour {
	public int speed = 5;
	public float upTime = 2;
	public int damage = 50;
	public GameObject fireSound;
	public GameObject destroyPs;
	public AudioSource scrambledSound;

	private GameObject player;
	private float timeMarker;
	private Vector3 forwardBeforeScramble;
	private bool isScrambled;
	private Vector3 ghostLocation; 

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		timeMarker = Time.time;
		//setting this vector to an impossible to get random vector for comparison later on
		forwardBeforeScramble = new Vector3 (-100000, -100000, -100000); 
		Instantiate (fireSound, transform.position, Quaternion.identity);
		Destroy (gameObject, 15);
		StartCoroutine (ghostTimer ());
	}

	IEnumerator ghostTimer() {
		while (true) {
			yield return new WaitForSeconds (.5f);
			ghostLocation = player.transform.position;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - timeMarker < upTime) {
			transform.Translate (transform.forward * speed * Time.deltaTime, Space.World);
		} else if (isScrambled) {
			if (forwardBeforeScramble == new Vector3 (-100000, -100000, -100000)) {
				forwardBeforeScramble = transform.forward;
			}
			transform.forward = -forwardBeforeScramble;
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
			string[] checkList = new string[]{"Enemy", "EnemyMissile"};
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

