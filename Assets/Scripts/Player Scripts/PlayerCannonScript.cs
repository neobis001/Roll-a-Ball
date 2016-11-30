using UnityEngine;
using System.Collections;

public class PlayerCannonScript : PlayerWeaponScript {
	public GameObject beamToBeFired;
	public GameObject firePrefab;

	public override void fireBeam(RaycastHit hit) {
		GameObject bfp = Instantiate (beamToBeFired);
		bfp.transform.position = currentSpawner.transform.position;
		bfp.transform.LookAt (hit.point);

		Instantiate (firePrefab, currentSpawner.transform.position, currentSpawner.transform.rotation);

		setAmmo("d");
	}
}
