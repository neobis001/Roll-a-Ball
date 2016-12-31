using UnityEngine;
using System.Collections;

public class PlayerScramblerScript : PlayerDefenseScript {
	public Vector3 offset; //offset from scrambler to player
	  //scrambler is a gameObject whose center isn't meant to be in player location
	public int tempLifetime = 3; //temp. variable for temp. timer
	public bool runCoroutine; //temp. variable to run timer or not
	public GameObject scramblerInstance; //scrambler to be spawned on right click
	public int delayTime = 5; //delay before re-enabling

	private bool delay = false; //for disable code
	private GameManager gm; //gm

	void Start() {
		if (runCoroutine) {
			StartCoroutine (DisableScrambler ());
		}
		gm = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ();
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
		//even odder, if you add currentPds.eflag = true in PlayerControllerScript.cs Line 228, glitch is reversed
		//first button glitches out, other 2 work fine
		//until I figure out how to debug, can't really look into this problem :( 
		yield return new WaitForEndOfFrame ();
		setInactive ();
		delay = true;
		yield return new WaitForSeconds (delayTime);
		delay = false; 	
		gm.sFlag = false; //at the end, re-disable scrambler flag
		setEnabled ();
		Debug.Log ("setEnabled here");
	}

	// Update is called once per frame
	void Update () {
		transform.position = pcs.transform.position + offset;
		if (aFlag && !delay && Input.GetMouseButtonDown (1)) {
			Instantiate (scramblerInstance, transform.position, Quaternion.identity);
			gm.sFlag = true;
			StartCoroutine (DelayDisable ());
		}
	}

}
