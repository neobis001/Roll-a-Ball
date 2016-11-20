using UnityEngine;
using System.Collections;

public class AudioClipScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		AudioSource s = GetComponent<AudioSource> ();
		Destroy (gameObject, s.clip.length);
	}

}
