using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour {

	public Character ch;
	public GameObject deadBodyPrefab, comic;
	public bool	dead, jumped, active, isJumping;
	public int donutsPicked, currentRecord = 0, side = -1;

	private Animator animator;
	private Rigidbody2D	rigidBody;
	private GameManager	gameManager;
	private CameraShake	cameraShake;
	private AudioManager audioManager;
	private BoxCollider2D boxCollider2D;
	private	CameraController cameraController;
	private SpawnTileManager spawnTileManager;
	private PreferenceManager prefManager;

	private	Text scoreText, pointsText;
	private	bool isOnMovingTile, isFirstMovement = true, jumpButtonDown;

	private	float	speed = 0.2f, 
					decreaseFactor = 0.1f,
					sideSpeed,
					jumpSpeed,
					lastHeight,
					slowSideSpeed,
					horizontalSpeed = 0,
					currentSideSpeed,
					maxHorizontalSpeed = 1;

	private int currentDonutsPicked = 0, currentTotalJumps = 0;



	void Start()
	{
		rigidBody = GetComponent<Rigidbody2D>();
		boxCollider2D = GetComponent<BoxCollider2D> ();
		cameraShake	= GameObject.FindObjectOfType<CameraShake>();
		gameManager = GameObject.FindObjectOfType<GameManager> ();
		audioManager = GameObject.FindObjectOfType<AudioManager> ();
		prefManager = GameObject.FindObjectOfType<PreferenceManager> ();
		cameraController = GameObject.FindObjectOfType<CameraController> ();
		spawnTileManager = GameObject.FindObjectOfType<SpawnTileManager> ();

		pointsText = GameObject.FindGameObjectWithTag ("DonutPointsText").GetComponent<Text> ();

		initializeCharacterParameters ();

		donutsPicked = prefManager.getDonut();
		pointsText.text = donutsPicked.ToString ();

		lastHeight = transform.position.y;

		Destroy (boxCollider2D);
		boxCollider2D = gameObject.AddComponent<BoxCollider2D> () as BoxCollider2D;

	}

	private void initializeCharacterParameters(){

		jumpSpeed = ch.jumpSpeed;
		sideSpeed = ch.sideSpeed;
		rigidBody.mass = ch.mass;
		slowSideSpeed = ch.slowSideSpeed;
		rigidBody.gravityScale = ch.gravityScale;
		GetComponent<SpriteRenderer> ().sprite = ch.sprite;
	}
		
	public void activate(){
		
		scoreText = GameObject.FindGameObjectWithTag ("HeightText").GetComponent<Text> ();
		showMessage ();
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
		
	void checkFirstMovement(){
		
		if (isFirstMovement) {
			hideMessage ();
			isFirstMovement = false;
		}
	}

	#region Input methods

	public void onJumpButtonDown(){
		
		jumpButtonDown = true;
		checkFirstMovement ();
	}

	public void onJumpButtonUp(){
		
		jumpButtonDown = false;
	}
		
	public void goLeft(){
		
		side = 0;
		checkFirstMovement ();
	}

	public void goRight(){
		
		side = 1;
		checkFirstMovement ();
	}
	
	public void stopHorizontalSpeed(){
		side = -1;
	}

	#endregion

	void FixedUpdate()
	{
		if (active) {
			
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
				
				rigidBody.AddForce (new Vector2 (0, jumpSpeed), ForceMode2D.Impulse);

				currentTotalJumps++;

				spawnTileManager.pencilSpawnThisLevel = false;
				spawnTileManager.shurikenSpawnThisLevel = false;

				jumped = true;
			}

			if (transform.position.y < cameraController.getMinPosition ())
				die ();
		}
	}

	public bool isPlayerVerticalStill(){

		if ((!jumpButtonDown && rigidBody.velocity.y == 0) || dead)
			return true;
		else
			return false;
	}

	void OnCollisionExit2D(Collision2D collision){
		
		if (collision.gameObject.CompareTag ("MovingTile") 
            || collision.gameObject.CompareTag ("Pencil")) {

			isOnMovingTile = false;
			transform.SetParent (null);
		}
	}
		
	void die(){

        if (!dead)
        {
			//ScreenCapture.CaptureScreenshot ("\\Pictures\\LastDeath.png", 2);
			dead = true;

			prefManager.increasePlayNumber ();
			prefManager.increaseTotalDonutsPicked (currentDonutsPicked);
			prefManager.increaseTotalJumps (currentTotalJumps);
			prefManager.increaseTotalHeight (currentRecord);

			cameraShake.startBigShake ();
			audioManager.playGameOverSound();


            Vector2 v = new Vector2(transform.position.x - 3, transform.position.y - 30 + 1);
            Instantiate(deadBodyPrefab, v, Quaternion.identity);
            this.gameObject.SetActive(false);

            
			gameManager.gameEnd();
        }
	}


	void OnCollisionEnter2D(Collision2D collision){

		GameObject c = collision.gameObject;

		if (c.CompareTag ("Pencil")) {
			
			if (jumped)
				cameraShake.startShake ();
			
			isJumping = false;
			jumped = false;

			if (boxCollider2D.bounds.min.y >= c.transform.position.y) {

				this.transform.SetParent (c.transform);
				c.GetComponent<PencilMovement> ().showExplosion ();
				prefManager.increaseTotalPencilExplosion ();

			} else if (boxCollider2D.bounds.min.y <= c.transform.position.y && boxCollider2D.bounds.min.x <= c.GetComponent<BoxCollider2D> ().bounds.min.x) {
				prefManager.increaseDeathByPencil ();
				die ();
			}
		}

		if ((c.CompareTag ("ColorizeTile") || c.CompareTag ("BounceTile")) && boxCollider2D.bounds.min.y >= c.GetComponent<BoxCollider2D>().bounds.max.y)
			c.GetComponent<TileBehavior> ().behave ();

		if ((c.CompareTag ("MovingTile") || c.CompareTag ("Lift")) && boxCollider2D.bounds.min.y > c.transform.position.y) {

			isOnMovingTile = true;
			this.transform.SetParent(c.transform);
			isJumping = false;
            jumped = false;
		}

		if ((c.CompareTag("Tile") 
			|| c.CompareTag("CloudTile") 
			|| c.CompareTag("UselessTile") 
			|| c.CompareTag("MovingTile"))
			&& boxCollider2D.bounds.min.y > c.transform.position.y ) {

			isJumping = false;
			jumped = false;
			spawnTileManager.spawnEnemies ();
			c.GetComponent<TileBehavior> ().behave();
		}

		if (c.CompareTag ("Shuriken") || (c.CompareTag ("SpineTile") && boxCollider2D.bounds.min.y >= c.GetComponent<BoxCollider2D> ().bounds.max.y)) {
			
			prefManager.increaseDeathByShuriken ();
			die ();
		}
	}


	public void increaseDonutsPicked(){


		audioManager.playDonutsPickupSound ();
		currentDonutsPicked++;
		donutsPicked++;
		pointsText.text = donutsPicked.ToString ();
  	}

	public void SetGravity(bool enableGravity){

		if (enableGravity) {
			
			rigidBody = GetComponent<Rigidbody2D> ();
			rigidBody.bodyType = RigidbodyType2D.Dynamic;
		} else {

			rigidBody = GetComponent<Rigidbody2D>();
			rigidBody.bodyType = RigidbodyType2D.Static;
		}
	}
}