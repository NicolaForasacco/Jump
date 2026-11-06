using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreferenceManager : MonoBehaviour {

	string likeForDonutString = "LikeForDonutUsed", 
			lastTimeGiftPickedString = "LastTimeGiftPicked", 
			recordString = "Record",
			donutsString = "Donuts",
			isBoughtString = "IsBought",
			lastCharacterSelectedString = "CharacterSelected",
			soundOnString = "SoundOn";

	#region saveDataInfo 

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

		return PlayerPrefs.GetString (lastTimeGiftPickedString, "");
	}

	public void setLastTimeGift(string time){

		PlayerPrefs.SetString (lastTimeGiftPickedString, time);
	}


	public int getLastSelectedCharacter(){

		return PlayerPrefs.GetInt (lastCharacterSelectedString, 1);
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

	#endregion

	string numberOfPlays = "NumberOfPlays",
			totalDonutsPicked = "TotalDonutsPicked",
			totalJumps = "TotalJumps",
			totalHeight = "TotalHeight",
			deathByPencil = "DeathByPencil",
			deathByShuriken = "DeathByShuriken",
			totalPaintButtonPressed = "TotalPaintButtonPressed",
			totalFlyingAnimalsScared = "TotalFlyingAnimalsScared",
			totalPencilExplosion = "TotalPencilExplosion",
			deathBySpine = "DeathBySpine";

	#region InfoForStats

	public int getPlaysNumber(){
		return PlayerPrefs.GetInt (numberOfPlays, 0);
	}

	public void increasePlayNumber(){
		PlayerPrefs.SetInt (numberOfPlays,getPlaysNumber() + 1);
	}

	public int getDeathBySpine(){
		return PlayerPrefs.GetInt(deathBySpine, 0);
	}

	public void increaseDeathBySpine(){
		PlayerPrefs.SetInt (deathBySpine, getDeathBySpine() + 1);
	}

	public void increaseTotalFlyingAnimalsScared(int n = 1){
		PlayerPrefs.SetInt (totalFlyingAnimalsScared, getTotalFlyingAnimalsScared() + n);
	}

	public int getTotalFlyingAnimalsScared(){
		return PlayerPrefs.GetInt (totalFlyingAnimalsScared, 0);
	}

	public void increaseTotalPaintButton(int n = 1){
		PlayerPrefs.SetInt (totalPaintButtonPressed, getTotalPaintButton() + n);
	}

	public int getTotalPaintButton(){
		return PlayerPrefs.GetInt (totalPaintButtonPressed, 0);
	}

	public int getTotalDonutsPicked(){
		
		return PlayerPrefs.GetInt (totalDonutsPicked, 0);
	}

	public void increaseTotalDonutsPicked(int donut){
		
		PlayerPrefs.SetInt (totalDonutsPicked, getTotalDonutsPicked() + donut);
	}


	public int getTotalJumps(){

		return PlayerPrefs.GetInt (totalJumps, 0);
	}

	public void increaseTotalJumps(int jump){

		PlayerPrefs.SetInt (totalJumps, getTotalJumps() + jump);
	}


	public int getTotalHeight(){

		return PlayerPrefs.GetInt (totalHeight, 0);
	}

	public void increaseTotalHeight(int height){
		PlayerPrefs.SetInt (totalHeight, getTotalHeight() + height);
	}


	public int getDeathByPencil(){
		
		return PlayerPrefs.GetInt (deathByPencil, 0);
	}

	public void increaseDeathByPencil(){
		
		PlayerPrefs.SetInt (deathByPencil, getDeathByPencil() + 1);
	}


	public int getDeathByShuriken(){

		return PlayerPrefs.GetInt (deathByShuriken, 0);
	}

	public void increaseDeathByShuriken(){

		PlayerPrefs.SetInt (deathByShuriken, getDeathByShuriken() + 1);
	}

	public int getTotalPencilExplosion(){

		return PlayerPrefs.GetInt (totalPencilExplosion, 0);
	}

	public void increaseTotalPencilExplosion(){

		PlayerPrefs.SetInt (totalPencilExplosion, getTotalPencilExplosion () + 1);
	}


	#endregion
}
