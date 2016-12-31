using UnityEngine;
using System.Collections;

public class PlayerQuadCScript : PlayerWeaponScript {
	public GameObject beamToBeFired;
	public GameObject firePrefab;
	public GameObject fireSound; //4 beams fired at once, still want just one sound
	public GameObject[] spawners; 
	public int phlebotinumPercentage; //how much to increase damage

	public override void fireBeam(RaycastHit hit, GameObject autoedEnemy = null) {
		foreach (GameObject spawner in spawners) {
			GameObject bfp = (GameObject) Instantiate (beamToBeFired, spawner.transform.position, transform.rotation);
			PlayerBeamScript pbs = bfp.GetComponent<PlayerBeamScript> ();
			pbs.giveQuadDecrease ();
			pbs.isQuad = true;

			if (phlebotinum) {
				pbs.givePhlebotinumBoost (phlebotinumPercentage);
			}

			Instantiate (firePrefab, spawner.transform.position, spawner.transform.rotation);
		}

		Instantiate (fireSound, transform.position, Quaternion.identity);
		setAmmo("d");
	}
}
