using UnityEngine;
using System.Collections;

public class FireDelayLogic : MonoBehaviour {

	public float fireDelayTime = 0.1f;
	public bool canFireShot = true;

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			fireShot ();
		}
	}

	IEnumerator FireDelay() {
		canFireShot = false;
		yield return new WaitForSeconds (fireDelayTime);
		canFireShot = true;
	}

	void fireShot() {
		if (!canFireShot) {
			return;
		}
		StartCoroutine (FireDelay ());

		/*
		 * firing code here
		*/
	}
}
