using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public Rigidbody2D rb;

	public float sideSpeed;
	public float slowSideSpeed;
	private float currentSideSpeed;

	public float jumpSpeed;

	public bool onGround = true;


	public SpawnTile spawnManager;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{


		currentSideSpeed = (onGround) ? sideSpeed : slowSideSpeed;

		//if (Input.GetKey ("a"))
		transform.Translate (new Vector2(Time.deltaTime * currentSideSpeed*Input.GetAxis("Horizontal"),0));

		//if (Input.GetKey ("d"))
		//	transform.Translate (Vector2.right * Time.deltaTime * currentSideSpeed);

		if (Input.GetButton ("Jump") && rb.velocity.y == 0) {

			rb.AddForce (new Vector2(0, jumpSpeed * Time.deltaTime), ForceMode2D.Impulse);
			onGround = false;
		}
		/*
		if (Input.GetKey ("w") && rb.velocity.y == 0) {
			
			rb.AddForce (new Vector2(0, jumpSpeed * Time.deltaTime), ForceMode2D.Impulse);
			onGround = false;
		}*/
	}

	void OnCollisionEnter2D(Collision2D collision){

		onGround = true;
		spawnManager.spawnMultipleGrounds ();

		StartCoroutine (enalbeTileGravity(1.2f,collision.gameObject));
	}

	IEnumerator enalbeTileGravity(float time, GameObject tile){

		yield return new WaitForSeconds(time);

		(tile.GetComponent<Rigidbody2D> ()).bodyType = RigidbodyType2D.Dynamic;
		(tile.GetComponent<Rigidbody2D> ()).gravityScale = 1;
	}
}
