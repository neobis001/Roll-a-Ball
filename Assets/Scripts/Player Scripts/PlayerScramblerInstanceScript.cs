using UnityEngine;
using System.Collections;

public class PlayerScramblerInstanceScript : MonoBehaviour {
	//scrambler duration
	public int lifeTime;

	//for following player transform
	private GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		StartCoroutine (Timer ());

		//add this so that it's placed right before next Update
		transform.position = player.transform.position;
	}

	IEnumerator Timer() {
		yield return new WaitForSeconds (lifeTime);
		Destroy (gameObject);
	}

	void Update() {
		transform.position = player.transform.position;
	}

}
