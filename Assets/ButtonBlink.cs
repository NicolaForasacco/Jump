using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBlink : MonoBehaviour {

	public Sprite normalButton, blinkButton;
	public float timeBetweenSpriteChange = 0.8f;
	public bool blinkActive = false;

	Image spriteRenderer;
	bool isNormalButtonOn = true;
	float timeElapsed;

	void Start () {
		
		spriteRenderer = GetComponent<Image> ();
	}

	void Update () {
		
		if (blinkActive) {
			timeElapsed -= Time.deltaTime;

			if (timeElapsed <= 0) {

				if (isNormalButtonOn)
					spriteRenderer.sprite = blinkButton;
				else
					spriteRenderer.sprite = normalButton;
				
				isNormalButtonOn = !isNormalButtonOn;

				timeElapsed = timeBetweenSpriteChange;
			}
		}
	}
}
