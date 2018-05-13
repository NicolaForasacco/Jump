using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour {

	public Character ch;
	public GameObject deadBodyPrefab, comicPrefab;
	public bool	dead, jumped, active;
	public int donutsPicked, currentRecord = 0, side = -1;
	public bool isOnTile = true;
	public float jumpSpeed, sideSpeed, slowSideSpeed, mass, gravityScale;

	private GameObject comic;
	private Canvas canvas;
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
	private	bool isFirstMovement = true, jumpButtonDown;
	private int currentDonutsPicked = 0, currentTotalJumps = 0, bonusPointsColorizeTile = 100, bonusPointsPencil = 30, bonusPointsUselessTile = 5;

	private	float maxHorizontalSpeed = 1,horizontalSpeed = 0,
					currentSideSpeed,
					speed = 0.2f,
					decreaseFactor = 0.2f;

	public GameObject bonusText;

	public int bonusPoints = 0;

	void Start()
	{
		
		canvas = (Canvas)GameObject.FindObjectOfType<Canvas>();

		cameraShake	= GameObject.FindObjectOfType<CameraShake>();
		gameManager = GameObject.FindObjectOfType<GameManager> ();
		audioManager = GameObject.FindObjectOfType<AudioManager> ();
		prefManager = GameObject.FindObjectOfType<PreferenceManager> ();
		cameraController = GameObject.FindObjectOfType<CameraController> ();
		spawnTileManager = GameObject.FindObjectOfType<SpawnTileManager> ();

		pointsText = GameObject.FindGameObjectWithTag ("DonutPointsText").GetComponent<Text> ();

		GetComponent<SpriteRenderer> ().sprite = ch.sprite;

		donutsPicked = prefManager.getDonut();
		pointsText.text = donutsPicked.ToString ();
	}

	private void initializeCharacterParameters(){

		if (ch.differentProperties) {
			
			jumpSpeed = ch.jumpSpeed;
			sideSpeed = ch.sideSpeed;
			rigidBody.mass = ch.mass;
			slowSideSpeed = ch.slowSideSpeed;

		} else {
			
			rigidBody.mass = mass;
			rigidBody.gravityScale = gravityScale;
		}
	}
		
	public void activate(){

		comic = Instantiate (comicPrefab,comicPrefab.transform.position, Quaternion.identity);
		comic.transform.SetParent (this.transform);

		this.gameObject.AddComponent<Rigidbody2D> ();
		boxCollider2D = gameObject.AddComponent<BoxCollider2D> () as BoxCollider2D;

		rigidBody = GetComponent<Rigidbody2D>();

		rigidBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
		rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;

		initializeCharacterParameters ();

		scoreText = GameObject.FindGameObjectWithTag ("HeightText").GetComponent<Text> ();
		showMessage ();
		active = true;
	}

	void Update(){
		
		if (active) {

			if (!dead && transform.position.y / 20 + bonusPoints > currentRecord) {

				currentRecord = Mathf.FloorToInt (transform.position.y / 20) + bonusPoints;
				scoreText.text = currentRecord.ToString ();
			}

			/*if (rigidBody.velocity.y == 0 && jumped) {
				
			}*/

			//if (isOnTile) {
				//jumped = false;
			//}

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

			#if UNITY_STANDALONE_WIN

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

			if (jumpButtonDown && !jumped && isOnTile && (rigidBody.velocity.y == 0)){



				audioManager.playJumpSound ();
				rigidBody.AddForce (new Vector2 (0, jumpSpeed), ForceMode2D.Impulse);
				jumped = true;

				currentTotalJumps++;

				spawnTileManager.pencilSpawnThisLevel = false;
				spawnTileManager.shurikenSpawnThisLevel = false;
			}

			if (transform.position.y < cameraController.getMinPosition ())
				die ();
		}
	}

	void showBonusPointsText(Transform target, int points){


		GameObject text = Instantiate (bonusText, new Vector3(transform.position.x + Random.Range (-20f, 20f), transform.position.y - Random.Range (0f, 20f), transform.position.z), Quaternion.identity);
		scoreText.GetComponent<Animator> ().SetTrigger ("Blink");

		text.GetComponent<Text> ().text = points.ToString();
		bonusPoints += points;
		text.transform.SetParent(GameObject.FindGameObjectWithTag("PointsCanvas").transform);

		text.transform.localPosition = new Vector3(target.position.x, target.position.y, target.position.z);

	}

	void OnCollisionEnter2D(Collision2D collision){
		
		GameObject collidingObject = collision.gameObject;

		switch(collidingObject.tag){

		case "ColorizeTile":
		case "BounceTile":
			
			if (boxCollider2D.bounds.min.y >= collidingObject.GetComponent<BoxCollider2D> ().bounds.max.y) {
				
				collidingObject.GetComponent<TileBehavior> ().behave ();

				if(collidingObject.CompareTag("ColorizeTile"))
					showBonusPointsText (collision.transform, bonusPointsColorizeTile);

				isOnTile = true;
				jumped = false;
			}


			break;

		case "Tile":
		case "CloudTile":
		case "UselessTile":
		case "AlterningTile":
			
			if(boxCollider2D.bounds.min.y > collidingObject.transform.position.y ) {

				isOnTile = true;
				jumped = false;

				spawnTileManager.spawnEnemies ();
				collidingObject.GetComponent<TileBehavior> ().behave();

				if(collidingObject.CompareTag("UselessTile"))
					showBonusPointsText (collision.transform, bonusPointsUselessTile);
			}

			break;

		case "Pencil":
			
			if (jumped)
			cameraShake.startShake ();

			if (boxCollider2D.bounds.min.y >= collidingObject.transform.position.y) {

				isOnTile = true;
				jumped = false;

				this.transform.SetParent (collidingObject.transform);
				collidingObject.GetComponent<PencilMovement> ().showExplosion ();

				showBonusPointsText (collision.transform, bonusPointsPencil);

				prefManager.increaseTotalPencilExplosion ();

			} else if (boxCollider2D.bounds.min.y <= collidingObject.transform.position.y && boxCollider2D.bounds.min.x <= collidingObject.GetComponent<BoxCollider2D> ().bounds.min.x && !dead) {
				prefManager.increaseDeathByPencil ();
				die ();
			}

			break;

		case "MovingTile":
			
			if(boxCollider2D.bounds.min.y > collidingObject.transform.position.y) {

				this.transform.SetParent(collidingObject.transform);
				isOnTile = true;
				jumped = false;
			}

			break;
		case "SpineTile":
			
			if (boxCollider2D.bounds.min.y >= collidingObject.GetComponent<BoxCollider2D> ().bounds.max.y && !dead) {
				prefManager.increaseDeathBySpine ();
				die ();
			}

			break;
		case "Shuriken":
			if (!dead) {
				prefManager.increaseDeathByShuriken ();
				die ();
			}
			break;
		}
	}

	void OnCollisionExit2D(Collision2D collision){

		string tag = collision.gameObject.gameObject.tag;

		switch (tag) {

		case "ColorizeTile":
		case "BounceTile":
		case "Tile":
		case "CloudTile":
		case "UselessTile":
		case "AlterningTile":
			if (jumped)
				isOnTile = false;
			break;

		case "Pencil":
			transform.SetParent (null);
			if (jumped)
				isOnTile = false;
			break;

		case "MovingTile":
			transform.SetParent (null);
			if (jumped)
				isOnTile = false;
			break;
		}
	}

	void die(){

		if (!dead)
		{
			dead = true;
			active = false;

			Handheld.Vibrate();
			prefManager.increasePlayNumber ();
			prefManager.increaseTotalDonutsPicked (currentDonutsPicked);
			prefManager.increaseTotalJumps (currentTotalJumps);
			prefManager.increaseTotalHeight (currentRecord);

			cameraShake.startBigBigShake ();
			FindObjectOfType<CameraController> ().changeRandomBackgroundColor ();

			audioManager.playExplosionSound ();
			audioManager.playGameOverSound();
			audioManager.changeBackgroundTrack ();

			Vector2 v = new Vector2(transform.position.x - 3, transform.position.y - 30 + 1);
			Instantiate(deadBodyPrefab, v, Quaternion.identity);


			cameraController.active = false;
			GetComponent<SpriteRenderer> ().enabled = false;

			StartCoroutine (callGameEndAfter (1.5f));
		}
	}

	private IEnumerator callGameEndAfter(float time){

		yield return new WaitForSeconds(time);

		this.gameObject.SetActive(false);
		gameManager.gameEnd ();
	}

	public void increaseDonutsPicked(){


		audioManager.playDonutsPickupSound ();
		currentDonutsPicked++;
		donutsPicked++;
		pointsText.text = donutsPicked.ToString ();
  	}

	public bool isPlayerVerticalStill(){

		if ((!jumpButtonDown && rigidBody.velocity.y == 0) || dead)
			return true;
		else
			return false;
	}
}