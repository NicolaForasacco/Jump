using System;
using System.Reflection;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdsManager : MonoBehaviour {

  	private RewardBasedVideoAd rewardBasedVideo;

	public GameManager gm;

  	public void Start(){
		
		gm = FindObjectOfType<GameManager> ();
		
          /*#if UNITY_ANDROID
              string appId = "ca-app-pub-3940256099942544~3347511713";
          #elif UNITY_IPHONE
              string appId = "ca-app-pub-3940256099942544~1458002511";
          #else
              string appId = "unexpected_platform";
          #endif

          // Initialize the Google Mobile Ads SDK.
          MobileAds.Initialize(appId);*/


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


        this.RequestRewardedVideo();
      }

      private void RequestRewardedVideo()
    {
		
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
        MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");

		gm.showDonutPrize (gm.prizeForVideo);
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message);

		gm.showDonutPrize (gm.prizeForVideo);
    }

  	public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoOpened event received");

		gm.showDonutPrize (gm.prizeForVideo);
    }

    public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoStarted event received");

		gm.showDonutPrize (gm.prizeForVideo);
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoClosed event received");

		gm.showDonutPrize (gm.prizeForVideo);
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        //string type = args.Type;
        //double amount = args.Amount;
       MonoBehaviour.print("HandleRewardBasedVideoRewarded event received for ");

		gm.showDonutPrize (gm.prizeForVideo);

    }

    public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");

		gm.showDonutPrize (gm.prizeForVideo);
    }
}
