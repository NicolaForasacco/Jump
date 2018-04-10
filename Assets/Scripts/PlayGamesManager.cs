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

		if(Application.internetReachability != NetworkReachability.NotReachable){
			
			PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
			PlayGamesPlatform.InitializeInstance(config);
			PlayGamesPlatform.Activate();

			Social.localUser.Authenticate(success => { });
		}

		#endif
	}


	public void AddScoreToLeaderboard(string leaderboardId, long score)
	{
		#if UNITY_ANDROID

		Social.ReportScore(score, leaderboardId, success => { });

		#endif
	}

	public bool showLeatherboard(string leaderboardId){

		try{
			
			#if UNITY_ANDROID

			PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboardId);

			#endif
			return true;
		}catch(NullReferenceException  e){
			return false;
		}
	}
}
