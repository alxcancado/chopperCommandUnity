using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

	
public class GameTile : MonoBehaviour {
	
	public static float TILE_WIDTH = 9.6f;
	public static float TILE_HEIGHT = 5.36f;

	public Truck[] trucks;
	public Enemy[] enemies;

	public Enemy[] selectedEnemies;

	[HideInInspector]
	public RadarTile radarTile;

	private List<Bomb> bombs;

	void Start () {
		AddEnemies ();
	}

	public void Reset () {

		foreach (var e in enemies) {
			e.Reset ();
			e.gameObject.SetActive (false);
		}

		foreach (var t in trucks) t.Reset();

		AddEnemies ();
	}

	void AddEnemies () {

		var i = 0;
		selectedEnemies = new Enemy[3];

		while (i < 3) {
			var jet = Random.Range (0, 10) > 5;
			Enemy enemy = null;
			if (jet) {
				enemy = enemies [3 + i];
				enemy.type = 1;
			} else {
				enemy = enemies [i];
				enemy.type = 0;
			}
			enemy.Reset ();
			enemy.gameObject.SetActive (true);
			selectedEnemies[i] = enemy;
			i++;
		}
	}
}


