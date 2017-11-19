using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour {

	public Rigidbody2D rb;
	public CameraController cameraController;
	public GameManager gm;

	public GameObject[] pencilPrefab;

	public float sideSpeed, 
					slowSideSpeed, 
					jumpSpeed,
					slowJumpSpeed;

	public bool isJumping = false, jumped = false;

	public SpawnTile spawnManager;

	public Text scoreText, 
				pointsText;

	private float currentSideSpeed, 
					horizontalSpeed, 
					maxHorizontalSpeed = 1;

	private Animator anim;

	private int points;

	private bool pencilSpawnThisLevel = true, 
					shurikenSpawnThisLevel = true;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator> ();

		spawnManager.spawnMultiplePlatform();
	}


	void Update(){
		scoreText.text = Mathf.Round(transform.position.y * 5).ToString();
		pointsText.text = points.ToString ();

		if (Input.GetButton ("Jump")&& !isJumping) {
			
			isJumping = true;
		}
	}

	public void onJumpButtonDown(){
		isJumping = true;
	}
		
	public void onJumpButtonUp(){
		//jumpKeyRealised = true;
	}
		
	public void goLeft(){
		horizontalSpeed -=0.05f;
	}

	public void goRight(){
		horizontalSpeed += 0.05f;
	}
	
	public void stopHorizontalSpeed(){
		horizontalSpeed = 0;
	}


	void FixedUpdate()
	{
	
		currentSideSpeed = (rb.velocity.y == 0) ? sideSpeed : slowSideSpeed;
			
		horizontalSpeed = Input.GetAxis ("Horizontal");

		if (horizontalSpeed != 0)
			if (horizontalSpeed > 0 && horizontalSpeed < maxHorizontalSpeed)
				horizontalSpeed += 0.05f;
			else if (horizontalSpeed < 0 && horizontalSpeed > -maxHorizontalSpeed)
				horizontalSpeed -= 0.05f;
		
		if (!isJumping)
			anim.SetFloat ("isMoving", Mathf.Abs (horizontalSpeed));
		
		//h = Input.acceleration.x * 2;
	
		transform.Translate (new Vector2(Time.deltaTime * currentSideSpeed * horizontalSpeed, 0));

		if (isJumping && !jumped ){
			
			anim.SetBool ("isJumping", true);
			rb.AddForce (new Vector2 (0, jumpSpeed * Time.deltaTime), ForceMode2D.Impulse);
			pencilSpawnThisLevel = false;
			shurikenSpawnThisLevel = false;

			jumped = true;
		}
		

		if (horizontalSpeed < 0)
				transform.localScale = new Vector2 (1, 1);
		else if (horizontalSpeed > 0)
				transform.localScale = new Vector2 (-1, 1);

		if (transform.position.y < cameraController.getMinPosition())
			gm.gameEnd ();
	}

	void OnCollisionEnter2D(Collision2D collision){

		GameObject c = collision.gameObject;

		if (c.CompareTag ("Pencil")) {

			isJumping = false;
			anim.SetBool ("isJumping", false);

			if (GetComponent<BoxCollider2D>().bounds.min.y <= c.transform.position.y)
				gm.gameEnd ();
		}
			
		if ((c.CompareTag("Tile") || c.CompareTag("CloudTile")) 
			&& transform.position.y > c.transform.position.y 
			&& rb.velocity.y <= 0) {
			
			anim.SetBool ("isJumping", false);

			isJumping = false;
			jumped = false;

			spawnRandomHorizontalPencil ();
			spawnRandomVerticalShuriken();
			spawnManager.spawnALevelOfPlatforms ();
			c.GetComponent<TileBehavior> ().behave();
		}
	}

	public float pencilN = 0.95f;

	void spawnRandomHorizontalPencil(){


		if (Random.value > pencilN && !pencilSpawnThisLevel) {

			pencilSpawnThisLevel = true;
			Vector2 v = new Vector2 (cameraController.getXBounds(), transform.position.y + 0.5f);
			Instantiate (pencilPrefab[0], v, Quaternion.identity);
		}
	}

	public float shurikenN = 0.95f;

	void spawnRandomVerticalShuriken(){


		if (Random.value > shurikenN && !shurikenSpawnThisLevel) {

			shurikenSpawnThisLevel = true;
			Vector2 v = new Vector2 (transform.position.x, cameraController.getMaxPosition());
			Instantiate (pencilPrefab[1], v, Quaternion.identity);
		}
	}
}