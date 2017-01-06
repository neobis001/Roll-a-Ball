using UnityEngine;
using System.Collections;

public class PlayerRepairKitScript : PlayerDefenseScript {
	public int delayTime; 	//lifetime in seconds
	public GameObject repairPrefab; //to represent repairing visually

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
		transform.position = pcs.transform.position; //don't need this, just doing it for consistencey with scrambler
		if (aFlag && !delay && Input.GetMouseButtonDown (1)) {
			if (pcs.gContact) {
				Instantiate (repairPrefab, pcs.transform.position, Quaternion.identity);
				StartCoroutine (DelayDisable ());
			}
		}
	}
}
