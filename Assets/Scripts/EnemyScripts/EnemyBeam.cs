using UnityEngine;
using System.Collections;

//OnTriggerEnter list in here
public class EnemyBeam : MonoBehaviour {

	// Use this for initialization
	public int damage = -10;
	public int speed = 6;
	public GameObject fieldReflectSound;

	private GameObject player;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		transform.LookAt (player.transform);
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			other.GetComponent<PlayerControllerScript> ().changeHealth (-damage);
			Destroy (gameObject);
		} else if (other.gameObject.CompareTag("ForceField")) {
			Instantiate (fieldReflectSound, transform.position, Quaternion.identity);
			transform.forward *= -1;
		} else {
			string[] checkList = new string[]{"Enemy", "EnemyMissile", "AutoTrigger", "EnemyLaser", "Scrambler"};
			foreach (string tag in checkList) {
				if (other.gameObject.CompareTag (tag)) {
					return;
				}
			}
			Destroy (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(transform.forward.normalized * speed * Time.deltaTime , Space.World);
	}
}
