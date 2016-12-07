using UnityEngine;
using System.Collections;

public class ShieldEnter : MonoBehaviour
{

	ForceFieldScript ownerScript;

	public bool destoryIncoming = false;
	public GameObject esplossion;


	void OnTriggerEnter (Collider other)
	{
		if (other.CompareTag ("EnemyProjectile")) {

			Bullets bull;
			bull = other.gameObject.GetComponent<Bullets> ();

			if (bull != null) {
				bull.destoryBull ();
			}

		}
	}







}
