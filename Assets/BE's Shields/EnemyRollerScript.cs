using UnityEngine;
using System.Collections;

public class EnemyRollerScript : MonoBehaviour
{


	public float fuss = 1f;
	private Vector3 moveDir = new Vector3 ();
	private Rigidbody rb;
	public Transform Player;


	void Awake ()
	{
		rb = GetComponent<Rigidbody> ();
	}


	// Update is called once per frame
	void Update ()
	{
		moveDir = Player.position - transform.position;

		moveDir.Normalize ();
		moveDir.y = 0;

		Debug.DrawRay (transform.position, moveDir * 5, Color.red);

		rb.AddForce (moveDir * fuss * Time.deltaTime);


	}
}
