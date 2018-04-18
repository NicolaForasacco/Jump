using System;
using System.Reflection;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdsManager : MonoBehaviour {

  	public RewardBasedVideoAd rewardBasedVideo;

	bool prizeAlreadyGiven = false; 

	public void Start (){
		
		#if UNITY_ANDROID
		string appId = "ca-app-pub-3940256099942544~3347511713";
		#else
     	string appId = "unexpected_platform";
		#endif

		// Initialize the Google Mobile Ads SDK.
		MobileAds.Initialize (appId);


		// Get singleton reward based video ad reference.
		this.rewardBasedVideo = RewardBasedVideoAd.Instance;

		// Called when an ad request has successfully loaded.
		rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
		// Called when an ad request failed to load.
		rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;

		// Called when an ad is shown.
		rewardBasedVideo.OnAdOpening += HandleRewardBasedVideoOpened;

		// Called when the ad starts to play.
		rewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;

		// Called when the user should be rewarded for watching a video.
		rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;

		// Called when the ad is closed.
		rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;

		// Called when the ad click caused the user to leave the application.
		rewardBasedVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;
		this.RequestRewardedVideo ();
	}
	
	private void RequestRewardedVideo(){


        #if UNITY_ANDROID
			string adUnitId = "ca-app-pub-3940256099942544/5224354917";
        #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/1712485313";
        #else
            string adUnitId = "unexpected_platform";
        #endif
	        
		// Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded video ad with the request.
        this.rewardBasedVideo.LoadAd(request, adUnitId);
	}

    public void showVideo(){

        if (rewardBasedVideo.IsLoaded()) {
          rewardBasedVideo.Show();
        }
    }

	public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardBasedVideoLoaded event received");
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message);
    }

  	public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardBasedVideoOpened event received");
    }

    public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardBasedVideoStarted event received");
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardBasedVideoClosed event received");
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
		Debug.Log("HandleRewardBasedVideoRewarded event received for ");

		if (!prizeAlreadyGiven) {

			prizeAlreadyGiven = true;
			FindObjectOfType<EndPanelsManager> ().showDonutPrizeForVideo ();
		}

    }

    public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args){
        Debug.Log("HandleRewardBasedVideoLeftApplication event received");
	}
}