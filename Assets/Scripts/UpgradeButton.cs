using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//12/31/16 setup is currently handled by UpgradeManager because Start() order caused some oddities
public class UpgradeButton : MonoBehaviour {
	public string gmBool; //string saying what boolean in gm button is connected to
	  //like an id too for cycling selectionMode
	public string text; //for quick test purposes in Start()

	private Button btn;
	private bool isInteractable = true;
	private string selectionMode = "normal";
	private UpgradeManager um;

	public void toggleClick() {
		if (selectionMode == "normal") {
			toggle ("selectionSelected");
			um.cycleSelection ("selectionNotSelected", gmBool);
		} else if (selectionMode == "selectionSelected") { 
			toggle ("normal");
			um.cycleSelection ("normal", gmBool);
		} else if (selectionMode == "selectionNotSelected") {
			toggle ("selectionSelected");
			um.cycleSelection ("selectionNotSelected", gmBool);
		} else {
			Debug.LogWarning ("toggleClick() didn't read anything");
		}
	}

	public void toggle(string mode) { //for toggling via code
		if (!isInteractable) {
			return;
		}

		ColorBlock cb = btn.colors; //can't assign to btn.colors.normalColor directly
		//use ColorBlock (Unity Answers: How to change Normal color...)
		//however, can get directly
		if (mode == "normal") {
			cb.normalColor = um.originalNormalC;
			cb.highlightedColor = um.originalNormalC;
			cb.pressedColor = um.originalNormalC;
			btn.colors = cb;
			selectionMode = "normal";

		} else if (mode == "selectionSelected") {
			cb.normalColor = um.selectionSelectedC;
			cb.highlightedColor = um.selectionSelectedC;
			cb.pressedColor = um.selectionSelectedC;
			btn.colors = cb;
			selectionMode = "selectionSelected";
		} else if (mode == "selectionNotSelected") {
			cb.normalColor = um.selectionNotSelectedC;
			cb.highlightedColor = um.selectionNotSelectedC;
			cb.pressedColor = um.selectionNotSelectedC;
			btn.colors = cb;
			selectionMode = "selectionNotSelected";
		} else {
			Debug.LogWarning ("toggle() didn't read anything");
		}
	}

	public void startSetUp() { //apparently, setUpInteractable runs before button is gotten
		  //oddly specifically on scene transition from level to upgrade scene, have UpgradeManager handle setup here
		um = GameObject.FindGameObjectWithTag ("UpgradeManager").GetComponent<UpgradeManager>();
		btn = GetComponent<Button> (); 
		Text txtHolder = GetComponentInChildren<Text> ();
		txtHolder.text = text;
	}

	public void setUpInteractable(bool isOn) {
		if (isOn) {
			btn.interactable = true;
			isInteractable = true;
		} else {
			btn.interactable = false;
			isInteractable = false;
		}
	}

	public bool interactable{
		get { return isInteractable; }
	}

	public string selSelected {
		get { return selectionMode; }
	}


}
