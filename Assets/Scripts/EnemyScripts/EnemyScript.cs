using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {
	public float speed = 1;

	void Start() {
		Destroy (gameObject, 10);
	}

	// Update is called once per frame
	void Update () {
		transform.Translate (transform.forward * Time.deltaTime * speed, Space.World);
	}
}
