using UnityEngine;
using System.Collections;

public class PlayerDefenseScript : PlayerItemScript {
	public int keyNumber; //what num key used to set this active
	public PlayerControllerScript pcs;
	//probs don't need this
	//public string id;
	//whether item is unlocked or not
	//public bool unlocked = false;
	//button to be activated on Ui if it's true
	public GameObject uiObject;

	private bool activeFlag = true;
	private bool enabledFlag = true;

	//on making this inactive and disabled, set the aFlag to false, and cycle right to look for defenses
	protected void setInactive() {
		aFlag = false;
		eFlag = false;
		pcs.reactToDefenseDisabled (gameObject);
	}

	//on making this enabled, let pcs react as needed
	protected void setEnabled() {
		eFlag = true;
		pcs.reactToDefenseEnabled (gameObject);
	}

	//property to access whether an item is considered active or not
	//making it a property may be extra, but it works, so whatever
	public bool aFlag {
		get { return activeFlag;}
		set { activeFlag = value; }
	}

	//property to access whether it's enabled or not
	public bool eFlag {
		get { return enabledFlag;}
		set { enabledFlag = value; }
	}
}
