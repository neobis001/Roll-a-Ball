using UnityEngine;
using System.Collections;

public class PlayerScramblerScript : PlayerDefenseScript {
	//offset from scrambler to player
	public Vector3 offset;
	//temp. variable for temp. timer
	public int tempLifetime = 3;
	//temp. variable to run timer or not
	public bool runCoroutine;
	//scrambler to be spawned on right click
	public GameObject scramblerInstance;
	//delay before re-enabling
	public int delayTime = 5;

	//player position
	private Transform playerPos;
	//for disable code
	private bool delay = false;

	//start code
	void Start() {
		if (runCoroutine) {
			StartCoroutine (DisableScrambler ());
		}
		playerPos = pcs.transform;
	}

	//temp. code: disable scrambler after some time
	IEnumerator DisableScrambler() {
		yield return new WaitForSeconds (tempLifetime);
		StartCoroutine (DelayDisable ());
	}

	//delay for some time before player can run scrambler again
	IEnumerator DelayDisable() {
		//12/6/16 there's a very odd glitch where when the right click was pressed, apparently, multiple buttons were deactivated at once
		//this only when it was the second or third item selected, but not the first
		//I think it's because for some reason, since I had multiple scrambler objects, the if statements ran through on the same frame
		//the if's include the input.mouse button down press
		//what i find weird is that why didn't it glitch out when the first button was selected?
		//anyways, I found I could wait til end of frame for this coroutine, should prevent input flag from running for multiple scripts
		//on the same frame
		yield return new WaitForEndOfFrame ();
		setInactive ();
		delay = true;
		yield return new WaitForSeconds (delayTime);
		delay = false;
		setEnabled ();
	}

	// Update is called once per frame
	void Update () {
		transform.position = playerPos.position + offset;
		if (aFlag && !delay && Input.GetMouseButtonDown (1)) {
			Debug.Log ("click statement ran through");
			Instantiate (scramblerInstance, transform.position, Quaternion.identity);
			StartCoroutine (DelayDisable ());
		}
	}

}
