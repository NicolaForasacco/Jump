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
	public GameObject deadBodyPrefab;

	public float sideSpeed, 
					slowSideSpeed, 
					jumpSpeed;

	public bool isJumping = false, jumped = false;

	public Text scoreText, 
				pointsText;

	private float currentSideSpeed, 
					horizontalSpeed, 
					maxHorizontalSpeed = 1;

	private Animator anim;

	private int points;

	private bool pencilSpawnThisLevel = true, 
					shurikenSpawnThisLevel = true,	
					liftSpawnThisLevel = true;

	float lastHeight;

	public int side = -1;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator> ();
        

		cameraShake = GameObject.FindObjectOfType<CameraShake>();
		cameraController = GameObject.FindObjectOfType<CameraController> ();
		gm = GameObject.FindObjectOfType<GameManager> ();

		lastHeight = transform.position.y;
		horizontalSpeed = 0;
	}

	public void increasePoint(){
		points++;
	}

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
	}

	public bool dead = false;

	float currentRecord = 0;

	void Update(){

		useButtons = gm.useButtons;
		if (transform.position.y * 5> lastHeight * 5 + 400) {

			lastHeight = transform.position.y;

		}

		if (!dead) {
			
			if(transform.position.y / 20 > currentRecord){
				currentRecord = transform.position.y / 20;
				scoreText.text = Mathf.Round (currentRecord).ToString ();

			}

			pointsText.text = points.ToString ();
		}

		if (rb.velocity.y == 0 && jumped) {
			isJumping = false;
			jumped = false;
		}


		if (useButtons) {
			
			#if UNITY_EDITOR_WIN

			if (Input.GetButton ("Jump")) {
                onJumpButtonDown();
			}

			if (Input.GetKey(KeyCode.A))
				side = 0;
			else if (Input.GetKey(KeyCode.D))
				side = 1;
			else
				side = -1;
			
			#endif

		} else {
			Swipe ();
		}
	}

	public void onJumpButtonDown(){
		isJumping = true;
	}
		
	public void onJumpButtonUp(){
	}
		
	public void goLeft(){
		side = 0;
	}

	public void goRight(){
		side = 1;
	}
	
	public void stopHorizontalSpeed(){
		side = -1;
	}


	bool useButtons = true;
	float speed = 0.2f; 
	float decreaseFactor = 0.1f;

	void FixedUpdate()
	{
		currentSideSpeed = (rb.velocity.y == 0) ? sideSpeed : slowSideSpeed;
	

		if (horizontalSpeed < maxHorizontalSpeed && side == 1)
			horizontalSpeed += speed;
		
		else if (horizontalSpeed > - maxHorizontalSpeed && side == 0)
			horizontalSpeed -= speed;
		
		else if (side == -1 && horizontalSpeed != 0) {

			if(horizontalSpeed > 0)
				horizontalSpeed -= decreaseFactor;
			else if(horizontalSpeed < 0)
				horizontalSpeed += decreaseFactor;

			if (horizontalSpeed >= -decreaseFactor && horizontalSpeed <= decreaseFactor)
				horizontalSpeed = 0;
		}			

		transform.Translate (new Vector2 (Time.deltaTime * currentSideSpeed * horizontalSpeed, 0));

		if (horizontalSpeed < 0)
			transform.localScale = new Vector2 (1, 1);
		else if(horizontalSpeed > 0)
			transform.localScale = new Vector2 (-1, 1);

        if (!isJumping && !jumped)
            if (horizontalSpeed == 0)
                idleAnim();
            else
                movingAnim();

		if (isJumping && !jumped && rb.velocity.y == 0){

            jumpAnim();

			rb.AddForce (new Vector2 (0, jumpSpeed), ForceMode2D.Impulse);

			pencilSpawnThisLevel = false;
			shurikenSpawnThisLevel = false;
			liftSpawnThisLevel = false;

			jumped = true;
		}

		if (transform.position.y < cameraController.getMinPosition ())
			die ();
	}

	void OnCollisionExit2D(Collision2D collision){
		
		if (collision.gameObject.CompareTag ("MovingTile") 
            || collision.gameObject.CompareTag ("Pencil")) {

			if(!isOnLift)
				transform.SetParent (null);
		}

		if (collision.gameObject.CompareTag ("LiftTile")) {

			isOnLift = false;
			transform.SetParent (null);
		}
	}

    void idleAnim()
    {

        anim.SetBool("Idle", true);
        anim.SetBool("Moving", false);
        anim.SetBool("Jumping", false);
    }

    void movingAnim()
    {

        anim.SetBool("Idle", false);
        anim.SetBool("Moving", true);
        anim.SetBool("Jumping", false);
    }

    void jumpAnim() {

        anim.SetBool("Idle",false);
        anim.SetBool("Moving", false);
        anim.SetBool("Jumping", true);
    }

	void die(){

        if (!dead)
        {
            Vector2 v = new Vector2(transform.position.x - 3, transform.position.y - 30 + 1);
            Instantiate(deadBodyPrefab, v, Quaternion.identity);
            this.gameObject.SetActive(false);

            dead = true;
            gm.gameEnd();
        }
	}

	bool isOnLift = false;

	void OnCollisionEnter2D(Collision2D collision){

		GameObject c = collision.gameObject;

		if (c.CompareTag ("Shuriken"))
			die ();
		
		if (c.CompareTag ("Pencil")) {

			if(jumped)
				cameraShake.startShake ();
			
			isJumping = false;
			jumped = false;

            if (GetComponent<BoxCollider2D> ().bounds.min.y >= c.transform.position.y) {

				this.transform.SetParent (c.transform);
				c.GetComponent<PencilMovement> ().enableGravity ();

			} else if (GetComponent<BoxCollider2D> ().bounds.min.y <= c.transform.position.y)
				die ();
		}

		if (c.CompareTag ("ColorizeTile")
			&& GetComponent<BoxCollider2D> ().bounds.min.y > c.transform.position.y) {

			if (!c.GetComponent<TileBehavior> ().isVisited) {
				
				cameraController.changeRandomBackgroundColor ();
				cameraShake.startBigShake ();
				c.GetComponent<TileBehavior> ().behave ();
			}
		}

		if ((c.CompareTag ("MovingTile") || c.CompareTag ("LiftTile"))
			&& GetComponent<BoxCollider2D> ().bounds.min.y > c.transform.position.y) {

            if (!isOnLift){

                this.transform.SetParent(c.transform);

                if (c.CompareTag("LiftTile")){
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
			|| c.CompareTag("MovingTile")) 
			&& GetComponent<BoxCollider2D> ().bounds.min.y > c.transform.position.y ) {

			if(jumped)
				cameraShake.startShake ();

			isJumping = false;
			jumped = false;
			idleAnim ();

            spawnEnemies ();

			c.GetComponent<TileBehavior> ().behave();
		}

		if (c.CompareTag ("SpineTile") 
			&& GetComponent<BoxCollider2D> ().bounds.min.y >= c.transform.position.y )
			die ();

		if (c.CompareTag ("BounceTile") 
			&& transform.position.y > c.transform.position.y) {

			if(jumped)
				cameraShake.startShake ();

			c.GetComponent<TileBehavior> ().behave();
		}
	}
	public float enemiesN = 0.05f;

	public float pencilN = 0.05f;

	void spawnEnemies(){

		if (Random.value > 1 - enemiesN) {
			int r = Random.Range (1, pencilPrefab.Length + 1);

			if (r == 1)
				spawnRandomVerticalShuriken ();
			if (r == 2)
				spawnRandomHorizontalPencil ();
			if (r == 3)
				spawnRandomVerticalLift ();
		}
	}

	void spawnRandomHorizontalPencil(){


		if (Random.value > 1 - pencilN 
			&& !pencilSpawnThisLevel) {

			pencilSpawnThisLevel = true;
			Vector2 v = new Vector2 (cameraController.getXBounds(), transform.position.y);
			Instantiate (pencilPrefab[0], v, Quaternion.identity);
		}
	}

	public float shurikenN = 0.05f;

	void spawnRandomVerticalShuriken(){


		if (Random.value > 1 - shurikenN 
			&& !shurikenSpawnThisLevel) {

			shurikenSpawnThisLevel = true;
			Vector2 v = new Vector2 (cameraController.getXStart(), transform.position.y + 40);
			Instantiate (pencilPrefab[1], v, Quaternion.identity);
		}
	}

	public float liftN = 0.05f;

	void spawnRandomVerticalLift(){
		
		if (Random.value > 1 - liftN 
			&& !shurikenSpawnThisLevel) {

			liftSpawnThisLevel = true;
			Vector2 v = new Vector2 (Random.Range(cameraController.getXStart(), cameraController.getXBounds()), cameraController.getMinPosition());
			Instantiate (pencilPrefab[2], v, Quaternion.identity);
		}
	}
}