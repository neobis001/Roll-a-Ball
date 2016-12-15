using UnityEngine;
using System.Collections;

public class PlayerJetPackScript : MonoBehaviour {
	private Transform playerPos;

	// Use this for initialization
	void Start () {
		playerPos = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform> ();
		transform.position = playerPos.position; //do it early so it comes into screen nicely
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = playerPos.position;
	}
}
