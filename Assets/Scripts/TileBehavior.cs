using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehavior : MonoBehaviour {
	
	private GameObject player;
	private Rigidbody2D rb;
	public bool isVisited = false;

	Animator anim;

	AudioManager audioManager;

	void Start () {

		audioManager = GameObject.FindObjectOfType<AudioManager> ();

		rb = GetComponent<Rigidbody2D> ();
		player = GameObject.FindGameObjectWithTag ("player");
		anim = GetComponent<Animator> ();

		if(transform.parent == null)
			transform.SetParent (GameObject.FindGameObjectWithTag ("foreground").transform);
	}

	void Update () {
		
		if (player != null) {
			if (player.transform.position.y - transform.position.y > 200)
				DestroyObject (this.gameObject);
		}
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
			showExplosion (0f);
			break;

		default:
			break;
		}

		isVisited = true;
	}

	private void enableGravity(float waitFor){
		
		if(!isVisited)
			StartCoroutine (enableTileGravity(waitFor));
	}

	private IEnumerator enableTileGravity(float time){

		yield return new WaitForSeconds(time);

		rb.bodyType = RigidbodyType2D.Dynamic;
		rb.mass = 60;
		rb.gravityScale = 20;
	}

	public GameObject explosion;

	private void showExplosion(float time){
		
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