using UnityEngine;

[System.Serializable]
public class Tile{

	public GameObject tile;

	[Range(0f,1f)]
	public float currentProbability = 0.1f,
				changeProbabilityParameter = 1f,
				minProbability = 0f,
				maxProbability = 0.6f;

	public TileType type;

	public enum TileType {	NoTile = -1,
							NormalTile, 
							CloudTile, 
							UselessTile, 
							MovingTile1,
							MovingTile2,
							MovingTile3,
							AlternatingTile, 
							ColorizeTile,
							BounceTile, 
							SpineTile,
							LiftTile}

	public void changeProbability(){
		
		if(currentProbability * changeProbabilityParameter >= minProbability && currentProbability * changeProbabilityParameter <= maxProbability)
		currentProbability *= changeProbabilityParameter;
	}

	public float getProbability(){

		return currentProbability;
	}
}
