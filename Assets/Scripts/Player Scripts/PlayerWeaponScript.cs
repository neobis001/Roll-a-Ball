 using UnityEngine;
using System.Collections;

public class PlayerWeaponScript : PlayerItemScript {

	public int ammo = 5;
	public int autoReloadDecrease; //percentage decrease 
	public GameObject button;
	public AudioSource emptySound;
	public int magazineDecrease; //a percentage
	public int phlebotinumReloadDecrease; //percentage decrease in time reload
	public AudioSource reloadSound;
	public float reloadTime; //float so can do a decimal percentage decrease

	protected GameObject currentSpawner;
	protected bool phlebotinum = false; //make protected so derived classes can check it when instantiating projectiles
	protected bool canFireAuto = true; //combined with autoDelay bool, derived so overrided fireBeam can access
	 //this wasn't made for checking reloading status, made isReloading for that

	private bool autoReload = false; //made private unlike phlebotinum, doesn't need derived access
	private int initialAmmo;
	private bool isReloading; //for preventing another reloading while one is taking place
	private GameManager gm; //for accessing text only in reload case, else player handles ammo text change


	void Start() {
		gm = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager>();
		initialAmmo = ammo;
		Transform[] possibleObjects = GetComponentsInChildren<Transform> ();
		foreach (Transform po in possibleObjects) {
			if (po.gameObject.CompareTag ("TurretSpawner")) {
				currentSpawner = po.gameObject;
				break;
			}
		}

	}

	IEnumerator Reload() {
		isReloading = true;
		reloadSound.Play ();
		gm.setAmmoText ("Reloading...");
		yield return new WaitForSeconds (reloadTime);
		isReloading = false;
		ammo = initialAmmo;
		gm.setAmmoText (ammo.ToString ());
	}


/*
	protected IEnumerator AutoDelay(RaycastHit hit, GameObject autoedEnemy = null) {
		fireBeam (hit, autoedEnemy); //trying a wait for end of frame may start coroutine twice, causing 0 ammo problem while text still at one
		  //just remove it
		  //knowing debugging, could pause on instant of 0 fire
		canFireAuto = false;
		yield return new WaitForSeconds (autoDelay);
		canFireAuto = true;
	}

	public void autoBeam(RaycastHit hit, GameObject autoedEnemy = null) { //the autoedEnemy parameter is for missile script only, optional for beam
		if (canFireAuto && !isReloading) { //canFireAuto is for shot delay, and !isRealoding is reload check
			StartCoroutine (AutoDelay (hit, autoedEnemy));
		}
	} */


	public virtual void fireBeam(RaycastHit hit, GameObject autoedEnemy = null) { 
		
	}

	//player accesses this, a play for reloading is done in Reload coroutine
	public void playEmpty() {
		emptySound.Play ();
	}

	public void setAmmo(string flag) {
		if (flag == "d") {
			ammo--;
		} else if (flag == "r" && !isReloading && ammo != initialAmmo) {
			StartCoroutine (Reload ());
		}
	}

	public bool Reloading { //for player to check if in middle of reloading to say whether to fire or not
		get { return isReloading; }
	}

	public bool aReload {
		get { return autoReload;}
		set { autoReload = true; 
			reloadTime *= (1 - autoReloadDecrease / 100f); //make sure to put f somewhere so that it's a float decrease
			initialAmmo = (int) (initialAmmo * (1 - magazineDecrease/100f)); //value is truncated, not rounded
			ammo = initialAmmo; //need to update ammo after ammo drop, but text set is still being done by player
		}
	}

	public bool pBotinum { 
		set { phlebotinum = value;
			if (value == true) { //if set true, implies a decrease in realod time, so put it here
				reloadTime *= (1 - phlebotinumReloadDecrease / 100f); //make sure to put f somewhere so that it's a float decrease
			} 
		}
	}

}
