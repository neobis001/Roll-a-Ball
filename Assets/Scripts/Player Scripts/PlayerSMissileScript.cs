using UnityEngine;
using System.Collections;

public class PlayerSMissileScript : PlayerWeaponScript {
	public GameObject firePrefab;
	public GameObject missileToBeFired;
	public int phlebotinumPercentage; //how much to increase damage

	public override void fireBeam(RaycastHit hit) {
		GameObject mtbf = (GameObject) Instantiate (missileToBeFired, currentSpawner.transform.position, Quaternion.Euler (-90, 0, 0));
		PlayerMissileScript pms = mtbf.GetComponent<PlayerMissileScript> ();
		pms.isEnemyTarget (hit);
		if (phlebotinum) {
			pms.givePhlebotinumBoost (phlebotinumPercentage);
		}

		Instantiate (firePrefab, currentSpawner.transform.position, Quaternion.identity);

		setAmmo ("d");
	}


}
