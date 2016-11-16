using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

/*	public Rigidbody rb;

	void Start() {
		Debug.Log ("The rigidbody is" + rb.name.ToString());
	} */

	// Update is called once per frame
	void Update () {
		transform.Rotate(new Vector3 (15, 30, 45) * Time.deltaTime);
	}
}
