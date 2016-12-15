using UnityEngine;
using System.Collections;

public class PlayerRepairScript : MonoBehaviour{
	public int lifeTime; //scrambler duration
	public float percentIncrease; //for health increase

	private GameObject player; //for following player transform
	private PlayerControllerScript pcs; //for changing health

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		pcs = player.GetComponent<PlayerControllerScript> ();
		StartCoroutine (Timer ());

		//add this so that it's placed right before next Update
		transform.position = player.transform.position;
	}

	//increase health every second until lifeTime reached
	IEnumerator Timer() {
		int timeCounter = 0;
		while (timeCounter < lifeTime) {
			yield return new WaitForSeconds (1);
			pcs.changeHealth ((int)(percentIncrease * pcs.health));
			timeCounter++;
		}
		//GetComponent<AudioSource> ().Stop ();
		Destroy (gameObject);
	}

	//update to player pos
	void Update() {
		transform.position = player.transform.position;
	}

}
