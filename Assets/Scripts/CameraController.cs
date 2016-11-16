﻿using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	//test
	public GameObject player;
	private Vector3 offset;

	// Use this for initialization
	public void Start () {
		offset = transform.position - player.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.position = player.transform.position + offset;
	}
}
