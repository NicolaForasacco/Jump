using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehavior : MonoBehaviour {
	
	private GameObject player;
	private Rigidbody2D rb;

	Animator anim;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		player = GameObject.FindGameObjectWithTag ("player");
		anim = GetComponent<Animator> ();
	}

	void Update () {
		
		if (player != null) {
			if (player.transform.position.y - transform.position.y > 200)
				DestroyObject (this.gameObject);
		}
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
			enableGravity (0.2f);
			break;
		default:
			break;
		}
	}

	private void enableGravity(float waitFor){
		
		StartCoroutine (enableTileGravity(waitFor));
	}

	private IEnumerator enableTileGravity(float time){

		yield return new WaitForSeconds(time);

		rb.bodyType = RigidbodyType2D.Dynamic;
		rb.gravityScale = 1;
	}
}