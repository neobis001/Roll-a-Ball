using UnityEngine;
using System.Collections;

public class EnemyBeam : MonoBehaviour, Bullets
{

	// Use this for initialization
	public int speed = 6;
	public int damage = -10;

	private GameManager gm;
	private GameObject player;

	public GameObject particles;

	void Start ()
	{
		
		player = GameObject.FindGameObjectWithTag ("Player");
		transform.LookAt (player.transform);
		GameObject gmObject = GameObject.FindWithTag ("GameManager");
		gm = gmObject.GetComponent<GameManager> ();

		Destroy (gameObject, 10);

	}

	void OnTriggerEnter (Collider other)
	{
		Destroy (gameObject);
		if (other.gameObject.CompareTag ("Player")) {
			gm.changeHealth (damage);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.Translate (transform.forward.normalized * speed * Time.deltaTime, Space.World);
	}


	public void destoryBull ()
	{
		GameObject.Instantiate (particles, transform.position, Quaternion.identity);
		Destroy (gameObject);
	}

}
