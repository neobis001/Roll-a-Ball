using UnityEngine;
using System.Collections;

public class PlayerDefenseScript : MonoBehaviour {
	public PlayerControllerScript pcs;
	public bool runCoroutine = false;

	private bool activeFlag = true;


	void Start() {
		StartCoroutine (Test (runCoroutine));
	}

	IEnumerator Test(bool run) {
		if (run) {
			Debug.Log ("before yield statement");
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
		
	public bool aFlag {
		get { return activeFlag;}
		set { activeFlag = value; }
	}
}
