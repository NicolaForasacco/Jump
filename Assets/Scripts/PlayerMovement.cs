using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour {

	public	GameObject[]	enemiesPrefab;
	public	GameObject	deadBodyPrefab,
						comic;
	public	int	cost;

	public	float	sideSpeed,
					slowSideSpeed,
					jumpSpeed,
					enemiesP	= 0.05f,
					shurikenP	= 0.3f,
					pencilP	= 0.3f,
					liftP	= 0.3f;

	public	bool 	isJumping = false,
					jumped = false,
					active = false,
					dead = false,
					isOnLift = false;

	public	int	donutsPicked,
				currentRecord = 0,
				side = -1;


	private Rigidbody2D	rb;
	private GameManager	gm;
	private Animator	anim;
	private CameraShake	cameraShake;
	private	CameraController	cameraController;

	private	Text	scoreText,	
					pointsText;

	private	float	currentSideSpeed,
					horizontalSpeed,
					maxHorizontalSpeed = 1,
					lastHeight;

	private	bool	pencilSpawnThisLevel = true,
					shurikenSpawnThisLevel = true,
					isOnMovingTile = false,
					isFirstMovement = true,
					jumpButtonDown = false;

	AudioManager audioManager;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		audioManager = GameObject.FindObjectOfType<AudioManager> ();

		pointsText = GameObject.FindGameObjectWithTag ("DonutPointsText").GetComponent<Text> ();

		cameraShake	=	GameObject.FindObjectOfType<CameraShake>();
		cameraController	= GameObject.FindObjectOfType<CameraController> ();
		gm	=	GameObject.FindObjectOfType<GameManager> ();


		if (comic != null) {
			
			comic.GetComponent<SpriteRenderer> ().sprite = GameObject.FindObjectOfType<MessageManager> ().getStartMessage ();
			comic.GetComponent<Animator> ().SetTrigger ("FadeIn");
		}


		donutsPicked = PlayerPrefs.GetInt ("Donuts", 0);
		pointsText.text = donutsPicked.ToString ();

		lastHeight = transform.position.y;
		horizontalSpeed = 0;
	}



	Vector2 firstPressPos;
	Vector2 secondPressPos;
	Vector2 currentSwipe;

	/*public void Swipe()
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

				//swipe upwards
				if(currentSwipe.y > 0  && currentSwipe.x > -0.5f  && currentSwipe.x < 0.5f)
				{
					Debug.Log("up swipe");
				}else 
				//swipe down
				if(currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
				{
					Debug.Log("down swipe");
				}
				
				//swipe left
				if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) {
					//left = true;
				} else if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) {
					//right = true;
				} else {
					if (!isJumping) {
						isJumping= true;
					}
				}
			}
		}
	}*/

	public void activate(){
		
		scoreText = GameObject.FindGameObjectWithTag ("HeightText").GetComponent<Text> ();

		// non metter questo in start(provare a capire il problema)
		//groundDust = GameObject.FindGameObjectWithTag ("GroundDust");
		//anim = groundDust.GetComponent<Animator>();

		active = true;
	}

	void Update(){
		if (active) {
			
			useButtons = gm.useButtons;
			if (transform.position.y * 5 > lastHeight * 5 + 400) {

				lastHeight = transform.position.y;

			}

			if (!dead) {
			
				if (transform.position.y / 20 > currentRecord) {
					currentRecord = Mathf.FloorToInt (transform.position.y / 20);
					scoreText.text = currentRecord.ToString ();

				}

				pointsText.text = donutsPicked.ToString ();
			}

			if (rb.velocity.y == 0 && jumped) {
				isJumping = false;
				jumped = false;
			}


			if (useButtons) {
			
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

			} else {
				//Swipe ();
			}
		}
	}


	public void showMessage(){
		
		if (comic != null) {

			comic.GetComponent<SpriteRenderer> ().sprite = GameObject.FindObjectOfType<MessageManager> ().getStartMessage ();
			comic.GetComponent<Animator> ().SetTrigger ("FadeIn");
		}
	}

	public void hideMessage(){
		
		if (comic != null) {
			
			comic.GetComponent<Animator> ().SetTrigger ("FadeOut");
		}
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


	bool useButtons = true;
	float speed = 0.2f; 
	float decreaseFactor = 0.1f;

	void FixedUpdate()
	{
		if (active) {
			currentSideSpeed = (rb.velocity.y == 0) ? sideSpeed : slowSideSpeed;
	

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

			if (isJumping && !jumped && (rb.velocity.y == 0 || isOnMovingTile)) {

				jumpAnim ();

				rb.AddForce (new Vector2 (0, jumpSpeed), ForceMode2D.Impulse);

				pencilSpawnThisLevel = false;
				shurikenSpawnThisLevel = false;

				jumped = true;
			}

			if (transform.position.y < cameraController.getMinPosition ())
				die ();

		}
	}

	void OnCollisionExit2D(Collision2D collision){
		
		if (collision.gameObject.CompareTag ("MovingTile") 
            || collision.gameObject.CompareTag ("Pencil")) {

			isOnMovingTile = false;

			if(!isOnLift)
				transform.SetParent (null);
		}

		if (collision.gameObject.CompareTag ("Lift")) {

			isOnLift = false;
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

            
			gm.gameEnd();
        }
	}


	void OnCollisionEnter2D(Collision2D collision){

		GameObject c = collision.gameObject;

		if (c.CompareTag ("Shuriken"))
			die ();
		
		if (c.CompareTag ("Pencil")) {

			if (jumped) {
				cameraShake.startShake ();
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

                if (c.CompareTag("Lift")){
                    c.GetComponent<VerticalLift>().incrementSpeed();
                    isOnLift = true;
                }

                isJumping = false;
                jumped = false;
				idleAnim ();
            }
		}

		if ((c.CompareTag("Tile") 
			|| c.CompareTag("CloudTile") 
			|| c.CompareTag("UselessTile") 
			|| c.CompareTag("MovingTile")
			|| c.CompareTag("RotatingTile")
			|| c.CompareTag("LiftTile"))
			&& GetComponent<BoxCollider2D> ().bounds.min.y > c.transform.position.y ) {

			if (jumped) {
				cameraShake.startShake ();
				landAnim ();
			}

			isJumping = false;
			jumped = false;
			idleAnim ();

            spawnEnemies ();

			try{
				c.GetComponent<TileBehavior> ().behave();
			}catch{}

		}

		if (c.CompareTag ("SpineTile") 
			&& GetComponent<BoxCollider2D> ().bounds.min.y >= c.GetComponent<BoxCollider2D>().bounds.max.y)
			die ();

		if (c.CompareTag ("BounceTile") 
			&& transform.position.y > c.transform.position.y) {

			if(jumped)
				cameraShake.startShake ();

			c.GetComponent<TileBehavior> ().behave();
		}
	}

		
	void spawnEnemies(){

		if (Random.value <= enemiesP) {
			float r = Random.value;

			if (r <= shurikenP && !shurikenSpawnThisLevel)
				spawnRandomVerticalShuriken ();
			else if (r <= shurikenP + pencilP && !pencilSpawnThisLevel)
				spawnRandomHorizontalPencil ();
		}
	}

	void spawnRandomHorizontalPencil(){

		pencilSpawnThisLevel = true;
		Vector2 v = new Vector2 (cameraController.getXBounds(), transform.position.y + 80);
		Instantiate (enemiesPrefab[1], v, Quaternion.identity);	
	}

	void spawnRandomVerticalShuriken(){

		shurikenSpawnThisLevel = true;
		Vector2 v = new Vector2 (cameraController.getXStart(), transform.position.y + 40);
		Instantiate (enemiesPrefab[0], v, Quaternion.identity);
	}


	public void increaseDonutsPicked(){
		donutsPicked++;
	}

	public void increasePizzaPicked(){

	}

	public void increaseHamburgerPicked(){
	}
}