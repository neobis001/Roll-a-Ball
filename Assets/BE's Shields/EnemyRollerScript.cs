using UnityEngine;
using System.Collections;

public class EnemyRollerScript : MonoBehaviour
{


	public float fuss = 1f;
	private Vector3 moveDir = new Vector3 ();
	private Rigidbody rb;
	public Transform Player;

	public float firedelay = 6f;

	public GameObject enemyLaser;

	void Awake ()
	{
		rb = GetComponent<Rigidbody> ();
		InvokeRepeating ("Fire", 2f, firedelay);
	}


	// Update is called once per frame
	void Update ()
	{
		if (!Player) {
			CancelInvoke ();
			return;
		}

		moveDir = Player.position - transform.position;
		moveDir.Normalize ();
		moveDir.y = 0;

		Debug.DrawRay (transform.position, moveDir * 5, Color.red);

		rb.AddForce (moveDir * fuss * Time.deltaTime);
	}



	void Fire ()
	{
		GameObject.Instantiate (enemyLaser, transform.position + moveDir * 2, Quaternion.identity);
	}




}
