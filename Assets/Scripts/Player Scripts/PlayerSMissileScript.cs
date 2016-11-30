using UnityEngine;
using System.Collections;

public class PlayerSMissileScript : PlayerWeaponScript {
	public GameObject missileToBeFired;
	public GameObject firePrefab;

	public override void fireBeam(RaycastHit hit) {
		GameObject mtbf = (GameObject) Instantiate (missileToBeFired, currentSpawner.transform.position, Quaternion.Euler (-90, 0, 0));
		PlayerMissileScript pms = mtbf.GetComponent<PlayerMissileScript> ();
		pms.isEnemyTarget (hit);

		Instantiate (firePrefab, currentSpawner.transform.position, Quaternion.identity);

		setAmmo ("d");
	}


}
