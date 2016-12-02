using UnityEngine;
using System.Collections;

public class PlayerScramblerScript : MonoBehaviour {
	public Vector3 offset;
	//NOTE: what about disabled and lasting time mode

	private GameObject player; //temp testing? don't know if really need player in the end

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = player.transform.position + offset;
	}

	/*void OnTriggerEnter(Collider other) {
		if (other.CompareTag ("EnemyMissile")) {
			Debug.Log ("in setIsScrambled code");
			other.GetComponent<EnemyMissileScript> ().setIsScrambled (true);
		}
	}*/

}
