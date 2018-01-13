using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkDestroyer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		destroy (1f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void destroy(float time){
		StartCoroutine (destroyCoroutine(time));
	}

	private IEnumerator destroyCoroutine(float time){

		yield return new WaitForSeconds(time);

		GameObject.Destroy (this.gameObject);
	}
}
