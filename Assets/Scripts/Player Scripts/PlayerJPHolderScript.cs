using UnityEngine;
using System.Collections;


//Player Jet Pack Holder Script
public class PlayerJPHolderScript : MonoBehaviour {
	public GameObject button;
	public int delayTime; //should be greater than floatTime plus upTime plus a little extra
	public float floatTime;
	public GameObject jetPack;
	public PlayerControllerScript pcs;
	public float upTime;
	public float yForce; 


	private float floatLocation; //y location of tank at the moment it stops going up
	private GameManager gm;
	private bool isGoingUp = false; //don't need a isFloating bool, just found that by trial and error
	private bool jetPackStarted = false;


	void Start() {
		gm = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager>();
	}

	/*
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.M) && !jetPackStarted) { //this doesn't test for if player is on ground 
			StartCoroutine (jetTimer ());
		}
	} */ 

	void FixedUpdate() {
		if (isGoingUp) {
			pcs.rbProp.AddForce (new Vector3 (0, yForce)); //need to put a lot to make the ball actually go up
			 //with mass 1 and g = 9.8, yForce > 9.8 at least
		}
	}

	//this handles code for timer, rb stuff, and gm stuff
	IEnumerator jetTimer() {
		GameObject jPack = Instantiate (jetPack); //on start, the script in it positions as needed
		jetPackStarted = true;
		isGoingUp = true;
		gm.changeJetIcon (false);
		yield return new WaitForSeconds (upTime);
		isGoingUp = false; //from true to false after upTime, signals for player to stop going up
		pcs.rbProp.velocity = new Vector3 (pcs.rbProp.velocity.x, 0, pcs.rbProp.velocity.z); // can't set just y
		pcs.rbProp.constraints = RigidbodyConstraints.FreezePositionY; //don't know what'll happen on unfreeze
		 //maybe setting velocity to 0 will make it work intuitively
		yield return new WaitForSeconds (floatTime); 
		Destroy (jPack);
		pcs.rbProp.constraints = RigidbodyConstraints.None;
		yield return new WaitForSeconds (delayTime - floatTime - upTime); //delay time happens while float time and uptime count down
		 //so subtract to account for relative left over time
		jetPackStarted = false;
		gm.changeJetIcon (true);
	}

	public void startJetPack() {
		if (!jetPackStarted) {
			StartCoroutine (jetTimer ());
		}
	}
}
