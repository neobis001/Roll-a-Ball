using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {
	public float speed = 1;
	public int fireDelay = 2;
	public GameObject missileToBeFired;

	void Start() {
		Destroy (gameObject, 10);
		StartCoroutine (Timer ());
	}


	IEnumerator Timer() {
		while (true) {
			yield return new WaitForSeconds (fireDelay);
			Instantiate (missileToBeFired, transform.position, Quaternion.Euler (-90, 0, 0));
		}
	}

	// Update is called once per frame
	void Update () {
		transform.Translate (transform.forward * Time.deltaTime * speed, Space.World);
	}
}
