using UnityEngine;
using System.Collections;

//for sound prefab, not player's defense item prefab
public class FieldSoundScript : MonoBehaviour { 

	// Use this for initialization
	void Start () {
		Destroy (gameObject, GetComponent<AudioSource> ().clip.length);
	}
		
}
