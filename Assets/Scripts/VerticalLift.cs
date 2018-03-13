using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalLift : MonoBehaviour {
	/*
	public GameObject explosion;
	public float	speed = 100,
					fastSpeed = 200,
					sideSpeed = 30,
					fastSideSpeed = 40,
					fastSpeedDuration = 5,
					maxDistanceFromPlayer = 300,
					instantiateExplosionAfter = 4;

	private GameObject player;
	private SpawnTileManager gameManagerSpawnTile;
	private float difference;
	private bool isFast = false;

	public bool isLiftWorking = true;

	void Start(){
		
		player = GameObject.FindGameObjectWithTag ("player");
		gameManagerSpawnTile = GameObject.FindGameObjectWithTag ("GameController").GetComponent<SpawnTileManager>();
		GetComponent<Animator> ().SetTrigger ("TurnOn");
	}
		
	void Update () {

		transform.position += Vector3.up * speed * Time.deltaTime;

		if (isLiftWorking) {

			if (isFast)
				goTowardsLastTile ();
			else
				goTowardsPlayer ();
			
		} else {

			goTowardsLastTile ();
			speed -= fastSpeed/1.5f * Time.deltaTime;
		}

		if (player != null) {
			
			if (isFast && !isLiftWorking)
				StartCoroutine (destroyObjectAfter (instantiateExplosionAfter));
			else if (player.transform.position.y - transform.position.y > maxDistanceFromPlayer ||
			        transform.position.y - player.transform.position.y > maxDistanceFromPlayer)
				DestroyObject (this.gameObject);
		}
	}

	void goTowardsLastTile(){
		
		difference = gameManagerSpawnTile.lastXPosition - transform.position.x;

		if (difference >= fastSideSpeed || difference <= -fastSideSpeed) {

			if(difference > fastSideSpeed)
				transform.position += Vector3.right * fastSideSpeed * Time.deltaTime;
			else
				transform.position -= Vector3.right * fastSideSpeed * Time.deltaTime;
		}
	}

	void goTowardsPlayer(){
		
		if (player.transform.position.x > transform.position.x)
			transform.position += Vector3.right * fastSideSpeed * Time.deltaTime;
		else
			transform.position -= Vector3.right * fastSideSpeed * Time.deltaTime;
	}


		
	public void incrementSpeed(){

		//un giocatore è saltato sopra il lift
		speed = fastSpeed;
		isFast = true;
		StartCoroutine (turnOffLift(fastSpeedDuration));
	}


	private IEnumerator turnOffLift(float time = 0){

		yield return new WaitForSeconds(time);

		GetComponent<Animator> ().SetTrigger ("TurnOff");
		isLiftWorking = false;
	}

	private IEnumerator destroyObjectAfter(float time = 0){

		yield return new WaitForSeconds(time);

		Instantiate (explosion, transform.position, Quaternion.identity);
		DestroyObject (this.gameObject);
	}*/
}
