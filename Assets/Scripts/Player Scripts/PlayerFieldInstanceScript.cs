using UnityEngine;
using System.Collections;

public class PlayerFieldInstanceScript : MonoBehaviour {
	public int lifeTime; //seconds, should be less than button delay

	private GameObject player; 

	void Start() {
		player = GameObject.FindGameObjectWithTag ("Player");
		Destroy (gameObject, lifeTime);
	}

	// Update is called once per frame
	void Update () {
		transform.position = player.transform.position;
	}
}
