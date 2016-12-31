using UnityEngine;
using System.Collections;

public class PlayerFieldScript : PlayerDefenseScript {
	public float delayTime; 	//lifetime in seconds before next field allowed
	public GameObject fieldPrefab; //to represent repairing visually

	//for coroutine
	private bool delay = false;

	IEnumerator DelayDisable() {
		yield return new WaitForEndOfFrame ();
		setInactive ();
		delay = true;
		yield return new WaitForSeconds (delayTime);
		delay = false;
		setEnabled ();
	}

	// Update is called once per frame
	void Update () { 
		transform.position = pcs.transform.position; //don't need to update position, but doing it for consistency like for scrambler
		if (aFlag && !delay && Input.GetMouseButtonDown (1)) {
			if (pcs.gContact) {
				Instantiate (fieldPrefab, pcs.transform.position, Quaternion.identity);
				StartCoroutine (DelayDisable ());			
			}
		}

	}
}
