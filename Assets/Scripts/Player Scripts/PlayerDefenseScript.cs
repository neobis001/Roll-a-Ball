using UnityEngine;
using System.Collections;

public class PlayerDefenseScript : MonoBehaviour {
	public PlayerControllerScript pcs;
	public bool runCoroutine = false;

	private bool activeFlag = true;


	void Start() {
		StartCoroutine (Test (runCoroutine));
	}

	//test function to set defense item inactive after some time
	//othewise don't need this
	IEnumerator Test(bool run) {
		if (run) {
			yield return new WaitForSeconds (3);
			Debug.Log ("settingInactive");
			setInactive ();
		}
	}

	//on making this inactive, set the aFlag to false, and cycle right to look for defenses
	protected void setInactive() {
		aFlag = false;
		pcs.reactToDefenseInactive (gameObject);
		gameObject.SetActive (false);
	}

	//property to access whether an item is considered active or not
	//making it a property may be extra, but it works, so whatever
	public bool aFlag {
		get { return activeFlag;}
		set { activeFlag = value; }
	}
}
