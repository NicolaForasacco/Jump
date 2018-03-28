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

	public GameObject videoForDonutsPanel,
						shareForDonutsPanel,
						donutsPresentsPanel,
						likeForDonutsPanel,
						donutPrizePanel,
						donutsGiftSoonPanel,
						donutExplosion,
						foreground,
						bestText,
						newBestText,
						settingsMenu,
						optionsMenu,
						creditsPanel,
						characterButton, 
						ground;

	private bool isSettingsMenuClosed = true;//, isOptionsMenuClosed = true;

	public int prizeForSharing = 30, prizeForLike = 30, prizeForGift = 100, prizeForVideo = 100, hourBetweenGift = 3;

	private float showVideoPanelP = 0.7f, showLikePanelP = 0.5f, showSharePanelP = 0.2f, showGiftSoonPanelP = 0.3f;

	private PlayerMovement player;
	private PreferenceManager prefManager;
	private GameObject lastDonutExplosion;
	private AdsManager adsManager;
	private AudioManager audioManager;

	public Text testText;

	void Start(){

		audioManager = GameObject.FindObjectOfType<AudioManager> ();
		adsManager = GameObject.FindObjectOfType<AdsManager> ();
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

		manageVideoForDonutsPanel ();
		manageDailyGiftPanel ();

		if(!manageshareForDonutsPanel ())
			manageLikeForDonutsPanel ();

		//#endif
	}


	#region EndGamePanels

	bool manageLikeForDonutsPanel(){

		if (UnityEngine.Random.value <= showLikePanelP && !prefManager.likeAlreadyGiven()) {

			likeForDonutsPanel.SetActive (true);
			likeForDonutsPanel.GetComponentInChildren<ButtonBlink> ().blinkActive = true;
			return true;
		}

		return false;
	}

	bool manageVideoForDonutsPanel(){

		if (UnityEngine.Random.value <= showVideoPanelP && adsManager.rewardBasedVideo.IsLoaded()) {

			videoForDonutsPanel.SetActive (true);
			videoForDonutsPanel.GetComponentInChildren<ButtonBlink> ().blinkActive = true;
			return true;
		}

		return false;
	}

	bool manageshareForDonutsPanel(){

		if (UnityEngine.Random.value <= showSharePanelP || newBestText.activeSelf) {

			shareForDonutsPanel.SetActive (true);
			shareForDonutsPanel.GetComponentInChildren<ButtonBlink> ().blinkActive = true;
			return true;
		}

		return false;
	}

	bool manageDailyGiftPanel(){

		string lastTimeGiftPicked = prefManager.getLastTimeGift();

		System.DateTime lastTime;

		System.DateTime.TryParse (lastTimeGiftPicked,out lastTime);

		int currentHoursInMinute = System.DateTime.Now.Hour * 60;
		int currentMinute = System.DateTime.Now.Minute;
		int lastHourInMinute = lastTime.Hour * 60; 
		int hourBetweenGiftInMinute = hourBetweenGift * 60;
		int lastMinutesToMidnight = (24 - lastTime.Hour) * 60 - lastTime.Minute;

		if ((System.DateTime.Now.Day - lastTime.Day > 0 && (lastMinutesToMidnight + currentHoursInMinute + currentMinute >= hourBetweenGiftInMinute)) 
			|| (currentHoursInMinute + currentMinute - lastHourInMinute - lastTime.Minute >= hourBetweenGiftInMinute && System.DateTime.Now.Day == lastTime.Day)
			|| lastTimeGiftPicked.CompareTo ("") == 0) {

			donutsPresentsPanel.SetActive (true);
			donutsPresentsPanel.GetComponentInChildren<ButtonBlink> ().blinkActive = true;

			return true;
		} else {

			if (UnityEngine.Random.value <= showGiftSoonPanelP) {
				donutsGiftSoonPanel.SetActive (true);

				int minuteLeft = 0, hourLeft = 0;

				if (System.DateTime.Now.Day == lastTime.Day)
					minuteLeft = ((lastHourInMinute + lastTime.Minute + hourBetweenGiftInMinute) - currentHoursInMinute - currentMinute);
				else if(System.DateTime.Now.Day > lastTime.Day)
					minuteLeft = ((hourBetweenGiftInMinute - lastMinutesToMidnight) - currentHoursInMinute - currentMinute);

				hourLeft = (int)Mathf.Floor (minuteLeft / 60);
				minuteLeft = minuteLeft % 60;
				donutsGiftSoonPanel.GetComponentInChildren<Text> ().text = "NEXT GIFT IN " + hourLeft + "H " + minuteLeft + "M";
			}
		}

		return false;
	}
		
	/*void showVideoForDonutsPanel(){
		videoForDonutsPanel.SetActive(true);
	}

	void showShareForDonutsPanel(){
		shareForDonutsPanel.SetActive(true);
  	}
	*/	

	public void showVideoForDonuts(){

		#if UNITY_ANDROID

		videoForDonutsPanel.SetActive(false);
		adsManager.showVideo();

		#endif
	}


	public void shareForDonuts(){

		#if UNITY_ANDROID


		string body = "My record in Rage Jump is " + prefManager.getRecord() + ". Can you beat my record?", subject = "";

		//Reference of AndroidJavaClass class for intent
		AndroidJavaClass intentClass = new AndroidJavaClass ("android.content.Intent");
		//Reference of AndroidJavaObject class for intent
		AndroidJavaObject intentObject = new AndroidJavaObject ("android.content.Intent");
		//call setAction method of the Intent object created
		intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
		//set the type of sharing that is happening
		intentObject.Call<AndroidJavaObject>("setType", "text/plain");
		//add data to be passed to the other activity i.e., the data to be sent
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), body);
		//get the current activity
		AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
		//start the activity by sending the intent data
		currentActivity.Call ("startActivity", intentObject);


		shareForDonutsPanel.SetActive(false);
		showDonutPrize(prizeForSharing);

		#endif
	}

	public void likeForDonuts (){

		#if UNITY_ANDROID

		prefManager.likeGiven();
		likeForDonutsPanel.SetActive (false);
		Application.OpenURL("market://details?id=" + Application.productName);
		showDonutPrize (prizeForLike);

		#endif
	}



	public void showDonutPresent(){
		
		donutsPresentsPanel.SetActive(false);

		prefManager.setLasttimeGift (System.DateTime.Now.ToString());
		showDonutPrize (prizeForGift);
	}



	public void showDonutPrize (int donutPrize)
	{
		donutPrizePanel.SetActive (true);
		lastDonutExplosion = Instantiate (donutExplosion, new Vector3(cam.transform.position.x, cam.transform.position.y, 0), Quaternion.identity);

		prefManager.setDonut (prefManager.getDonut() + donutPrize);
		donutText.text = prefManager.getDonut().ToString();
	}

	public void closeDonutPrizePanel(){

		GameObject.Destroy (lastDonutExplosion);
		donutPrizePanel.SetActive (false);
	}

	#endregion

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

	public GameObject statsPanel;
	public Text statText;

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
