using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

	
public class RadarTile : MonoBehaviour {

	public static float TILE_WIDTH = 1.2f;
	public static float TILE_HEIGHT = 0.64f;

	public GameObject[] trucks;
	public GameObject[] enemies;

	public void UpdateRadarSprites (Enemy[] gameEnemies, Truck[] gameTrucks) {
		var i = 0;

		while (i < enemies.Length) {

			if (i >= gameEnemies.Length) {
				enemies [i].SetActive (false);
			} else {
				MatchSprites (enemies [i], gameEnemies [i].gameObject);
			}
			if (i >= gameTrucks.Length) {
				trucks [i].SetActive (false);
			} else {
				MatchSprites (trucks [i], gameTrucks [i].gameObject);
			}
			
			i++;
		}
	}

	void MatchSprites (GameObject radarGO, GameObject screenGO) {

		if (!screenGO.activeSelf ) {
			radarGO.SetActive (false);
			return;
		}

		if (!radarGO.activeSelf) radarGO.SetActive (true);
		if (screenGO.tag != "Truck") {

			var p = radarGO.transform.localPosition;

			p.x = -screenGO.transform.position.x * -ChopperGame.SCREEN_TO_RADAR_RATIO;
			radarGO.transform.localPosition = p;
		}

	}

}


