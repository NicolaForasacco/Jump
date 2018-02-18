using UnityEngine;

[System.Serializable]
public class Tile{

	public GameObject tile;

	[Range(0f, 1f)]
	public float probability;
	public TileType id;

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
}
