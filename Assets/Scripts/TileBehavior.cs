using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehavior : MonoBehaviour {

	public	GameObject	explosion;
	public	bool isVisited;
	private	int	maxDistanceFromPlayer = 400;

	private	GameObject player;
	private	Rigidbody2D rb;
	private	Animator anim;
	private AudioManager am;

	void Start () {
		
		am = FindObjectOfType<AudioManager> ();
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		player = GameObject.FindGameObjectWithTag ("player");

		if(transform.parent == null)
			transform.SetParent (GameObject.FindGameObjectWithTag ("foreground").transform);
	}

	void Update () {

		if (player != null)
		if (player.transform.position.y - transform.position.y > maxDistanceFromPlayer)
			DestroyObject (this.gameObject);
	}



	public void behave(){

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

				am.changeBackgroundTrack ();
				FindObjectOfType<CameraShake> ().startBigShake ();
				am.playExplosionSound ();
				FindObjectOfType<CameraController> ().changeRandomBackgroundColor ();
				FindObjectOfType<PreferenceManager> ().increaseTotalPaintButton ();
				Instantiate(explosion, transform.position, Quaternion.identity);
				FindObjectOfType<CameraShake> ().startSlowMotion();
			}

			break;
		default: break;
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

		am.playExplosionSound ();
		Instantiate(explosion, transform.position, Quaternion.identity);

		GameObject.FindObjectOfType<CameraShake> ().startShake ();

		if(this.CompareTag("UselessTile"))
			GameObject.Destroy (this.gameObject);
	}
}
