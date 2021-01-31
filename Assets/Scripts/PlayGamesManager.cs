using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System;

public class PlayGamesManager : MonoBehaviour {

	public void signIn(){

		
		#if UNITY_ANDROID

		try{
			if(Application.internetReachability != NetworkReachability.NotReachable){
			
				PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
				PlayGamesPlatform.InitializeInstance(config);
				PlayGamesPlatform.Activate();

				Social.localUser.Authenticate(success => { });
		 	}
		}catch(NullReferenceException  e){
			return;
		}
		
		#endif

		
	}


	public void AddScoreToLeaderboard(string leaderboardId, long score)
	{

		
		#if UNITY_ANDROID
		try{
			Social.ReportScore(score, leaderboardId, success => { });	
		}catch(NullReferenceException  e){
			return;
		}
		

		#endif

		
	}

	public bool showLeatherboard(string leaderboardId){

		

		#if UNITY_ANDROID
		try{
			
			

			PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboardId);

			
			return true;
		}catch(NullReferenceException  e){
			
			return false;
		}
		#endif

		return false;
	}
}
