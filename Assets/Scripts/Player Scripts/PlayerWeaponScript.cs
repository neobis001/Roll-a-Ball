using UnityEngine;
using System.Collections;

public class PlayerWeaponScript : MonoBehaviour {

	public int ammo = 5;
	public AudioSource reloadSound;
	public AudioSource emptySound;

	protected GameObject currentSpawner;

	void Start() {
		Transform[] possibleObjects = GetComponentsInChildren<Transform> ();
		foreach (Transform po in possibleObjects) {
			if (po.gameObject.CompareTag ("TurretSpawner")) {
				currentSpawner = po.gameObject;
				break;
			}
		}
	}

	public void setAmmo(string flag) {
		if (flag == "d") {
			ammo--;
		} else if (flag == "r") {
			ammo = 5;
		}
	}

	public virtual void fireBeam(Vector3 targetLocation) {
	
	}

	public void playEmpty() {
		emptySound.Play ();
	}

	public void playReload() {
		reloadSound.Play ();
	}
}
