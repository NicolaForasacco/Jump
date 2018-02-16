using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalLift : MonoBehaviour {

	public GameObject explosion;
	public float	speed = 100,
					sideSpeed = 30,
					fastSpeed = 200,
					fastSideSpeed = 40,
					fastSpeedDuration = 10,
					instantiateExplosionAfter = 4;

	private GameObject player;
	private SpawnTile gameManagerSpawnTile;
	private bool	isLiftWorking = true,
					isFast = false;

	void Start(){
		
		Debug.Log ("Lift spawned");

		player = GameObject.FindGameObjectWithTag ("player");
		gameManagerSpawnTile = GameObject.FindGameObjectWithTag ("GameController").GetComponent<SpawnTile>();
	}

	// Update is called once per frame
	void Update () {

		transform.position += Vector3.up * speed * Time.deltaTime;

		if (isLiftWorking) {
			if (isFast) {

				//va verso l'ultima tile
				if (gameManagerSpawnTile.lastXPosition > transform.position.x)
					transform.position += Vector3.right * fastSideSpeed * Time.deltaTime;
				else
					transform.position -= Vector3.right * fastSideSpeed * Time.deltaTime;
			} else {

				//va verso il giocatore
				if (player.transform.position.x > transform.position.x)
					transform.position += Vector3.right * sideSpeed * Time.deltaTime;
				else
					transform.position -= Vector3.right * sideSpeed * Time.deltaTime;
			}
		}else
			speed -= 30 * Time.deltaTime;


		if (player != null)
		if (isFast && !isLiftWorking )
			StartCoroutine (destroyObjectAfter(instantiateExplosionAfter));
		else if (player.transform.position.y - transform.position.y > 300 ||  transform.position.y - player.transform.position.y > 300)
			DestroyObject (this.gameObject);
		
	}


		
	public void incrementSpeed(){

		//un giocatore è saltato sopra il lift
		speed = fastSpeed;
		isFast = true;
		StartCoroutine (turnOffLift(fastSpeedDuration));
	}


	private IEnumerator turnOffLift(float time){

		yield return new WaitForSeconds(time);

		GetComponent<Animator> ().SetTrigger ("TurnOff");
		isLiftWorking = false;
	}

	private IEnumerator destroyObjectAfter(float time){

		yield return new WaitForSeconds(time);

		Instantiate (explosion, transform.position, Quaternion.identity);
		DestroyObject (this.gameObject);
	}
}
