using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreferenceManager : MonoBehaviour {

	string likeForDonutString = "LikeForDonutUsed", 
			lastTimeGitPickedString = "LastTimeGiftPicked", 
			recordString = "Record", 
			donutsString = "Donuts", 
			isBoughtString = "Isbought",
			lastCharacterSelectedString = "CharacterSelected",
			soundOnString = "SoundOn";

	public bool isFirstTime(){
		
		return (getRecord () == 0) ? true : false;
	}

	public bool hasBeenBought(int id){
		
		return (PlayerPrefs.GetInt (isBoughtString + id, 0) == 1);
	}

	public void setHasBought(int id){

		PlayerPrefs.SetInt (isBoughtString + id, 1);
	}


	public int getRecord(){

		return PlayerPrefs.GetInt (recordString, 0);
	}

	public void setRecord(int record){

		PlayerPrefs.SetInt (recordString, record);
	}


	public void setDonut(int donut){

		PlayerPrefs.SetInt (donutsString, donut);
	}

	public int getDonut(){

		return PlayerPrefs.GetInt (donutsString, 0);
	}


	public bool likeAlreadyGiven(){

		return (PlayerPrefs.GetInt (likeForDonutString, 0) == 1);
	}

	public void likeGiven(){
		
		PlayerPrefs.SetInt (likeForDonutString, 1);
	}


	public string getLastTimeGift(){
		
		return PlayerPrefs.GetString (lastTimeGitPickedString, "");
	}

	public void setLasttimeGift(string time){
		
		PlayerPrefs.SetString (lastTimeGitPickedString, time);
	}


	public int getLastSelectedCharacter(){

		return PlayerPrefs.GetInt (lastCharacterSelectedString, 0);
	}

	public void setLastSelectedCharacter(int id){

		PlayerPrefs.SetInt (lastCharacterSelectedString, id);
	}


	public bool getSoundOn(){

		return (PlayerPrefs.GetInt (soundOnString, 1) == 1);
	}

	public void setSoundOn(bool on){

		if(on)
			PlayerPrefs.SetInt (soundOnString, 1);
		else
			PlayerPrefs.SetInt (soundOnString, 0);
	}
}
