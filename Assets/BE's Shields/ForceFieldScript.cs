using UnityEngine;
using System.Collections.Generic;

public class ForceFieldScript : MonoBehaviour
{
	//Please... put a sphere for goodnessake
	public GameObject Shield;
	//reference to the object after instantiated
	private GameObject instShield;

	[Header ("Shield Properties")]
	//shield size
	public float curShieldDiameter = 4f;
	//shield size
	public float shieldDiameter = 4f;
	//shield Min Size
	public float shieldMinDiameter = 1.2f;
	//difference
	private float diaDif = 0f;

	//reactionForce
	public float reactForce = 500f;

	//Asthetics
	//rotation speed of shield
	public float revolveSpeed = 2f;
	//transparency of shield
	public float minTransp = 0.1f;
	public float maxTransp = 0.9f;
	private float transpDif = 0f;
	//are shields up?
	bool shieldsUp = false;
	float shieldDecay = 0.01f;


	void ShieldsUp ()
	{
		//shields are now up Cap'n
		shieldsUp = true;
		//update the current store of shield
		curShieldDiameter = shieldDiameter;
		//create a shield object
		Object inst = Instantiate (Shield, transform.position, Quaternion.identity);
		instShield = (GameObject)inst;
		//turn the collider into a trigger
		instShield.GetComponent<Collider> ().isTrigger = true;
		//make sure the shield is correctly sized
		instShield.transform.localScale = new Vector3 (shieldDiameter, shieldDiameter, shieldDiameter);
		//make child of player
		instShield.transform.parent = transform;
		//asthetics
		transpDif = maxTransp - minTransp;
		diaDif = shieldDiameter - shieldMinDiameter;

	}


	// Update is called once per frame
	void Update ()
	{

		//EnemyRepel ();
		ProjectileDefence ();

		if (shieldsUp)
			DecayShield ();



	}


	void ProjectileDefence ()
	{
		if (Input.GetKeyDown (KeyCode.H) && !shieldsUp) {

			ShieldsUp ();
			instShield.GetComponent<ShieldEnter> ().destoryIncoming = true;

		}
	}


	void DecayShield ()
	{
		Vector4 temp = instShield.GetComponent<Renderer> ().material.color;
		temp.w -= shieldDecay * Time.deltaTime;
		instShield.GetComponent<Renderer> ().material.color = temp;

		if (temp.w <= 0) {


			Destroy (instShield);
			shieldsUp = false;


		}

	}



	void EnemyRepel ()
	{

		//instShield.transform.position = transform.position;

		Collider[] inbounds = Physics.OverlapSphere (transform.position, shieldDiameter / 2);
		Transform closestEnemy = null;

		for (int i = 0; i < inbounds.Length; i++) {

			if (inbounds [i].CompareTag ("Enemy")) {

				closestEnemy = inbounds [i].transform;			

				if (Vector3.Distance (transform.position, inbounds [i].transform.position) < Vector3.Distance (transform.position, closestEnemy.transform.position))
					closestEnemy = inbounds [i].transform;
			}
		}


		if (!closestEnemy) {
			instShield.transform.localScale = new Vector3 (shieldDiameter, shieldDiameter, shieldDiameter);

			Vector4 temp = instShield.GetComponent<Renderer> ().material.color;
			temp.w = minTransp;
			instShield.GetComponent<Renderer> ().material.color = temp;

			return;

		} else {
			curShieldDiameter = Vector3.Distance (transform.position, closestEnemy.position) * 2;
			curShieldDiameter = Mathf.Clamp (curShieldDiameter, shieldMinDiameter, shieldDiameter);
			instShield.transform.localScale = new Vector3 (curShieldDiameter, curShieldDiameter, curShieldDiameter);


			Vector4 temp = instShield.GetComponent<Renderer> ().material.color;
			temp.w = Mathf.Lerp (maxTransp, minTransp, curShieldDiameter / shieldDiameter);

			instShield.GetComponent<Renderer> ().material.color = temp;

			//Mathf.Lerp (minTransp, maxTransp, curShieldDiameter / shieldDiameter);


			if (curShieldDiameter == shieldMinDiameter) {

				Vector3 taraDir = Vector3.Normalize (closestEnemy.position - transform.position);
				taraDir.y = 0.4f;
				closestEnemy.GetComponent<Rigidbody> ().AddForce (taraDir * reactForce);

			}

		}



	}






}
