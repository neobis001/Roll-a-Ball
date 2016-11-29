using UnityEngine;
using System.Collections;

public class PlayerCannonScript : PlayerWeaponScript {
	public GameObject beamToBeFired;
	public GameObject firePrefab;

	public override void fireBeam(Vector3 targetLocation) {
		GameObject bfp = Instantiate (beamToBeFired);
		bfp.transform.position = currentSpawner.transform.position;
		bfp.transform.LookAt (targetLocation);

		Instantiate (firePrefab, currentSpawner.transform.position, currentSpawner.transform.rotation);

		setAmmo("d");
	}
}
