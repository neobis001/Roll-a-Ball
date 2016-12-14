using UnityEngine;
using System.Collections;

//used by player and enemy projectiles to do extra fx stuff like explosion/sound handling
public class ExtraFXScript : MonoBehaviour {
	public bool isThereALight = true;
	public GameObject lightObject; //for destroying point light when explosion finishes

	// Use this for initialization
	void Start () {
		AudioSource s = GetComponent<AudioSource> ();
		Destroy (gameObject, s.clip.length);

		if (isThereALight) { //if there is, destroy it at moment particle system is destroyed
			ParticleSystem ps = GetComponent<ParticleSystem> ();
			Destroy (lightObject, ps.duration);
		}

	}

}
