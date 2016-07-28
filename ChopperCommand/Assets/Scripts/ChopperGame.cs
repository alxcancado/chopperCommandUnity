using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

	
public class ChopperGame : MonoBehaviour {

	public static float SCREEN_TO_RADAR_RATIO;

	public GameTile[] screens;
	public RadarTile[] radarScreens;

	public Text scoreTxt;
	public GameObject[] lifeIcons;

	public Player player;
	public GameObject mountain1;
	public GameObject mountain2;
	public Transform controlButton;
	public Transform shootButton;
	public GameObject bulletPoolContainer;

	private float speed = -0.008f;
	private bool shooting;
	private Bullet[] bulletPool;
	private int bulletIndex = 0;

	private float shootButtonSquareRadius = 3.0f;
	private float controlButtonSquareRadius = 3.0f;

	private List<GameTile> scrollingScreens;
	private List<RadarTile> scrollingRadarScreens;
	private float timeOfLastBullet;


	void Start () {

		bulletPool = bulletPoolContainer.GetComponentsInChildren<Bullet> ();
		foreach (var b in bulletPool)
			b.gameObject.SetActive (false);

		Debug.Log (bulletPool.Length);

		scrollingScreens = new List<GameTile> ();
		scrollingRadarScreens = new List<RadarTile> ();
		SCREEN_TO_RADAR_RATIO = RadarTile.TILE_WIDTH / GameTile.TILE_WIDTH;


		DistributeGameScreens ();


		GameEvents.OnStartNewLife += HandleNewLife;
		GameEvents.OnRestartGame += HandleRestart;

	}

	void HandleNewLife () {
		shooting = false;
		DistributeGameScreens ();
		UpdateRadar ();
		player.Reset ();
	}

	void HandleRestart () {
		shooting = false;
		DistributeGameScreens ();
		UpdateRadar ();

		foreach (var s in screens)
			s.Reset ();
		
		
		player.Reset ();
	}


	void Update () {

		ProcessInput ();	
		ScrollScreens ();
		ScrollBackground ();
		UpdateRadar ();
		UpdateBullets ();
	}

	void ProcessInput () {

		player.moveVector = Vector2.zero;
		shooting = false;


		if (!player.alive)
			return; 
		
		if (Input.touches.Length > 0) {
			foreach (var touch in  Input.touches) {
				if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) {
					ProcessTouch (touch.position);
				}
			}
		}
		else  {
			#if UNITY_EDITOR
			ProcessTouch (Input.mousePosition);
			#endif
		}
	}

	void ProcessTouch (Vector2 touch) {


		var touchPosition = Camera.main.ScreenToWorldPoint (touch);

		//track shooting
		if (Mathf.Pow(touchPosition.x - shootButton.position.x, 2) +
			Mathf.Pow(touchPosition.y - shootButton.position.y, 2) <
			shootButtonSquareRadius) {
			shooting = true;
		}

		//track move control
		if (Mathf.Pow(touchPosition.x - controlButton.position.x, 2) +
			Mathf.Pow(touchPosition.y - controlButton.position.y, 2) <
			controlButtonSquareRadius) {

			var vector = new Vector2 (touchPosition.x - controlButton.position.x,
				             touchPosition.y - controlButton.position.y);

			vector.Normalize ();
			player.moveVector = vector;
		}
	}

	void ScrollScreens () {
		var i = 0;
		while (i < screens.Length) {

			var screen = scrollingScreens[i];
			var rScreen = scrollingRadarScreens [i];
			var p = screen.transform.position;
			var totalspeed = speed + player.screenScroll * 0.5f;

			if (totalspeed < 0) {
				
				if (screen.transform.position.x + totalspeed < -3 * GameTile.TILE_WIDTH) {
					
					p.x = scrollingScreens[scrollingScreens.Count - 1].transform.position.x + GameTile.TILE_WIDTH;

					scrollingScreens.RemoveAt (i);
					scrollingScreens.Add (screen);

					scrollingRadarScreens.RemoveAt (i);
					scrollingRadarScreens.Add (rScreen);

					screen.gameObject.SetActive (true);
				} else {
					p.x  +=  totalspeed;

				}
				screen.transform.position = p;

			} else if (totalspeed > 0) {
				
				if (screen.transform.position.x + totalspeed > 3 * GameTile.TILE_WIDTH) {
					p.x = scrollingScreens[0].transform.position.x - GameTile.TILE_WIDTH;
					scrollingScreens.RemoveAt (i);
					scrollingScreens.Insert (0, screen);
					scrollingRadarScreens.RemoveAt (i);
					scrollingRadarScreens.Insert (0, rScreen);
					screen.gameObject.SetActive (true);
				} else {
					p.x  +=  totalspeed;
				}
				screen.transform.position = p;
			}

			p = rScreen.transform.position;
			p.x = -screen.transform.position.x * -SCREEN_TO_RADAR_RATIO;
			rScreen.transform.position = p;

			i++;
		}

	}

	void ScrollBackground () {
		
		var p = mountain2.transform.position;
		p.x += player.screenScroll * 0.5f;
		mountain2.transform.position = p;

		p = mountain1.transform.position;
		p.x += player.screenScroll * 2;
		mountain1.transform.position = p;

		if (player.screenScroll < 0) {
			if (mountain2.transform.position.x < -GameTile.TILE_WIDTH) {
				p = mountain2.transform.position;
				p.x += GameTile.TILE_WIDTH - (p.x + GameTile.TILE_WIDTH);
				mountain2.transform.position = p;
			}

			if (mountain1.transform.position.x < -GameTile.TILE_WIDTH) {
				p = mountain1.transform.position;
				p.x += GameTile.TILE_WIDTH - (p.x + GameTile.TILE_WIDTH);
				mountain1.transform.position = p;
			}

		} else if (player.screenScroll > 0) {
			if (mountain2.transform.position.x > -1) {
				p = mountain2.transform.position;
				p.x -= GameTile.TILE_WIDTH + (p.x + 1);
				mountain2.transform.position = p;
			}

			if (mountain1.transform.position.x > -1) {
				p = mountain1.transform.position;
				p.x -= GameTile.TILE_WIDTH + (p.x +  1);
				mountain1.transform.position = p;
			}
		}
	}

	void UpdateRadar () {

		var i = 0;
		while (i < screens.Length) {
			scrollingRadarScreens [i].UpdateRadarSprites (scrollingScreens [i].selectedEnemies, scrollingScreens [i].trucks);
			i++;
		}

		//update player in radar
		var radarPlayerPosition = player.radarPlayer.transform.localPosition;
		radarPlayerPosition.x = -(player.transform.position.x) * -SCREEN_TO_RADAR_RATIO;

		var playerActualY = Mathf.Abs(player.transform.position.y - player.minY); 
		radarPlayerPosition.y = -RadarTile.TILE_HEIGHT * 0.5f + playerActualY * SCREEN_TO_RADAR_RATIO;
		player.radarPlayer.transform.localPosition = radarPlayerPosition;
	}


	void UpdateBullets () {
		if (shooting && player.alive) {
			if (Time.time - timeOfLastBullet > 0.4f) {
				timeOfLastBullet = Time.time;

				var bullet = bulletPool [bulletIndex];
				bullet.Fire ();
				bulletIndex++;
				if (bulletIndex == bulletPool.Length) {
					bulletIndex = 0;
				}
			}
		}
	}

	void DistributeGameScreens () {
		
		var i = 0;
		while (i < screens.Length) {
			var radarScreen = radarScreens [i];
			var p = radarScreen.transform.position;
			p.x = - screens.Length * RadarTile.TILE_WIDTH * 0.5f + i * RadarTile.TILE_WIDTH;
			radarScreen.transform.position = p;
			scrollingRadarScreens.Add (radarScreen);

			var screen = screens [i];
			p = screen.transform.position;
			p.x = - screens.Length * GameTile.TILE_WIDTH * 0.5f + i * GameTile.TILE_WIDTH;
			screen.transform.position = p;
			scrollingScreens.Add (screen);
			i++;
		}

	}
}
