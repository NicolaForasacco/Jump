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

	public SpawnTile spawnManager;
	public Text scoreText;

	private float currentSideSpeed;
	private Animator anim;

	public int jumpNumber = 0;
	private int maxJumpNumber = 2;

	public bool jumpKeyRealised = false, isJumping = false;

	bool pencilSpawnThisLevel = true;
	bool shurikenSpawnThisLevel = true;

	int points;

	public Text pointsText;

	float horizontalSpeed, maxHorizontalSpeed = 1;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator> ();

		spawnManager.spawnMultiplePlatform();
	}


	void Update(){
		scoreText.text = Mathf.Round(transform.position.y*5).ToString();
		pointsText.text = points.ToString ();

		if (Input.GetButton ("Jump")) {
			isJumping = true;
		}

		if (Input.GetButtonUp ("Jump")) {
			anim.SetFloat ("isJumping", 0.6f);
			jumpKeyRealised = true;
		}
	}

	public void onJumpButtonDown(){

		anim.SetFloat ("isJumping", 0.6f);
		isJumping = true;
	}
		
	public void onJumpButtonUp(){
		jumpKeyRealised = true;
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
		
	
		anim.SetFloat ("isMoving", Mathf.Abs (horizontalSpeed));
		//h = Input.acceleration.x * 2;
	
		transform.Translate (new Vector2(Time.deltaTime * currentSideSpeed * horizontalSpeed, 0));

		if (isJumping && jumpKeyRealised){/*|| Input.GetTouch(0).tapCount > 0) {*/
			if (rb.velocity.y == 0) {
			
				rb.AddForce (new Vector2 (0, jumpSpeed * Time.deltaTime), ForceMode2D.Impulse);
				pencilSpawnThisLevel = false;
				shurikenSpawnThisLevel = false;

				jumpNumber++;
				isJumping = false;

			} else if (jumpNumber < maxJumpNumber && jumpKeyRealised) {

				//rb.AddForce (new Vector2 (0, slowJumpSpeed * Time.deltaTime), ForceMode2D.Impulse);
				jumpNumber++;
				jumpKeyRealised = false;
				isJumping = false;
			}
		}
		

		if (horizontalSpeed < 0)
				transform.localScale = new Vector2 (1, 1);
		else if (horizontalSpeed > 0)
				transform.localScale = new Vector2 (-1, 1);

		if (transform.position.y < cameraController.getMinPosition())
			gm.gameEnd ();
	}

	void OnCollisionEnter2D(Collision2D collision){
	
		if (collision.gameObject.CompareTag ("Food")) {
			Destroy (collision.gameObject);

			points++;
		}

		if (collision.gameObject.CompareTag ("Pencil")) {

			jumpNumber = 0;
			jumpKeyRealised = false;

			anim.SetFloat ("isJumping", 0.0f);

			if (this.gameObject.GetComponent<BoxCollider2D>().bounds.min.y <= collision.gameObject.transform.position.y)
				gm.gameEnd ();

		}

		if (rb.velocity.y == 0 && 
			transform.position.y > collision.gameObject.GetComponent<Transform>().position.y &&
			(collision.gameObject.CompareTag("Tile") || collision.gameObject.CompareTag("CloudTile"))) {
			
			jumpNumber = 0;
			jumpKeyRealised = false;

			anim.SetFloat ("isJumping", 0.0f);

			spawnRandomHorizontalPencil ();
			spawnRandomVerticalShuriken();

			spawnManager.spawnALevelOfPlatforms ();

			collision.gameObject.GetComponent<TileBehavior> ().behave();
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
