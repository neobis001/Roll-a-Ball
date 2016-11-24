using UnityEngine;
using System.Collections;

public class PlayerWeaponScript : MonoBehaviour {

	public int ammo = 5;
	public int beamTime = 2;
	public GameObject beamToBeFired;
	public AudioSource reloadSound;
	public AudioSource emptySound;
	public GameObject firePrefab;

	private GameObject currentSpawner;

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

	public void fireBeam(Vector3 targetLocation) {
		GameObject bfp = Instantiate (beamToBeFired);
		bfp.transform.position = currentSpawner.transform.position;
		bfp.transform.LookAt (targetLocation);

		Instantiate (firePrefab, currentSpawner.transform.position, currentSpawner.transform.rotation);

		setAmmo("d");
	}

	public void playEmpty() {
		emptySound.Play ();
	}

	public void playReload() {
		reloadSound.Play ();
	}
}
