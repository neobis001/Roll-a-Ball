using UnityEngine;
using System.Collections;

public class PlayerRepairKitScript : PlayerDefenseScript {
	//lifetime in seconds
	public int delayTime;
	//to represent repairing visually
	public GameObject repairPrefab;

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
		if (aFlag && !delay && Input.GetMouseButtonDown (1)) {
			Instantiate (repairPrefab, pcs.transform.position, Quaternion.identity);
			StartCoroutine (DelayDisable ());
		}
	}
}
