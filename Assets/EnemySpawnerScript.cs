using UnityEngine;
using System.Collections;

public class EnemySpawnerScript : MonoBehaviour {

	public GameObject beam;
	public float delay;
	public GameManager gm;

	// Use this for initialization
	void Start () {
		StartCoroutine (Timer());
	}

	IEnumerator Timer() {
		while (true) {
			yield return new WaitForSeconds (delay);
			GameObject beamInstance = Instantiate (beam);
			beamInstance.transform.position = transform.position;
		}
	}

	void Update() {
		if (gm.gameisOver == true) {
			Destroy (gameObject);
		}
	}

}
