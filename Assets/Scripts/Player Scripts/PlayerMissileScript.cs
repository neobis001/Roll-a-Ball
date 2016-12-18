using UnityEngine;
using System.Collections;

//OnTriggerEnter list in here
public class PlayerMissileScript : MonoBehaviour {
	public int damage = 10;
	public GameObject fireSound;
	public int speed = 5;
	public float upTime = 2;

	public GameObject destroyPs;

	private GameObject enemy;
	private Vector3 hitPoint = new Vector3(0,0,0); //default it to 0 so no null value
	private bool isEnemyTheTarget = false;
	private float timeMarker;

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

	//when the missile's destroyed, instantiate a destroyPs as an explosion
	void OnDestroy() {
		Instantiate (destroyPs, transform.position, Quaternion.identity);
	}

	//if it's the enemy, destroy it and missile
	//if it's the player or scrambler, don't do anything
	//else just destroy the missile
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Enemy")) {
			EnemyScript es = other.GetComponent<EnemyScript> ();
			es.changeHealth (-damage);
			Destroy (gameObject);
		} else {
			string[] checkList = new string[]{"Player", "Scrambler", "PlayerMissile", 
				"AutoTrigger", "PlayerBeam", "ForceField"}; //avoid destroy on player, scrambler, or duplicate missile
			foreach (string tag in checkList) {
				if (other.gameObject.CompareTag (tag)) {
					return;
				}
			}
			Destroy (gameObject);
		}
	}

	public void givePhlebotinumBoost(int percentIncrease) {
		damage = (int) (damage *  (1 + percentIncrease/100f)); //can't do augmented op, need to do explicit cast with this percentage stuff
		//unlike reload time stuff, need this to keep damage at type int
		//make sure to include an f for float increase
	}

	//changes isEnemyTheTarget bool if target is enemy else the missile's just firing at the environment
	public void isEnemyTarget(RaycastHit hit) {
		if (hit.transform.gameObject.CompareTag ("Enemy")) {
			isEnemyTheTarget = true;
			enemy = hit.transform.gameObject;
		} else {
			hitPoint = hit.point;
		}
	}

	public void setHitPoint(Vector3 hpt) {
		hitPoint = hpt;
	}


}
