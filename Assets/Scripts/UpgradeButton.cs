﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//12/31/16 setup is currently handled by UpgradeManager because Start() order caused some oddities
public class UpgradeButton : MonoBehaviour {
	public RectTransform centerDisplay;
	public RectTransform descriptionRect;
	public string gmBool; //string saying what boolean in gm button is connected to
	  //like an id too for cycling selectionMode
	public string text; //for quick test purposes in Start()

	private Button btn;
	private RectTransform centerDisplayReference;
	private string currentPivot; //only needed with checkPivot and setPivot (and movePivot, if putting on corners of button)
	private bool isInteractable = true;
	private RectTransform descriptionReference;
	private string selectionMode = "normal";
	private UpgradeManager um;

	/*
	void centerPivot() {
		movePivot ();

		Vector2 refPos = new Vector2();
		refPos += descriptionRect.anchoredPosition;
		Vector2 screenPos = descriptionRect.anchorMin; //anchorMax is the same if anchor is a point
		float screenWidth = um.cs.referenceResolution.x;
		float screenHeight = um.cs.referenceResolution.y;
		float halfWidth = screenWidth / 2;
		float halfHeight = screenHeight / 2;
		screenPos.x *= screenWidth; //screen sizes vary, which is why Screen.width and height didn't work for a check like this
		//regardless of the screen res, the reference res and rect transforms stay the same, so use reference res
		screenPos.y *= screenHeight;
		refPos += screenPos;

		if (refPos.x < halfWidth && refPos.y < halfHeight) {
			setPivot ("bl");
		} else if (refPos.x < halfWidth && refPos.y >= halfHeight) {
			setPivot ("tl");
		} else if (refPos.x >= halfWidth && refPos.y >= halfHeight) {
			setPivot ("tr");
		} else if (refPos.x >= halfWidth && refPos.y < halfHeight) {
			setPivot ("br");
		} else {
			Debug.LogWarning ("centerPivot() didn't read anything for button " + gameObject.name);
		}

		movePivot ();

	} */

	//centerPivot seems more useful than checkPivot, but coding this was interesting, so I want to keep it here
	/*
	void checkPivot() { //assumes pivot is already placed in bubble location
		//bl check, checking br
		bool clippedTopDown = false;
		bool clippedLeftRight = false;
		if (currentPivot == "bl") {
			clippedLeftRight = clippedEdge ("right");
			clippedTopDown = clippedEdge ("top");
			if (clippedLeftRight && clippedTopDown) {
				setPivot ("tr");
			} else if (clippedLeftRight) {
				setPivot ("br");
			} else if (clippedTopDown) {
				setPivot ("tl");
			}
		} else if (currentPivot == "tl") {
			clippedLeftRight = clippedEdge ("right");
			clippedTopDown = clippedEdge ("bottom");
			if (clippedLeftRight && clippedTopDown) {
				setPivot ("br");
			} else if (clippedLeftRight) {
				setPivot ("tr");
			} else if (clippedTopDown) {
				setPivot ("bl");
			}
		} else if (currentPivot == "tr") {
			clippedLeftRight = clippedEdge("left");
			clippedTopDown = clippedEdge("bottom");
			if (clippedLeftRight && clippedTopDown) {
				setPivot("bl");
			} else if (clippedLeftRight) {
				setPivot("tl");
			} else if (clippedTopDown) {
				setPivot("br");
			}
		} else if (currentPivot == "br") {
			clippedLeftRight = clippedEdge("left");
			clippedTopDown = clippedEdge("top");
			if (clippedLeftRight && clippedTopDown) {
				setPivot("tl");
			} else if (clippedLeftRight) {
				setPivot("bl");
			} else if (clippedTopDown) {
				setPivot("tr");
			}
		} else {
			Debug.LogWarning("checkPivot() didn't read anything. currentPivot value was " + currentPivot);
		}
	}

	bool clippedEdge(string side) {
		Vector2 refPos = new Vector2();
		refPos += descriptionRect.anchoredPosition;
		Vector2 screenPos = descriptionRect.anchorMin; //anchorMax is the same if anchor is a point
		screenPos.x *= um.cs.referenceResolution.x; //screen sizes vary, which is why Screen.width and height didn't work for a check like this
		  //regardless of the screen res, the reference res and rect transforms stay the same, so use reference res
		screenPos.y *= um.cs.referenceResolution.y;
		refPos += screenPos;

		if (side == "left") {
			refPos.x -= descriptionRect.rect.width;
			if (refPos.x < 0) {
				return true;
			} else {
				return false;
			} 
		} else if (side == "top") {
			refPos.y += descriptionRect.rect.height;
			if (refPos.y > um.cs.referenceResolution.y) {
				return true;
			} else {
				return false;
			}
		} else if (side == "right") {
			refPos.x += descriptionRect.rect.width;
			if (refPos.x > um.cs.referenceResolution.x) {
				return true;
			} else {
				return false;
			}
		} else if (side == "bottom") {
			refPos.y -= descriptionRect.rect.height;
			if (refPos.y < 0) {
				return true;
			} else {
				return false;
			}
		} else {
			Debug.LogWarning ("clippedEdge didn't read anything. returning false. side value was " + side);
			return false;
		}

	}
	*/

	void blCornerPivot() {
		setPivot ("bl"); 
		descriptionRect.anchorMin = new Vector2 ();
		descriptionRect.anchorMax = new Vector2 ();

		descriptionRect.anchoredPosition = descriptionReference.anchoredPosition;
	}

	void movePivot() { //works if anchors are in same place
		//description on top doesn't work with scrollbars specifically, even wit RaycastTarget off
		/*
		descriptionRect.anchoredPosition = btn.GetComponent<RectTransform>().anchoredPosition; //with ability to set
		  //Raycast Target, can put description on top of button again
		*/

		RectTransform btnRect = btn.GetComponent<RectTransform>();  
		int btnWidthAdd = (int) (btnRect.rect.width / 2) + 2; //2 pixels extra padding
		int btnHeightAdd = (int) (btnRect.rect.height / 2) + 2;

		Vector2 newPos = btnRect.anchoredPosition;

		switch (currentPivot) {
		case "tl":
			newPos.x += btnWidthAdd;
			newPos.y -= btnHeightAdd;
			break;
		case "bl":
			newPos.x += btnWidthAdd;
			newPos.y += btnHeightAdd;
			break;
		case "tr":
			newPos.x -= btnWidthAdd;
			newPos.y -= btnHeightAdd;
			break;
		case "br":
			newPos.x -= btnWidthAdd;
			newPos.y += btnHeightAdd;
			break;
		default:
			Debug.LogWarning ("movePivot() didn't read anything. currentPivot value was " + currentPivot);
			break;
		}

		descriptionRect.anchoredPosition = newPos;
	}
		

	void setPivot(string pivot) { //tl,bl,tr,br = top left, bottom left, top right, bottom right
			//changing a pivot should move box as needed 

		switch (pivot) {
		case "tl":
			descriptionRect.pivot = new Vector2 (0f, 1f);
			break;
		case "bl":
			descriptionRect.pivot = new Vector2 (0f, 0f);
			break;
		case "tr":
			descriptionRect.pivot = new Vector2 (1f, 1f);
			break;
		case "br":
			descriptionRect.pivot = new Vector2 (1f, 0f);
			break;
		default:
			Debug.LogWarning ("setPivot() switch didn't read anything. corner value was " + pivot.ToString ());
			break;
		}

		currentPivot = pivot;
	}
		
	public void startSetUp() { //apparently, setUpInteractable runs before button is gotten
		  //oddly specifically on scene transition from level to upgrade scene, have UpgradeManager handle setup here
		um = GameObject.FindGameObjectWithTag ("UpgradeManager").GetComponent<UpgradeManager>();
		btn = GetComponent<Button> (); 
		descriptionRect.gameObject.SetActive (false);
		//setPivot ("bl");

		centerDisplay.gameObject.SetActive (false);
		centerDisplayReference = GameObject.FindGameObjectWithTag ("CenterDisplay").GetComponent<RectTransform>();
		descriptionReference = GameObject.FindGameObjectWithTag ("Description").GetComponent<RectTransform> ();

/*		movePivot ();
		checkPivot ();
		movePivot (); //doing move pivot again because checkPivot may have changed pivot based on where
		  //the starting setPivot and movePivot put the description box in
*/
		//centerPivot ();

		blCornerPivot ();

		/*Text txtHolder = GetComponentInChildren<Text> ();
		txtHolder.text = text; */
	}

	public void setUpInteractable(bool isOn) {
		ColorBlock cb = btn.colors;
		cb.disabledColor = um.notInteractableC;
		btn.colors = cb;

		if (isOn) {
			btn.interactable = true;
			isInteractable = true;
		} else {
			btn.interactable = false;
			isInteractable = false;
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

		um.checkOk ();
	}

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

		/*Debug.Log ("rect.width is " + descriptionRect.rect.width.ToString ());
		Debug.Log ("rect.height is " + descriptionRect.rect.height.ToString ());
		Debug.Log("sizeDelta.x.ToString " + descriptionRect.sizeDelta.x.ToString());
		Debug.Log ("sizeDelta.y.ToString " + descriptionRect.sizeDelta.y.ToString ());*/
		/*
		Debug.Log ("ancheroed position is " + descriptionRect.anchoredPosition);
		Debug.Log ("rect position is " + descriptionRect.rect.x.ToString() + " " + descriptionRect.rect.y.ToString());
		*/
		//checkPivot ();
		//centerPivot();
	}


	public void turnOnDescription() {
/*		movePivot (); //because descriptionRect doesn't move automatically with button
		  //move pivot to button with current pivot first, then do a checkPivot check, then move pivot again if changes needed
		checkPivot ();
		movePivot (); */
		centerDisplay.gameObject.SetActive (true);
		centerDisplay.anchoredPosition = centerDisplayReference.anchoredPosition;
		centerDisplay.sizeDelta = centerDisplayReference.sizeDelta;

		//centerPivot ();

		blCornerPivot ();
		descriptionRect.gameObject.SetActive (true);
	}

	public void turnOffDescription() {
		centerDisplay.gameObject.SetActive (false);

		descriptionRect.gameObject.SetActive (false);
	}

	public bool interactable{
		get { return isInteractable; }
	}

	public string selSelected {
		get { return selectionMode; }
	}

}
