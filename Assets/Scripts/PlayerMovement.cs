using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

	public Rigidbody2D rb;
	public CameraController camera;
	public GameManager gm;

	public GameObject pencilPrefab;

	public float sideSpeed, 
					slowSideSpeed, 
					jumpSpeed,
					slowJumpSpeed;

	public SpawnTile spawnManager;
	public Text scoreText;

	private float currentSideSpeed;
	private Animator anim;
	private BoxCollider2D bc;

	public int jumpNumber = 0;
	private int maxJumpNumber = 2;

	public bool jumpKeyRealised = false ;
	public bool isJumping = false;


	bool pencilSpawnThisLevel = true;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator> ();
		bc = GetComponent<BoxCollider2D> ();

		spawnManager.spawnMultiplePlatform();
	}

	void Update(){
		scoreText.text = Mathf.Round(transform.position.y).ToString();


		/*if (Input.GetButtonDown ("Jump") && rb.velocity.y == 0) {

			anim.SetFloat ("isJumping", 0.1f);
			jumpKeyRealised = false;
			isJumping = true;
		}*/


		if (Input.GetButton ("Jump")) {

			anim.SetFloat ("isJumping", 0.6f);
			isJumping = true;
		}

		if (Input.GetButtonUp ("Jump"))
			jumpKeyRealised = true;
		
	}

	void FixedUpdate()
	{
	
		currentSideSpeed = (rb.velocity.y == 0) ? sideSpeed : slowSideSpeed;
		if(rb.velocity.y < 0)
			anim.SetFloat ("isJumping", 0.0f);
			
		float h = Input.GetAxis ("Horizontal");
		anim.SetFloat ("isMoving",Mathf.Abs(h));

		//h = Input.acceleration.x * 2;
	
		transform.Translate (new Vector2(Time.deltaTime * currentSideSpeed * h, 0));



		if (isJumping && jumpKeyRealised){/*|| Input.GetTouch(0).tapCount > 0) {*/
			if (rb.velocity.y == 0) {
			
				rb.AddForce (new Vector2 (0, jumpSpeed * Time.deltaTime), ForceMode2D.Impulse);
				pencilSpawnThisLevel = false;

				jumpNumber++;
				isJumping = false;

			} else if (jumpNumber < maxJumpNumber && jumpKeyRealised) {

				//rb.AddForce (new Vector2 (0, slowJumpSpeed * Time.deltaTime), ForceMode2D.Impulse);
				jumpNumber++;
				jumpKeyRealised = false;
				isJumping = false;
			}
		}
		

		if (h < 0)
			transform.localScale = new Vector2 (1, 1);
		else if (h > 0)
			transform.localScale = new Vector2 (-1, 1);

		if (transform.position.y < camera.getMinPosition())
			gm.gameEnd ();
	}


	void OnCollisionEnter2D(Collision2D collision){
	

		if (collision.gameObject.CompareTag ("Pencil")) {

			jumpNumber = 0;
			jumpKeyRealised = false;

			if (this.gameObject.GetComponent<BoxCollider2D>().bounds.min.y <= collision.gameObject.transform.position.y)
				gm.gameEnd ();

		}

		if (rb.velocity.y == 0 && 
			transform.position.y > collision.gameObject.GetComponent<Transform>().position.y && 
			(collision.gameObject.CompareTag("Tile") || collision.gameObject.CompareTag("CloudTile"))) {
			
			jumpNumber = 0;
			jumpKeyRealised = false;

			if (Random.value > 0.1 && !pencilSpawnThisLevel) {
				
				pencilSpawnThisLevel = true;
				spawnPencil ();
			}

			spawnManager.spawnALevelOfPlatforms ();

			collision.gameObject.GetComponent<TileBehavior> ().behave();
		}
	}

	void spawnPencil(){

		Vector2 v = new Vector2 (camera.getYBounds(), transform.position.y + 0.5f);

		Instantiate (pencilPrefab, v, Quaternion.identity);
	}
}
