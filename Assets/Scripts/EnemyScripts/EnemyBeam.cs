using UnityEngine;
using System.Collections;

public class EnemyBeam : MonoBehaviour {

	// Use this for initialization
	public int speed = 6;
	public int damage = -10;

	private GameObject player;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		transform.LookAt (player.transform);

		Destroy (gameObject, 10);
	}

	void OnTriggerEnter(Collider other) {
		Destroy (gameObject);
		if (other.gameObject.CompareTag("Player")) {
			other.GetComponent<PlayerControllerScript> ().changeHealth (-damage);
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(transform.forward.normalized * speed * Time.deltaTime , Space.World);
	}
}
