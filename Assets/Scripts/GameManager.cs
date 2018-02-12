using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public Canvas menu;

	public bool useButtons;
	public Animator cameraAnim;

	public CameraController cameraController;
	public bool isFirstPlay = true;

	public Animator menuAnim;
	public Text recordText;

	public GameObject foreground;

	public void gameEnd(){

		int record = PlayerPrefs.GetInt ("Record", 0);

		PlayerPrefs.SetInt ("Donuts", player.donutsPicked);

		recordText.text = "Best " + record.ToString ();

		menuAnim.SetTrigger ("GameOver");

		cameraController.active = false;
		cameraAnim.SetTrigger ("CameraZoomOff");



		if (player.currentRecord > record) {
			PlayerPrefs.SetInt ("Record", player.currentRecord);
			record = player.currentRecord;
		}
	}

	public void showVideoForDonuts(){

		#if UNITY_ANDROID



		string body,subject;

		//show video add


		showDonutPrize();

		#endif
	}

	public void shareForDonuts(){

		#if UNITY_ANDROID

	

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


		showDonutPrize();

		#endif
	}

	int donutPrize = 100;

	void showDonutPrize ()
	{

		PlayerPrefs.SetInt ("Donuts", PlayerPrefs.GetInt("Donuts",0) + donutPrize);
		throw new System.NotImplementedException ();
	}


	PlayerMovement player;
	public sliderManager sm;

	void Start(){

		//int characterSelected = PlayerPrefs.GetInt ("CharacterSelected", 0);
		sm.InstantiateCharacter();

		player = GameObject.FindGameObjectWithTag ("player").GetComponent<PlayerMovement>();

		menuAnim = menu.GetComponent<Animator> ();
		cameraAnim = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Animator> ();

		menuAnim.SetTrigger ("MenuOn");

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

	public GameObject optionsMenu2;

	bool isOptionsMenu2Closed = true;

	public void manageMenuOptions2(){

		if(isOptionsMenu2Closed)
			optionsMenu2.SetActive (true);
		else 
			optionsMenu2.SetActive (false);

		isOptionsMenu2Closed = !isOptionsMenu2Closed;
	}

	public GameObject optionsMenu;

	bool isOptionsMenuClosed = true;

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
		cameraAnim.SetTrigger ("CameraZoomCharacter");
		foreground.SetActive (false);
	}

	public void closeCharacterSelectionMenu(){

		//menuAnim.SetTrigger ("MenuOn");
		cameraAnim.SetTrigger ("CameraZoomOn");
		foreground.SetActive (true);
	}

	public void closeCharacterSelectionMenuShowMenu(){

		sm.close ();

		menuAnim.SetTrigger ("MenuOn");
		cameraAnim.SetTrigger ("CameraZoomOn");
		foreground.SetActive (true);
	}


	public void changeButtonsUse(){
		useButtons = !useButtons;
	}
}
