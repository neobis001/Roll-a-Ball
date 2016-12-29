using UnityEngine;
using System.Collections;

//on turn on, set firePrefab script w/ extra damage

public class PlayerCannonScript : PlayerWeaponScript {
	public GameObject beamToBeFired;
	public GameObject firePrefab;
	public int phlebotinumPercentage; //how much to increase damage

	public override void fireBeam(RaycastHit hit, GameObject autoedEnemy = null) {
		GameObject bfp = Instantiate (beamToBeFired);
		bfp.transform.position = currentSpawner.transform.position;
		if (autoedEnemy != null) {
			bfp.transform.LookAt (autoedEnemy.transform);
		} else {
			bfp.transform.LookAt (hit.point);
		}

		if (phlebotinum) {
			PlayerBeamScript pbs = bfp.GetComponent<PlayerBeamScript> ();
			pbs.givePhlebotinumBoost (phlebotinumPercentage);
		}

		Instantiate (firePrefab, currentSpawner.transform.position, currentSpawner.transform.rotation);

		setAmmo("d");
	}
}
