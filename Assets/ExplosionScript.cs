using UnityEngine;
using System.Collections;

public class ExplosionScript : MonoBehaviour {

	private ParticleSystem ps;

	// Use this for initialization
	void Start () {
		ps = GetComponent<ParticleSystem> ();
		Destroy (gameObject, ps.duration);
	}

}
