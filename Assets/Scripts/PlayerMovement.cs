using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour {

	public Character ch;
	public GameObject deadBodyPrefab, comic;

	public bool	dead,
				jumped,
				active,
				isOnLift,
				isJumping;

	public int donutsPicked,
				currentRecord = 0,
				side = -1,
				cost;


	private Animator animator;
	private Rigidbody2D	rigidBody;
	private GameManager	gameManager;
	private CameraShake	cameraShake;
	private AudioManager audioManager;
	private	CameraController cameraController;
	private SpawnTileManager spawnTileManager;

	private	Text	scoreText,	
					pointsText;

	private	float	sideSpeed,
					jumpSpeed,
					lastHeight,
					slowSideSpeed,
					horizontalSpeed,
					currentSideSpeed,
					maxHorizontalSpeed = 1;

	private	bool	isOnMovingTile,
					isFirstMovement = true,
					jumpButtonDown;


	void Start()
	{
		rigidBody = GetComponent<Rigidbody2D>();
		cameraShake	= GameObject.FindObjectOfType<CameraShake>();
		gameManager = GameObject.FindObjectOfType<GameManager> ();
		audioManager = GameObject.FindObjectOfType<AudioManager> ();
		spawnTileManager = GameObject.FindObjectOfType<SpawnTileManager> ();
		cameraController = GameObject.FindObjectOfType<CameraController> ();
		pointsText = GameObject.FindGameObjectWithTag ("DonutPointsText").GetComponent<Text> ();

		initializeCharacterParameters ();

		donutsPicked = PlayerPrefs.GetInt ("Donuts", 0);
		pointsText.text = donutsPicked.ToString ();

		lastHeight = transform.position.y;
		horizontalSpeed = 0;
	}

	private void initializeCharacterParameters(){

		rigidBody.gravityScale = ch.gravityScale;
		rigidBody.mass = ch.mass;
		GetComponent<SpriteRenderer> ().sprite = ch.sprite;
		jumpSpeed = ch.jumpSpeed;
		sideSpeed = ch.sideSpeed;
		slowSideSpeed = ch.slowSideSpeed;
	}
		
	public void activate(){
		
		scoreText = GameObject.FindGameObjectWithTag ("HeightText").GetComponent<Text> ();

		// non metter questo in start(provare a capire il problema)
		//groundDust = GameObject.FindGameObjectWithTag ("GroundDust");
		//anim = groundDust.GetComponent<Animator>();

		if (comic != null) {

			comic.GetComponent<SpriteRenderer> ().sprite = GameObject.FindObjectOfType<MessageManager> ().getStartMessage ();
			comic.GetComponent<Animator> ().SetTrigger ("FadeIn");
		}

		active = true;
	}

	void Update(){
		
		if (active) {
			
			if (transform.position.y * 5 > lastHeight * 5 + 400)
				lastHeight = transform.position.y;

			if (!dead && transform.position.y / 20 > currentRecord) {

				currentRecord = Mathf.FloorToInt (transform.position.y / 20);
				scoreText.text = currentRecord.ToString ();
			}

			if (rigidBody.velocity.y == 0 && jumped) {
				isJumping = false;
				jumped = false;
			}

			#if UNITY_EDITOR_WIN

			if (Input.GetButton ("Jump")) {
				onJumpButtonDown ();
			} else
				onJumpButtonUp ();

			if (Input.GetAxis("Horizontal") < 0)
				goLeft ();
			else if (Input.GetAxis("Horizontal") > 0)
				goRight ();
			else
				side = -1;
		
			#endif

			if (jumpButtonDown)
				isJumping = true;
		}
	}


	public void showMessage(){
		
		if (comic != null) {

			comic.GetComponent<SpriteRenderer> ().sprite = GameObject.FindObjectOfType<MessageManager> ().getStartMessage ();
			comic.GetComponent<Animator> ().SetTrigger ("FadeIn");
		}
	}

	public void hideMessage(){
		
		if (comic != null)
			comic.GetComponent<Animator> ().SetTrigger ("FadeOut");
	}
		

	public void onJumpButtonDown(){
		
		jumpButtonDown = true;

		if (isFirstMovement) {
			hideMessage ();
			isFirstMovement = false;
		}
	}

	public void onJumpButtonUp(){
		
		jumpButtonDown = false;


	}
		
	public void goLeft(){
		
		if (!isJumping && !jumped && side != 0)
			movingAnim ();
		
		side = 0;

		if (isFirstMovement) {
			hideMessage ();
			isFirstMovement = false;
		}
	}

	public void goRight(){

		if (!isJumping && !jumped && side != 1)
			movingAnim ();
		
		side = 1;

		if (isFirstMovement) {
			hideMessage ();
			isFirstMovement = false;
		}
	}
	
	public void stopHorizontalSpeed(){
		side = -1;
	}
		
	float speed = 0.2f; 
	float decreaseFactor = 0.1f;

	void FixedUpdate()
	{
		if (active) //&& (!isOnLift || (isOnLift && !currentLift.isLiftWorking ))) {
			
			currentSideSpeed = (rigidBody.velocity.y == 0) ? sideSpeed : slowSideSpeed;
	

			if (horizontalSpeed < maxHorizontalSpeed && side == 1)
				horizontalSpeed += speed;
			else if (horizontalSpeed > -maxHorizontalSpeed && side == 0)
				horizontalSpeed -= speed;
			else if (side == -1 && horizontalSpeed != 0) {

				if (horizontalSpeed > 0)
					horizontalSpeed -= decreaseFactor;
				else if (horizontalSpeed < 0)
					horizontalSpeed += decreaseFactor;

				if (horizontalSpeed >= -decreaseFactor && horizontalSpeed <= decreaseFactor)
					horizontalSpeed = 0;
			}			

			transform.Translate (new Vector2 (Time.deltaTime * currentSideSpeed * horizontalSpeed, 0));

			/*if (horizontalSpeed < 0)
				groundDust.transform.localScale = new Vector2 (-groundDust.transform.localScale.x, groundDust.transform.localScale.y);
			else if (horizontalSpeed > 0)
				groundDust.transform.localScale = new Vector2 (groundDust.transform.localScale.x, groundDust.transform.localScale.y);
*/

			/*if (!isJumping && !jumped)
            if (horizontalSpeed == 0)
                idleAnim();
            else
                movingAnim();
*/

			if (isJumping && !jumped && (rigidBody.velocity.y == 0 || isOnMovingTile)) {

				jumpAnim ();

				rigidBody.AddForce (new Vector2 (0, jumpSpeed), ForceMode2D.Impulse);

				spawnTileManager.pencilSpawnThisLevel = false;
				spawnTileManager.shurikenSpawnThisLevel = false;

				jumped = true;
			}

			if (transform.position.y < cameraController.getMinPosition ())
				die ();


	}

	void OnCollisionExit2D(Collision2D collision){
		
		if (collision.gameObject.CompareTag ("MovingTile") 
            || collision.gameObject.CompareTag ("Pencil")) {

			isOnMovingTile = false;
			transform.SetParent (null);
		}
	}

    void idleAnim()
    {
		/*
        anim.SetBool("Idle", true);
        anim.SetBool("Moving", false);
        anim.SetBool("Jumping", false);*/
    }

	void landAnim(){
		//anim.SetTrigger ("landing");
	}

    void movingAnim()
    {

		//anim.SetTrigger ("moving");

        /*anim.SetBool("Idle", false);
        anim.SetBool("Moving", true);
        anim.SetBool("Jumping", false);*/
    }

    void jumpAnim() {
		/*
        anim.SetBool("Idle",false);
        anim.SetBool("Moving", false);
        anim.SetBool("Jumping", true);
        */
    }

	void die(){

        if (!dead)
        {
			//ScreenCapture.CaptureScreenshot ("\\Pictures\\LastDeath.png", 2);
			dead = true;
            Vector2 v = new Vector2(transform.position.x - 3, transform.position.y - 30 + 1);
            Instantiate(deadBodyPrefab, v, Quaternion.identity);
            this.gameObject.SetActive(false);

            
			gameManager.gameEnd();
        }
	}


	void OnCollisionEnter2D(Collision2D collision){

		GameObject c = collision.gameObject;

		if (c.CompareTag ("Shuriken"))
			die ();
		
		if (c.CompareTag ("Pencil")) {

			if (jumped) {
				//cameraShake.startShake ();
				landAnim ();
			}
			
			isJumping = false;
			jumped = false;

            if (GetComponent<BoxCollider2D> ().bounds.min.y >= c.transform.position.y) {

				this.transform.SetParent (c.transform);
				c.GetComponent<PencilMovement> ().showExplosion ();

			} else if (GetComponent<BoxCollider2D> ().bounds.min.y <= c.transform.position.y &&
				transform.position.x <= c.GetComponent<BoxCollider2D> ().bounds.min.x)
				die ();
		}

		if (c.CompareTag ("ColorizeTile")
			&& GetComponent<BoxCollider2D> ().bounds.min.y >= c.GetComponent<BoxCollider2D>().bounds.max.y) {

			c.GetComponent<TileBehavior> ().behave ();
		}

		if ((c.CompareTag ("MovingTile") || c.CompareTag ("Lift"))
			&& GetComponent<BoxCollider2D> ().bounds.min.y > c.transform.position.y) {

			isOnMovingTile = true;

			if (jumped) {
				landAnim ();
			}

            if (!isOnLift){

                this.transform.SetParent(c.transform);

                isJumping = false;
                jumped = false;
				idleAnim ();
            }
		}

		if ((c.CompareTag("Tile") 
			|| c.CompareTag("CloudTile") 
			|| c.CompareTag("UselessTile") 
			|| c.CompareTag("MovingTile")
			|| c.CompareTag("RotatingTile"))
			&& GetComponent<BoxCollider2D> ().bounds.min.y > c.transform.position.y ) {

			if (jumped) {
				//cameraShake.startShake ();
				landAnim ();
			}

			isJumping = false;
			jumped = false;
			idleAnim ();

			spawnTileManager.spawnEnemies ();

			try{
				c.GetComponent<TileBehavior> ().behave();
			}catch{}

		}

		if (c.CompareTag ("SpineTile") 
			&& GetComponent<BoxCollider2D> ().bounds.min.y >= c.GetComponent<BoxCollider2D>().bounds.max.y)
			die ();

		if (c.CompareTag ("BounceTile") 
			&& transform.position.y > c.transform.position.y) {

			//if(jumped)
				//cameraShake.startShake ();

			c.GetComponent<TileBehavior> ().behave();
		}
	}


	public void increaseDonutsPicked(){
		
		donutsPicked++;
		pointsText.text = donutsPicked.ToString ();
	}

	public void disableGravity(){

		rigidBody = GetComponent<Rigidbody2D>();
		rigidBody.bodyType = RigidbodyType2D.Static;
	}
}