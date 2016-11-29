using UnityEngine;
using System.Collections;

public class PlayerNonBeamScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		AudioSource s = GetComponent<AudioSource> ();
		Destroy (gameObject, s.clip.length);
	}

}
