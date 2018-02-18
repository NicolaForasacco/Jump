using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehavior : MonoBehaviour {
	
	public	GameObject	explosion,
						liftPrefab;

	public	Sprite pressedTile;
	public	bool isVisited = false;
	public	int	maxDistanceFromPlayer = 200;

	private	GameObject player;
	private	Rigidbody2D rb;
	private	Animator anim;
	private	AudioManager audioManager;

	void Start () {

		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		player = GameObject.FindGameObjectWithTag ("player");
		audioManager = GameObject.FindObjectOfType<AudioManager> ();

		if(transform.parent == null)
			transform.SetParent (GameObject.FindGameObjectWithTag ("foreground").transform);
	}

	void Update () {
		
		if (player != null)
		if (player.transform.position.y - transform.position.y > maxDistanceFromPlayer)
			DestroyObject (this.gameObject);
	}



	public void behave(){

		audioManager.makeSound ();

		switch(this.tag){

		case "CloudTile":
			enableGravity(1.2f);
			break;

		case "BounceTile":
			anim.SetTrigger ("Bouncing");
			break;

		case "UselessTile":
			showExplosion (0.3f);
			break;

		case "ColorizeTile":
			anim.SetTrigger ("pressed");

			if (!isVisited) {

				FindObjectOfType<CameraShake> ().startBigShake ();
				FindObjectOfType<CameraController> ().changeRandomBackgroundColor ();
				showExplosion ();
			}
			break;

		case "LiftTile":
			GetComponent<SpriteRenderer> ().sprite = pressedTile;

			if (!isVisited) {
				
				FindObjectOfType<CameraShake> ().startBigShake ();
				Vector2 liftInitialPosition = new Vector2 (transform.position.x, FindObjectOfType<CameraController> ().getMinPosition ());
				
				Instantiate (liftPrefab, liftInitialPosition, Quaternion.identity);
			}
			break;

		default:
			break;
		}

		isVisited = true;
	}

	private void enableGravity(float waitFor = 0f){
		
		if(!isVisited)
			StartCoroutine (enableTileGravity(waitFor));
	}

	private IEnumerator enableTileGravity(float time){

		yield return new WaitForSeconds(time);

		rb.bodyType = RigidbodyType2D.Dynamic;
		rb.mass = 60;
		rb.gravityScale = 20;
	}

	private void showExplosion(float time = 0f){
		
		if(!isVisited)
			StartCoroutine (showExplosionCoroutine(time));
	}

	private IEnumerator showExplosionCoroutine(float time){

		yield return new WaitForSeconds(time);

		Instantiate(explosion, transform.position, Quaternion.identity);
		if(this.CompareTag("UselessTile"))
			GameObject.Destroy (this.gameObject);
	}
}