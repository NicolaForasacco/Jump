using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour {

	public Canvas menu;
	public bool useButtons, isFirstPlay = true;
	public Animator cameraAnim, menuAnim;
	public CameraController cameraController;
	public Text recordText, donutText;
	public Camera cam;
	public sliderCharacterManager sm;

	public GameObject donutPrizePanel,
						foreground,
						bestText,
						newBestText,
						settingsMenu,
						optionsMenu,
						creditsPanel,
						characterButton, 
						ground,
						statsPanel;

	public Text testText, statText;


	private bool isSettingsMenuClosed = true;//, isOptionsMenuClosed = true;

	private PlayerMovement player;
	private PreferenceManager prefManager;
	private AudioManager audioManager;
	private EndPanelsManager endPanelsManager;
	private PlayGamesManager playGamesManager;


	void Start(){
		
		endPanelsManager = GameObject.FindObjectOfType<EndPanelsManager> ();
		audioManager = GameObject.FindObjectOfType<AudioManager> ();
		prefManager = GameObject.FindObjectOfType<PreferenceManager> ();
		playGamesManager = GameObject.FindObjectOfType<PlayGamesManager> ();
	
		sm.InstantiateCharacter();

		player = GameObject.FindGameObjectWithTag ("player").GetComponent<PlayerMovement>();

		menuAnim = menu.GetComponent<Animator> ();
		cameraAnim = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Animator> ();

		menuAnim.SetTrigger ("MenuOn");
		characterButton.GetComponent<ButtonBlink> ().blinkActive = (sm.existCharacterToBuy ()) ? true : false;

		audioManager.restart ();

		playGamesManager.signIn ();
	}

	public void startGame(){

		if (!isFirstPlay) {
			SceneManager.LoadScene ("MainScene", LoadSceneMode.Single);
		} else {
			
			isFirstPlay = false;
			cameraController.active = true;
			foreground.SetActive (true);
			menuAnim.SetTrigger ("ControlsOn");

			if (!isSettingsMenuClosed)
				manageMenuOptions2();

			while(player == null)
				player = GameObject.FindGameObjectWithTag ("player").GetComponent<PlayerMovement>();
			
			player.activate ();
		}
	}
		
	public void gameEnd(){

		int record = prefManager.getRecord ();
		prefManager.setDonut (player.donutsPicked);

		recordText.text = "Best " + record.ToString ();

		cameraController.active = false;

		menuAnim.SetTrigger ("GameOver"); 

		if (player.currentRecord > record) {

			newBestText.SetActive (true);
			bestText.SetActive (false);

			prefManager.setRecord (player.currentRecord);
			record = player.currentRecord;

			//playGamesManager.AddScoreToLeaderboard (GPGSIds.leaderboard_top_scores, record);

		} else {
			
			//playGamesManager.AddScoreToLeaderboard (GPGSIds.leaderboard_top_scores, record);
			bestText.SetActive (true);
			newBestText.SetActive (false);
		}

		managePanels ();
	}

	void Update(){

		#if UNITY_EDITOR_WIN
		if (Input.GetButton ("Submit") && !cameraController.active) {
			sm.selectCurrentCharacter ();
		}

		#endif

		#if UNITY_STANDALONE_WIN

		if (Input.GetButton ("Submit") && !cameraController.active) {
		sm.selectCurrentCharacter ();
		}

		#endif


	}

	public void restartGame(){

		menuAnim.SetTrigger ("NoUI");
		StartCoroutine (reloadScene(1.2f));
	}

	void managePanels(){

		#if UNITY_ANDROID

		if(Application.internetReachability != NetworkReachability.NotReachable){

			endPanelsManager.manageVideoForDonutsPanel();
			if(!endPanelsManager.manageshareForDonutsPanel ())
				endPanelsManager.manageLikeForDonutsPanel ();
		}

		#endif

		if(!endPanelsManager.manageDailyGiftPanel ())
			endPanelsManager.manageRandomGiftPanel();
	}
		
	private IEnumerator reloadScene(float time){

		yield return new WaitForSeconds(time);

		SceneManager.LoadScene ("MainScene", LoadSceneMode.Single);
	}


	public void showOnlineShop(){
		Application.OpenURL("");
	}

	public void showSocialProfile(){
		Application.OpenURL("https://twitter.com/");
	}

	public void showLeatherboard(){
		if(!playGamesManager.showLeatherboard (GPGSIds.leaderboard_top_scores))
			playGamesManager.signIn ();
	}

	public void showCreditsPanel(){
		creditsPanel.SetActive (true);
	}

	public void showStatsPanel(){
		
		statsPanel.SetActive (true);
		string stat = prefManager.getPlaysNumber () + "\n"
					+ prefManager.getTotalDonutsPicked () + "\n"
		            + prefManager.getTotalJumps () + "\n"
		            + prefManager.getTotalHeight () + "\n"
		            + prefManager.getDeathByPencil () + "\n"
		            + prefManager.getDeathByShuriken () + "\n"
		            + prefManager.getDeathBySpine () + "\n"
					+ prefManager.getTotalPaintButton() + "\n"
					+ prefManager.getTotalFlyingAnimalsScared() + "\n"
					+ prefManager.getTotalPencilExplosion () + "\n"
					+ (int)(prefManager.getTotalJumps ()/prefManager.getPlaysNumber ()) + "\n"
					+ (int)(prefManager.getTotalHeight ()/prefManager.getPlaysNumber ());

		statText.text = stat;
	}

	public void showCharacterSelectionMenu(){

		settingsMenu.SetActive (false);
		isSettingsMenuClosed = true;
		ground.SetActive (false);
		menuAnim.SetTrigger ("CharacterSelectionMenu");
		cameraController.zoomOn ();
		foreground.SetActive (false);
		sm.openSelection ();
	}


	public void closeCreditsPanel(){
		creditsPanel.SetActive (false);
	}

	public void closeStatsPanel(){
		statsPanel.SetActive (false);
	}

	public void closeCharacterSelectionMenu(){
		
		ground.SetActive (true);
		cameraController.zoomOff();
		foreground.SetActive (true);
		sm.close ();
	}

	public void closeCharacterSelectionMenuAndShowMenu(){

		ground.SetActive (true);
		sm.close ();
		menuAnim.SetTrigger ("MenuOn");
		cameraController.zoomOff ();
		foreground.SetActive (true);
	}


	public void manageMenuOptions2(){

		if(isSettingsMenuClosed)
			settingsMenu.SetActive (true);
		else 
			settingsMenu.SetActive (false);

		isSettingsMenuClosed = !isSettingsMenuClosed;
	}
}
