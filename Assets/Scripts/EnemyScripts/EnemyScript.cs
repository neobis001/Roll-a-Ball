﻿using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {
	public int fireDelay = 2;
	public int health = 50;
	public GameObject missileToBeFired;
	public float speed = 1;

	private bool destroyedByPlayer = false;
	private GameManager gm;

	void Start() {
		Destroy (gameObject, 10);
		gm = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ();
		StartCoroutine (Timer ());
	}

	// Update is called once per frame
	void Update () {
		transform.Translate (transform.forward * Time.deltaTime * speed, Space.World);
	}

	void OnDestroy() {
		if (destroyedByPlayer) {
			gm.decreaseEnemyCount ();
		}
	}

	public void changeHealth(int hlth) {
		health += hlth;

		if (health <= 0) {
			Destroy (gameObject);
			destroyedByPlayer = true;
		}
	}

	IEnumerator Timer() {
		while (true) {
			yield return new WaitForSeconds (fireDelay);
			Instantiate (missileToBeFired, transform.position, Quaternion.Euler (-90, 0, 0));
		}
	}


}
