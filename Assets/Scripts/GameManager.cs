using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
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


	void Start(){

		endPanelsManager = GameObject.FindObjectOfType<EndPanelsManager> ();
		audioManager = GameObject.FindObjectOfType<AudioManager> ();
		prefManager = GameObject.FindObjectOfType<PreferenceManager> ();

		sm.InstantiateCharacter();

		player = GameObject.FindGameObjectWithTag ("player").GetComponent<PlayerMovement>();

		menuAnim = menu.GetComponent<Animator> ();
		cameraAnim = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Animator> ();

		menuAnim.SetTrigger ("MenuOn");
		characterButton.GetComponent<ButtonBlink> ().blinkActive = (sm.existCharacterToBuy ()) ? true : false;

		audioManager.restart ();
		//PlayerPrefs.DeleteAll ();


		// authenticate user:
		PlayGamesPlatform.Activate();

		PlayGamesPlatform.Instance.Authenticate (SignInCallback, true);

		/*Social.localUser.Authenticate((bool success) => {
			
			if(success)
				testText.text = "SUCCESS";
			else {
				testText.text = "NO";
			}

		});*/
	}

	public void SignInCallback(bool success) {
		if (success)
			testText.text = "SUCCESS";
		else {
			testText.text = "NO";
		}
	}

	public void startGame(){

		if (!isFirstPlay) {
			SceneManager.LoadScene ("MainScene", LoadSceneMode.Single);
		} else {
			
			isFirstPlay = false;
			cameraController.active = true;
			foreground.SetActive (true);
			menuAnim.SetTrigger ("ControlsOn");

			/*if (!isOptionsMenuClosed)
				manageMenuOptions();*/

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

			Social.ReportScore (record, GPGSIds.leaderboard_top_scores, (bool success) => {
				
				if (success)
					testText.text = "SUCCESS";
				else {
					testText.text = "NO";
				}
			});

		} else {
			bestText.SetActive (true);
			newBestText.SetActive (false);
		}

		managePanels ();
	}



	void managePanels(){

		//#if UNITY_ANDROID

		if(Application.internetReachability == NetworkReachability.NotReachable){

			endPanelsManager.manageVideoForDonutsPanel();
			if(!endPanelsManager.manageshareForDonutsPanel ())
				endPanelsManager.manageLikeForDonutsPanel ();
		}

		endPanelsManager.manageDailyGiftPanel ();

		//#endif
	}


	public void restartGame(){
		
		menuAnim.SetTrigger ("NoUI");
		StartCoroutine (reloadScene(1.2f));

	}

	private IEnumerator reloadScene(float time){

		yield return new WaitForSeconds(time);

		SceneManager.LoadScene ("MainScene", LoadSceneMode.Single);
	}

	public void showOnlineShop(){
		Application.OpenURL("");
	}

	public void showLeatherboard(){
		try{
			
			PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_top_scores);

		}catch(NullReferenceException  e){

			testText.text = "NO L";
			

		}
	}

	public void showCreditsPanel(){
		creditsPanel.SetActive (true);
	}

	public void closeCreditsPanel(){
		creditsPanel.SetActive (false);
	}


	public void showStatsPanel(){
		statsPanel.SetActive (true);
		string stat = "TOTAL PLAYS: " + prefManager.getPlaysNumber() + "\n" 
			+ "TOTAL DONUTS PICKED: " + prefManager.getTotalDonutsPicked() + "\n"
			+ "TOTAL JUMPS: " + prefManager.getTotalJumps() + "\n"
			+ "TOTAL HEIGHT: " + prefManager.getTotalHeight() + "\n"
			+ "DEATH BY PENCIL: " + prefManager.getDeathByPencil() + "\n"
			+ "DEATH BY SHURIKEN: " + prefManager.getDeathByShuriken() + "\n"
			+ "PAINT BUTTONS: " + prefManager.getTotalPaintButton() + "\n"
			+ "ANIMALS SCARED: " + prefManager.getTotalFlyingAnimalsScared();
		
		statText.text = stat;

	}

	public void closeStatsPanel(){
		statsPanel.SetActive (false);
	}


	public void manageMenuOptions2(){

		if(isSettingsMenuClosed)
			settingsMenu.SetActive (true);
		else 
			settingsMenu.SetActive (false);

		isSettingsMenuClosed = !isSettingsMenuClosed;
	}


	public void showCharacterSelectionMenu(){

		ground.SetActive (false);
		sm.openSelection ();
		menuAnim.SetTrigger ("CharacterSelectionMenu");
		cameraController.zoomOn ();
		foreground.SetActive (false);}

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
}
