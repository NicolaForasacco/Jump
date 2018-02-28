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
	public sliderManager sm;

	public GameObject videoForDonutsPanel,
						shareForDonutsPanel,
						donutPrizePanel,
						donutExplosion,
						foreground,
						bestText,
						newBestText,
						donutsPresentsPanel,
						optionsMenu2,
						optionsMenu;

	public GameObject[] panel1;
	public GameObject[] panel2;



	bool isOptionsMenu2Closed = true, isOptionsMenuClosed = true;

	private PlayerMovement player;

	int donutPrize = 100;


	public void gameEnd(){
		
		int record = PlayerPrefs.GetInt ("Record", 0);

		PlayerPrefs.SetInt ("Donuts", player.donutsPicked);

		recordText.text = "Best " + record.ToString ();

		cameraController.active = false;

		menuAnim.SetTrigger ("GameOver");

		if (player.currentRecord > record) {

			newBestText.SetActive (true);
			bestText.SetActive (false);

			PlayerPrefs.SetInt ("Record", player.currentRecord);
			record = player.currentRecord;
		} else {
			bestText.SetActive (true);
			newBestText.SetActive (false);
		}

		managePanels ();
	}

	void managePanels(){
		
		//#if UNITY_ANDROID

		if(Random.value <= 0.5f)
			panel1 [Random.Range (0, panel1.Length)].SetActive (true);

		/*if (Random.value <= 0.5f)
			showVideoForDonutsPanel ();

		if (Random.value <= 0.5f)
			showShareForDonutsPanel ();*/
		
		//#endif

		manageDailyGift ();
		//manageChallenges ();
	}

	void manageChallenges(){
		
		/*int lastChallenge = PlayerPrefs.GetInt ("LastChallenge", 0);

		if (lastChallenge != 0) {

			showChallengePanel ();

		}*/

	}

	void showChallengePanel(){}

	void manageDailyGift(){

		string lastTimeGiftPicked = PlayerPrefs.GetString ("LastTimeGiftPicked", "");

		System.DateTime lastTime;

		System.DateTime.TryParse (lastTimeGiftPicked,out lastTime);

		if (System.DateTime.Now.Day - lastTime.Day > 0 || lastTimeGiftPicked.CompareTo("") == 0) {
			donutsPresentsPanel.SetActive(true);
			PlayerPrefs.SetString ("LastTimeGiftPicked", System.DateTime.Now.ToString());
		}
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
		showDonutPrize();

		#endif
	}

	public void shareForDonuts(){

		#if UNITY_ANDROID

		/*
		string body = "My record in Rage Jump is " + PlayerPrefs.GetInt ("Record", 0) + ". Can you beat my record?", subject = "";

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
		showDonutPrize();

		#endif
	}

	public void showDonutGift(){

		donutsPresentsPanel.SetActive(false);
		showDonutPrize ();
	}


	void showDonutPrize ()
	{

		PlayerPrefs.SetInt ("Donuts", PlayerPrefs.GetInt("Donuts",0) + donutPrize);

		donutPrizePanel.SetActive (true);
		Instantiate (donutExplosion, new Vector3(cam.transform.position.x, cam.transform.position.y, 0), Quaternion.identity);

		donutText.text = PlayerPrefs.GetInt ("Donuts", 0).ToString();
	}

	public void closeDonutPrizePanel(){
		donutPrizePanel.SetActive (false);
	}

	public GameObject characterButton;		

	void Start(){

		sm.InstantiateCharacter();

		player = GameObject.FindGameObjectWithTag ("player").GetComponent<PlayerMovement>();

		menuAnim = menu.GetComponent<Animator> ();
		cameraAnim = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Animator> ();

		menuAnim.SetTrigger ("MenuOn");

		if (sm.existCharacterToBuy ()) {
			
			Debug.Log ("There are new characters");
			characterButton.GetComponent<ButtonBlink> ().blinkActive = true;
		} else
			characterButton.GetComponent<ButtonBlink> ().blinkActive = false;

		//deleteAll ();
	}

	void deleteAll(){
		
		PlayerPrefs.DeleteAll ();
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

			if (!isOptionsMenu2Closed)
				manageMenuOptions2();

			if(player!= null)
				player.activate ();
			else
				player = GameObject.FindGameObjectWithTag ("player").GetComponent<PlayerMovement>();

			if(player!= null)
				player.activate ();
		}
	}


	public void manageMenuOptions2(){

		if(isOptionsMenu2Closed)
			optionsMenu2.SetActive (true);
		else 
			optionsMenu2.SetActive (false);

		isOptionsMenu2Closed = !isOptionsMenu2Closed;
	}

	public void manageMenuOptions(){

		if(isOptionsMenuClosed)
			optionsMenu.SetActive (true);
		else 
			optionsMenu.SetActive (false);

		isOptionsMenuClosed = !isOptionsMenuClosed;
	}

	public void showCharacterSelectionMenu(){
		
		sm.openSelection ();
		menuAnim.SetTrigger ("CharacterSelectionMenu");
		cameraController.zoomOn ();
		foreground.SetActive (false);
	}

	public void closeCharacterSelectionMenu(){

		//menuAnim.SetTrigger ("MenuOn");
		cameraController.zoomOff();
		foreground.SetActive (true);
	}

	public void closeCharacterSelectionMenuAndShowMenu(){

		sm.close ();

		menuAnim.SetTrigger ("MenuOn");
		cameraController.zoomOff ();
		foreground.SetActive (true);
	}

	public void changeButtonsUse(){
		useButtons = !useButtons;
	}
}
