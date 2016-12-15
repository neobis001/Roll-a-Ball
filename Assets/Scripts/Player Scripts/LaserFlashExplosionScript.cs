using UnityEngine;
using System.Collections;

public class LaserFlashExplosionScript : MonoBehaviour {

	public float timeDestroy; //getting particle system duration doesn't mean entire particle animation, so just guess


	// Use this for initialization
	void Start () {
		Destroy (gameObject, timeDestroy);
	}
		
}
