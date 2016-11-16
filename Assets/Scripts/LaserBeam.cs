using UnityEngine;
using System.Collections;

public class LaserBeam : MonoBehaviour {

	// Use this for initialization
	public int speed = 6;

	private GameObject player;

	void Start () {
		//Debug.Log (transform.forward);
		player = GameObject.FindGameObjectWithTag ("Player");
		//Debug.Log (player.name);
		transform.LookAt (player.transform);
		Destroy (gameObject, 10);
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (transform.forward);
		//transform.Translate (transform.forward * speed * Time.deltaTime);
		transform.Translate(transform.forward.normalized * speed * Time.deltaTime , Space.World);
	}
}
