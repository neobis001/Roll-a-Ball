using UnityEngine;
using System.Collections;

//OnTriggerEnter list in here
public class PlayerBeamScript: MonoBehaviour {
	public int damage = 10;
	public GameObject destroyPs;
	public GameObject fireSound;
	public int speed = 6;

	private bool isQuadBeam;

	void Start () {
		Destroy (gameObject, 2);
		if (!isQuadBeam) { //don't want to spawn 4 sounds at once
			Instantiate (fireSound, transform.position, Quaternion.identity);
		}
	}

	//move forward in World Space
	void Update () {
		transform.Translate(transform.forward.normalized * speed * Time.deltaTime , Space.World);
	}

	//when the beam is destroyed, instantiate destroyPs as an explosion
	void OnDestroy() {
		Instantiate (destroyPs, transform.position, Quaternion.identity); 
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Enemy")) {
			EnemyScript es = other.GetComponent<EnemyScript> ();
			es.changeHealth (-damage);
			Destroy (gameObject);
		} else {
			string[] checkList = new string[]{"Player", "Scrambler", "AutoTrigger"};
			foreach (string tag in checkList) {
				if (other.gameObject.CompareTag (tag)) {
					return;
				}
			}
			Destroy (gameObject);
		}
	}

	public void givePhlebotinumBoost(int percentIncrease) {
		damage = (int) (damage *  (1 + percentIncrease/100f)); //can't do augmented op, need to do explicit cast with this percentage stuff
		  //unlike reload time stuff, need this to keep damage at type int
		  //make sure to include an f for float increase
	}

	public void giveQuadDecrease() {
		damage = (int)(damage * 0.5f);
	}

	public bool isQuad {
		get { return isQuadBeam; }
		set { isQuadBeam = value; }
	}

}
