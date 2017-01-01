using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//12/31/16 setup is currently handled by UpgradeManager because Start() order caused some oddities
public class UpgradeButton : MonoBehaviour {
	public RectTransform descriptionRect;
	public string gmBool; //string saying what boolean in gm button is connected to
	  //like an id too for cycling selectionMode
	public string text; //for quick test purposes in Start()

	private Button btn;
	private string currentPivot;
	private bool isInteractable = true;
	private string selectionMode = "normal";
	private UpgradeManager um;

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
		screenPos.x *= Screen.width;
		screenPos.y *= Screen.height;
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
			if (refPos.y > Screen.height) {
				return true;
			} else {
				return false;
			}
		} else if (side == "right") {
			refPos.x += descriptionRect.rect.width;
			if (refPos.x > Screen.width) {
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

	bool clippedLeftEdge() {
		Vector2 refPos = new Vector2();
		refPos += descriptionRect.anchoredPosition;
		Vector2 screenPos = descriptionRect.anchorMin; //anchorMax is the same if anchor is a point
		screenPos.x *= Screen.width;
		screenPos.y *= Screen.height;
		refPos += screenPos;
		refPos.x -= descriptionRect.rect.width;
		if (refPos.x < 0) {
			return true;
		} else {
			return false;
		} 
	}

	bool clippedRightEdge() {
		Vector2 refPos = new Vector2();
		refPos += descriptionRect.anchoredPosition;
		Vector2 screenPos = descriptionRect.anchorMin; //anchorMax is the same if anchor is a point
		screenPos.x *= Screen.width;
		screenPos.y *= Screen.height;
		refPos += screenPos;
		refPos.x += descriptionRect.rect.width;
		if (refPos.x > Screen.width) {
			return true;
		} else {
			return false;
		}
	}


	void movePivot() { //works if anchors are in same place
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
		setPivot ("bl");
		movePivot ();
		checkPivot ();

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
		checkPivot ();
	}


	public void turnOnDescription() {
		Debug.Log ("mouse enter");
		checkPivot ();
		movePivot ();
		descriptionRect.gameObject.SetActive (true);
	}

	public void turnOffDescription() {
		Debug.Log ("mouse exit");
		descriptionRect.gameObject.SetActive (false);
	}

	public bool interactable{
		get { return isInteractable; }
	}

	public string selSelected {
		get { return selectionMode; }
	}

}
