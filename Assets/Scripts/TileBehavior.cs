using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehavior : MonoBehaviour {
	
	private GameObject player;
	private Rigidbody2D rb;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		player = GameObject.FindGameObjectWithTag ("player");
	}

	void Update () {
		
		if (player != null) {
			if (player.transform.position.y - transform.position.y > 10)
				DestroyObject (this.gameObject);
		}
	}

	public void behave(){
		
		switch(this.tag){
		case "CloudTile":
			enableGravity();
			break;
		default:
			break;
		}
	}

	private void enableGravity(){
		
		StartCoroutine (enableTileGravity(1.2f));
	}

	private IEnumerator enableTileGravity(float time){

		yield return new WaitForSeconds(time);

		rb.bodyType = RigidbodyType2D.Dynamic;
		rb.gravityScale = 1;
	}
}