using UnityEngine;
using System.Collections;

public class ExternalScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	//turn on the first scrambler after 10 seconds
	IEnumerator Timer() {
		yield return new WaitForSeconds (10);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
