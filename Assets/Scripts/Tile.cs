using UnityEngine;

[System.Serializable]
public class Tile{

	public GameObject tile;

	[Range(0f,1f)]
	public float currentProbability,
				changeProbabilityParameter,
				minProbability,
				maxProbability;

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
							SpineTile}

	public void changeProbability(){

		if (currentProbability == 0 && changeProbabilityParameter >= 1) {
			currentProbability = minProbability;
		}

		if (changeProbabilityParameter > 1) {
			
			if (currentProbability * changeProbabilityParameter <= maxProbability)
				currentProbability *= changeProbabilityParameter;
			else
				currentProbability = maxProbability;
			

		} else if(changeProbabilityParameter < 1){
			
			if (currentProbability * changeProbabilityParameter >= minProbability)
				currentProbability *= changeProbabilityParameter;
			else
				currentProbability = minProbability;
		}
			
		/*if(currentProbability * changeProbabilityParameter >= minProbability && currentProbability * changeProbabilityParameter <= maxProbability)
			currentProbability *= changeProbabilityParameter;
		else if (currentProbability == 0 && changeProbabilityParameter > 1)
			currentProbability += minProbability;*/
	}

	public float getProbability(){

		return currentProbability;
	}
}
