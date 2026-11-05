using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndPanelsManager : MonoBehaviour {

	public GameObject likeForDonutsPanel,
						videoForDonutsPanel,
						shareForDonutsPanel,
						donutsGiftSoonPanel,
						donutsPresentsPanel,
						donutPrizePanel,
						donutExplosion,
						newBestText,
						randomGift;

	public Text donutText, donutPrizeText;

	private GameObject lastDonutExplosion;
	private PreferenceManager prefManager;

	private int prizeOnResume = 0, hourBetweenGift = 3;
	private bool showPrizeOnResume = false, prizeForVideoGiven = false;


	public int prizeForSharing, 
				prizeForLike, 
				prizeForGift,
				minRandomGift, 
				maxRandomGift, 
				prizeForVideo;

	private float showVideoPanelP = 0.95f,
					showSharePanelP = 0.2f,
					showGiftSoonPanelP = 0.25f,
					showRandomGiftP = 0.2f;

	private string productName = "brickteamdevelopment.ragejump";

	void Start(){

		prefManager = FindObjectOfType<PreferenceManager> ();
	}

	public bool manageLikeForDonutsPanel(){

		if (!prefManager.likeAlreadyGiven()) {

			likeForDonutsPanel.SetActive (true);
			likeForDonutsPanel.GetComponentInChildren<ButtonBlink> ().blinkActive = true;
			return true;
		}

		return false;
	}
		
	public bool manageVideoForDonutsPanel(){
		return false;
	}
	public bool manageshareForDonutsPanel(){

		if (Random.value <= showSharePanelP || newBestText.activeSelf) {

			shareForDonutsPanel.SetActive (true);
			shareForDonutsPanel.GetComponentInChildren<ButtonBlink> ().blinkActive = true;
			return true;
		}

		return false;
	}

	public bool manageDailyGiftPanel(){

		string lastTimeGiftPicked = prefManager.getLastTimeGift();

		System.DateTime lastTime;

		System.DateTime.TryParse (lastTimeGiftPicked,out lastTime);

		int currentHoursInMinute = System.DateTime.Now.Hour * 60;
		int currentMinute = System.DateTime.Now.Minute;
		int lastHourInMinute = lastTime.Hour * 60; 
		int hourBetweenGiftInMinute = hourBetweenGift * 60;
		int lastMinutesToMidnight = (24 - lastTime.Hour) * 60 - lastTime.Minute;

		int minuteLeft = 0, hourLeft = 0;

		if (System.DateTime.Now.Day == lastTime.Day)
			minuteLeft = ((lastHourInMinute + lastTime.Minute + hourBetweenGiftInMinute) - currentHoursInMinute - currentMinute);
		else if(System.DateTime.Now.Day > lastTime.Day)
			minuteLeft = ((hourBetweenGiftInMinute - lastMinutesToMidnight) - currentHoursInMinute - currentMinute);

		if (minuteLeft <= 0 || lastTimeGiftPicked.CompareTo ("") == 0) {

			donutsPresentsPanel.SetActive (true);
			donutsPresentsPanel.GetComponentInChildren<ButtonBlink> ().blinkActive = true;

			return true;
		} else {

			if (Random.value <= showGiftSoonPanelP) {

				donutsGiftSoonPanel.SetActive (true);

				hourLeft = (int)Mathf.Floor (minuteLeft / 60);
				minuteLeft = minuteLeft % 60;
				donutsGiftSoonPanel.GetComponentInChildren<Text> ().text = "NEXT GIFT IN " + hourLeft + "H " + minuteLeft + "M";

				return true;
			}
		}

		return false;
	}

	public void manageRandomGiftPanel(){
		
		if(Random.value <= showRandomGiftP)
			randomGift.SetActive (true);
	}

	/*void showVideoForDonutsPanel(){
		videoForDonutsPanel.SetActive(true);
	}

	void showShareForDonutsPanel(){
		shareForDonutsPanel.SetActive(true);
  	}
	*/	

	public void showVideoForDonuts(){
		videoForDonutsPanel.SetActive(false);
	}
	public void shareForDonuts(){

		#if UNITY_ANDROID

		showPrizeOnResume = true;
		prizeOnResume = prizeForSharing;

		string body = "My record in Rage Jump is " + prefManager.getRecord() + ".\nCan you beat my record? Try it " + "https://play.google.com/store/apps/details?id=" + productName, subject = "";

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

		#endif
	}

	public void likeForDonuts (){

		#if UNITY_ANDROID

		showPrizeOnResume = true;
		prizeOnResume = prizeForLike;

		prefManager.likeGiven();
		likeForDonutsPanel.SetActive (false);
		Application.OpenURL("market://details?id=" + productName);

		#endif
	}

	public void showDonutPresent(){

		donutsPresentsPanel.SetActive(false);

		prefManager.setLasttimeGift (System.DateTime.Now.ToString());
		showDonutPrize (prizeForGift);
	}

	public void showDonutRandomPresent(){
		
		randomGift.SetActive (false);
		showDonutPrize (Random.Range(minRandomGift, maxRandomGift));
	}


	void OnApplicationFocus(bool hasFocus)
	{
		if(hasFocus && showPrizeOnResume){
			showDonutPrize(prizeOnResume);

			prizeOnResume = 0;
			showPrizeOnResume = false;
		}
	}


	public void showDonutPrize (int donutPrize)
	{

		donutPrizeText.text = donutPrize.ToString ();
		donutPrizePanel.SetActive (true);
		lastDonutExplosion = Instantiate (donutExplosion, new Vector3(FindObjectOfType<Camera>().transform.position.x, FindObjectOfType<Camera>().transform.position.y, 0), Quaternion.identity);
		int totalDonut = prefManager.getDonut () + donutPrize;
		prefManager.setDonut (totalDonut);
		donutText.text = prefManager.getDonut().ToString();
	}

	public void showDonutPrizeForVideo ()
	{
		if (!prizeForVideoGiven) {
			
			prizeForVideoGiven = true;
			float rf = Random.value;
			int currendVideoPrize = 0;

			if (rf < 0.1f)
				currendVideoPrize = 250;
			else if (rf < 0.6f)
				currendVideoPrize = 500;
			else if (rf < 0.8f)
				currendVideoPrize = 750;
			else
				currendVideoPrize = 1000;
			

			donutPrizeText.text = currendVideoPrize.ToString ();
			donutPrizePanel.SetActive (true);
			lastDonutExplosion = Instantiate (donutExplosion, new Vector3 (FindObjectOfType<Camera> ().transform.position.x, FindObjectOfType<Camera> ().transform.position.y, 0), Quaternion.identity);

			int totalDonut = prefManager.getDonut () + currendVideoPrize;
			prefManager.setDonut (totalDonut);
			donutText.text = prefManager.getDonut ().ToString ();
		}
	}

	public void closeDonutPrizePanel(){

		GameObject.Destroy (lastDonutExplosion);
		donutPrizePanel.SetActive (false);
	}
}
