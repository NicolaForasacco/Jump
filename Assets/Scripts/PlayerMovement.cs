using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour {

	private Rigidbody2D rb;
	private CameraController cameraController;
	private CameraShake cameraShake;
	private GameManager gm;

	public GameObject[] pencilPrefab;

	public float sideSpeed, 
					slowSideSpeed, 
					jumpSpeed,
					slowJumpSpeed;

	public bool isJumping = false, jumped = false;

	private SpawnTile spawnManager;

	public Text scoreText, 
				pointsText;

	private float currentSideSpeed, 
					horizontalSpeed, 
					maxHorizontalSpeed = 1;

	private Animator anim;

	private int points;

	private bool pencilSpawnThisLevel = true, 
					shurikenSpawnThisLevel = true;

	float lastHeight;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator> ();

		cameraShake = GameObject.FindObjectOfType<CameraShake>();
		cameraController = GameObject.FindObjectOfType<CameraController> ();
		gm = GameObject.FindObjectOfType<GameManager> ();
		spawnManager = GameObject.FindObjectOfType<SpawnTile> ();

		lastHeight = transform.position.y;

		spawnManager.spawnMultiplePlatform();
	}

	public void increasePoint(){
		points++;
	}

	//inside class
	Vector2 firstPressPos;
	Vector2 secondPressPos;
	Vector2 currentSwipe;

	public void Swipe()
	{
		if(Input.touches.Length > 0)
		{
			Touch t = Input.GetTouch(0);
			if(t.phase == TouchPhase.Began)
			{
				//save began touch 2d point
				firstPressPos = new Vector2(t.position.x,t.position.y);
			}
			if(t.phase == TouchPhase.Ended)
			{
				//save ended touch 2d point
				secondPressPos = new Vector2(t.position.x,t.position.y);

				//create vector from the two points
				currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

				//normalize the 2d vector
				currentSwipe.Normalize();
				/*
				//swipe upwards
				if(currentSwipe.y > 0  && currentSwipe.x > -0.5f  && currentSwipe.x < 0.5f)
				{
					Debug.Log("up swipe");
				}else 
				//swipe down
				if(currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
				{
					Debug.Log("down swipe");
				}*/
				
				//swipe left
				if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) {
					left = true;
				} else if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) {
					right = true;
				} else {
					if (!isJumping) {
						isJumping= true;
					}
				}
			}
		}
	}

	void Update(){

		useButtons = gm.useButtons;
		if (transform.position.y * 5> lastHeight * 5 + 400) {

			lastHeight = transform.position.y;

		}

		scoreText.text = Mathf.Round(transform.position.y).ToString();
		pointsText.text = points.ToString ();

		if (rb.velocity.y == 0 && jumped) {
			isJumping = false;
			jumped = false;
		}


		if (useButtons) {
			if (Input.GetButton ("Jump") && !isJumping) {

				isJumping = true;
			}
		} else {

			if (Input.GetKey (KeyCode.A))
				left = true;

			if (Input.GetKey (KeyCode.D))
				right = true;

			Swipe ();
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

	bool left = false;

	bool right = false;

	bool useButtons = false;


	void FixedUpdate()
	{
		if (useButtons) {

			currentSideSpeed = (rb.velocity.y == 0) ? sideSpeed : slowSideSpeed;


			#if UNITY_EDITOR_WIN
			 horizontalSpeed = Input.GetAxis ("Horizontal");
			#endif

			if (!isJumping)
				anim.SetFloat ("isMoving", Mathf.Abs (horizontalSpeed));

			if (horizontalSpeed != 0) {

				if (horizontalSpeed > 0 && horizontalSpeed < maxHorizontalSpeed)
					horizontalSpeed += 0.05f;
				else if (horizontalSpeed < 0 && horizontalSpeed > -maxHorizontalSpeed)
					horizontalSpeed -= 0.05f;

				//h = Input.acceleration.x * 2;

				transform.Translate (new Vector2 (Time.deltaTime * currentSideSpeed * horizontalSpeed, 0));

				if (horizontalSpeed <= 0)
					transform.localScale = new Vector2 (1, 1);
				else if (horizontalSpeed > 0)
					transform.localScale = new Vector2 (-1, 1);
			}
		} else {
			int speed = 100;

			if (right) {
				transform.Translate (new Vector2 (Time.deltaTime * 100,0));

				transform.localScale = new Vector2 (-1, 1);

				if (!isJumping)
					anim.SetFloat ("isMoving", Mathf.Abs (1));

				if(Mathf.FloorToInt(transform.position.x % 20) == 0)
					right = false;

			} else if (left) {
				transform.Translate (new Vector2 (- Time.deltaTime * 100, 0));

				transform.localScale = new Vector2 (1, 1);

				if (!isJumping)
					anim.SetFloat ("isMoving", Mathf.Abs (1));

				if(Mathf.FloorToInt(transform.position.x % 20) == 0)
					left = false;
			}
		}


		if (isJumping && !jumped ){
			
			anim.SetTrigger ("Jumping");

			rb.AddForce (new Vector2 (0, jumpSpeed * Time.deltaTime), ForceMode2D.Impulse);

			pencilSpawnThisLevel = false;
			shurikenSpawnThisLevel = false;

			jumped = true;
		}

		if (transform.position.y < cameraController.getMinPosition())
			gm.gameEnd ();
	}

	void OnCollisionExit2D(Collision2D collision){
		
		if (collision.gameObject.CompareTag ("MovingTile"))
			transform.SetParent (null);
	}

	void OnCollisionEnter2D(Collision2D collision){

		GameObject c = collision.gameObject;

		if (c.CompareTag ("Pencil")) {
			
			if(jumped)
				cameraShake.startShake ();
			
			isJumping = false;
			jumped = false;
			if (GetComponent<BoxCollider2D>().bounds.min.y <= c.transform.position.y)
				gm.gameEnd ();
		}

		if (c.CompareTag ("ColorizeTile")
			&& transform.position.y > c.transform.position.y ) {

			if (!c.GetComponent<TileBehavior> ().isVisited) {
				
				cameraController.changeRandomBackgroundColor ();
				cameraShake.startBigShake ();
				c.GetComponent<TileBehavior> ().behave ();
			}
		}

		if (c.CompareTag ("MovingTile")  
			&& transform.position.y >= c.transform.position.y 
			&& rb.velocity.y == 0) {

			this.transform.SetParent (c.transform);
		}

		if (c.CompareTag ("BounceTile") 
			&& transform.position.y > c.transform.position.y) {

			if(jumped)
				cameraShake.startShake ();
			
			isJumping = false;
			jumped = false;

			spawnRandomHorizontalPencil ();
			spawnManager.spawnMultiplePlatform ();

			c.GetComponent<TileBehavior> ().behave();
		}
			
		if ((c.CompareTag("Tile") 
			|| c.CompareTag("CloudTile") 
			|| c.CompareTag("UselessTile") 
			|| c.CompareTag("MovingTile")) 
			&& transform.position.y > c.transform.position.y ) {

			if(jumped)
				cameraShake.startShake ();
			
			isJumping = false;
			jumped = false;

			spawnRandomHorizontalPencil ();
			spawnRandomVerticalShuriken();
			spawnManager.spawnMultiplePlatform ();
			c.GetComponent<TileBehavior> ().behave();
		}
	}
		
	public float pencilN = 0.95f;

	void spawnRandomHorizontalPencil(){


		if (Random.value > 1 - pencilN 
			&& !pencilSpawnThisLevel) {

			pencilSpawnThisLevel = true;
			Vector2 v = new Vector2 (cameraController.getXBounds(), transform.position.y + 0.5f);
			Instantiate (pencilPrefab[0], v, Quaternion.identity);
		}
	}

	public float shurikenN = 0.95f;

	void spawnRandomVerticalShuriken(){


		if (Random.value > 1 - shurikenN 
			&& !shurikenSpawnThisLevel) {

			shurikenSpawnThisLevel = true;
			Vector2 v = new Vector2 (cameraController.getXStart(), transform.position.y);
			Instantiate (pencilPrefab[1], v, Quaternion.identity);
		}
	}
}