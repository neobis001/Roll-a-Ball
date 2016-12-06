using UnityEngine;
using System.Collections;

public class PlayerScramblerScript : PlayerDefenseScript {
	public Vector3 offset;
	//NOTE: what about disabled and lasting time mode

	//private GameObject player; //temp testing? don't know if really need player in the end
	//no start here for now, overrides superclass's start, don't want that
	
	// Update is called once per frame
	void Update () {
		//transform.position = player.transform.position + offset;
	}

}
