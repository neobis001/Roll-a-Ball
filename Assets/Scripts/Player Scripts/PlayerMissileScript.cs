using UnityEngine;
using System.Collections;

public class PlayerMissileScript : MonoBehaviour {
	public int speed = 5;
	public float upTime = 2;
	public GameObject fireSound;
	public GameObject destroyPs;

	private float timeMarker;
	private bool isEnemyTheTarget = false;
	private Vector3 hitPoint = new Vector3(0,0,0); //default it to 0 so no null value
	private GameObject enemy;

	// Use this for initialization
	void Start () {
		timeMarker = Time.time;
		Instantiate (fireSound, transform.position, Quaternion.identity);
		Destroy (gameObject, 10);
	} 
	
	// Update is called once per frame
	void Update () {
		if (Time.time - timeMarker < upTime) {
			transform.Translate (transform.forward * speed * Time.deltaTime, Space.World);
		} else if (isEnemyTheTarget == true && enemy) {
			transform.LookAt (enemy.transform.position);
			transform.Translate (transform.forward * speed * Time.deltaTime, Space.World);
		} else {
			transform.LookAt (hitPoint);
			transform.Translate (transform.forward * speed * Time.deltaTime, Space.World);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Enemy")) {
			Destroy (other.gameObject);
			Destroy (gameObject);
		} else {
			string[] checkList = new string[]{"Player", "Scrambler"};
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

	public void setHitPoint(Vector3 hpt) {
		hitPoint = hpt;
	}

	public void isEnemyTarget(RaycastHit hit) {
		if (hit.transform.gameObject.CompareTag ("Enemy")) {
			isEnemyTheTarget = true;
			enemy = hit.transform.gameObject;
		} else {
			hitPoint = hit.point;
		}
	}
}
