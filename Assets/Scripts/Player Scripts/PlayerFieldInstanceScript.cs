using UnityEngine;
using System.Collections;

public class PlayerFieldInstanceScript : MonoBehaviour {
	public float delayBoost;
	public int lifeTime; //seconds, should be less than button delay

	private float delayElapsed = 0f;
	private GameObject player; 
	private PlayerControllerScript pcs;

	void Start() {
		player = GameObject.FindGameObjectWithTag ("Player");
		pcs = player.GetComponent<PlayerControllerScript> ();
		StartCoroutine (lifeTimeDelay ());
	}

	// Update is called once per frame
	void Update () {
		transform.position = player.transform.position;

		if (Input.GetMouseButtonDown (0) && pcs.successFire) {
			delayElapsed += delayBoost;
		}
	}

	IEnumerator lifeTimeDelay() {
		pcs.freezeOnGround (true);
		while (delayElapsed <= lifeTime) {
			yield return new WaitForEndOfFrame ();
			delayElapsed += Time.deltaTime;
		}
		Destroy (gameObject);
		pcs.freezeOnGround (false);
	}
}
