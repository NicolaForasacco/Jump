using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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


	private bool isSettingsMenuClosed = true, isOptionsMenuClosed = true;

	private int prizeForSharing = 30, prizeForLike = 30, prizeForGift = 100, prizeForVideo = 100;

	private PlayerMovement player;
	private PreferenceManager prefManager;


	void Start(){

		prefManager = GameObject.FindObjectOfType<PreferenceManager> ();

		sm.InstantiateCharacter();

		player = GameObject.FindGameObjectWithTag ("player").GetComponent<PlayerMovement>();

		menuAnim = menu.GetComponent<Animator> ();
		cameraAnim = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Animator> ();

		menuAnim.SetTrigger ("MenuOn");
		characterButton.GetComponent<ButtonBlink> ().blinkActive = (sm.existCharacterToBuy ()) ? true : false;

		//PlayerPrefs.DeleteAll ();
	}

	public void startGame(){

		if (!isFirstPlay) {
			SceneManager.LoadScene ("MainScene", LoadSceneMode.Single);
		} else {
			
			isFirstPlay = false;
			cameraController.active = true;
			foreground.SetActive (true);
			menuAnim.SetTrigger ("ControlsOn");

			if (!isOptionsMenuClosed)
				manageMenuOptions();

			if (!isSettingsMenuClosed)
				manageMenuOptions2();

			if(player!= null)
				player.activate ();
			else
				player = GameObject.FindGameObjectWithTag ("player").GetComponent<PlayerMovement>();

			if(player!= null)
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

		if (Random.value <= 0.5f && !prefManager.likeAlreadyGiven()) {

			likeForDonutsPanel.SetActive (true);
			likeForDonutsPanel.GetComponentInChildren<ButtonBlink> ().blinkActive = true;
			return true;
		}

		return false;
	}

	bool manageVideoForDonutsPanel(){

		if (Random.value <= 0.6f) {

			videoForDonutsPanel.SetActive (true);
			videoForDonutsPanel.GetComponentInChildren<ButtonBlink> ().blinkActive = true;
			return true;
		}

		return false;
	}

	bool manageshareForDonutsPanel(){

		if (Random.value <= 0.2f || newBestText.activeSelf) {

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

		if ((System.DateTime.Now.Day - lastTime.Day > 0 && (24 - lastTime.Hour + System.DateTime.Now.Hour > 2)) 
			|| System.DateTime.Now.Hour - lastTime.Hour > 2 
			|| lastTimeGiftPicked.CompareTo ("") == 0) {

			donutsPresentsPanel.SetActive (true);

			donutsPresentsPanel.GetComponentInChildren<ButtonBlink> ().blinkActive = true;
			prefManager.setLasttimeGift (System.DateTime.Now.ToString());

			return true;
		} else {

			if (Random.value <= 0.3f) {
				donutsGiftSoonPanel.SetActive (true);

				int minuteLeft = ((lastTime.Hour*60 + 3*60) - System.DateTime.Now.Hour*60 - System.DateTime.Now.Minute);

				int hourLeft = (int)Mathf.Floor(minuteLeft / 60);
				minuteLeft = minuteLeft % 60;
				donutsGiftSoonPanel.GetComponentInChildren<Text>().text = "NEXT GIFT IN " + hourLeft + "H " + minuteLeft + "M";

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



		//show video add

		videoForDonutsPanel.SetActive(false);
		showDonutPrize(prizeForVideo);

		#endif
	}


	public void shareForDonuts(){

		#if UNITY_ANDROID


		/*string body = "My record in Rage Jump is " + prefManager.getRecord() + ". Can you beat my record?", subject = "";

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
*/

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
		showDonutPrize (prizeForGift);
	}


	private GameObject lastDonutExplosion;

	void showDonutPrize (int donutPrize)
	{
		prefManager.setDonut (prefManager.getDonut() + donutPrize);

		donutPrizePanel.SetActive (true);
		lastDonutExplosion = Instantiate (donutExplosion, new Vector3(cam.transform.position.x, cam.transform.position.y, 0), Quaternion.identity);

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


	public void showCreditsPanel(){
		creditsPanel.SetActive (true);
	}

	public void closeCreditsPanel(){
		creditsPanel.SetActive (false);
	}



	public void manageMenuOptions2(){

		if(isSettingsMenuClosed)
			settingsMenu.SetActive (true);
		else 
			settingsMenu.SetActive (false);

		isSettingsMenuClosed = !isSettingsMenuClosed;
	}

	public void manageMenuOptions(){

		if(isOptionsMenuClosed)
			optionsMenu.SetActive (true);
		else 
			optionsMenu.SetActive (false);

		isOptionsMenuClosed = !isOptionsMenuClosed;
	}



	public void showCharacterSelectionMenu(){

		ground.SetActive (false);
		sm.openSelection ();
		menuAnim.SetTrigger ("CharacterSelectionMenu");
		cameraController.zoomOn ();
		foreground.SetActive (false);}

	public void closeCharacterSelectionMenu(){

		//menuAnim.SetTrigger ("MenuOn");

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
