using UnityEngine;
using System.Collections;

public class PlayerSMissileScript : PlayerWeaponScript {
	public GameObject missileToBeFired;
	public GameObject firePrefab;

	public override void fireBeam(Vector3 targetLocation) {
		GameObject mtbf = (GameObject) Instantiate (missileToBeFired, currentSpawner.transform.position, Quaternion.Euler (-90, 0, 0));
		mtbf.GetComponent<PlayerMissileScript> ().setHitPoint (targetLocation);
		Debug.Log ("Firing Beam");

		Instantiate (firePrefab, currentSpawner.transform.position, Quaternion.identity);

		setAmmo ("d");
	}


}
