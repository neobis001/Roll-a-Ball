using UnityEngine;
using System.Collections;

public class GroundCheckScript : MonoBehaviour {

	public PlayerControllerScript pcs;
	public float yCheck; //amount y distance to do raycast check relative to the gameObject's transform
	public float yOffset; //amount y distance relative to player transform

	private LayerMask lm;

	void Start() {
		string[] layerStrings = new string[1]; //9 for number of default layers, 0-9 excluding 8: AutoTrigger
		/*
		int indexCounter = 0; //need to separate indexing from numbers to put in given index
		for (int i = 0; i < 10; i++) {
			if (i != 8) {
				layerStrings [indexCounter] = LayerMask.LayerToName (i);
				indexCounter++;
			}
		} */
		layerStrings [0] = "Environment";
		lm = LayerMask.GetMask (layerStrings);
	}

	void Update() {
		transform.position = pcs.transform.position + new Vector3 (0, yOffset);

		RaycastHit hit;
		if (Physics.Raycast (transform.position, -Vector3.up, out hit, yCheck, lm)) {
			if (hit.transform.gameObject.layer == 9) {
				pcs.gContact = true;
			}
		} else {
			pcs.gContact = false;
		}
	}

	/*
	void OnTriggerEnter(Collider col) {
		Debug.Log (col.gameObject.layer);
		if (col.gameObject.layer == 9) { //not including an else statement because if trigger collides w/ missile for example
			//while on ground (not really possible, but just in case), envContact = false, could be confusing
			//if player lands on enemy, shouldn't need to change to false, it should already be false
			pcs.gbContact = true; 
		}
	}

	void OnTriggerExit(Collider col) {
		if (col.gameObject.layer == 9) { //same comment as above
			pcs.gbContact = false;
		}
	}*/

}
