using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {

	public Text scoreLabel;
	public GameObject[] livesIcon;

	public int lives;
	public int score;

	void Start () {
		GameEvents.OnEnemyDestroyed += HandleEnemyDestroyed;
		GameEvents.OnPlayerDestroyed += HandlePlayerDestroyed;
		GameEvents.OnRestartGame += HandleRestart;
	}

	void HandleRestart () {
		lives = 3;
		score = 0;
		scoreLabel.text = "0";
		foreach (var i in livesIcon)
			i.SetActive (true);
	}

	void HandlePlayerDestroyed () {
		
		lives--;
		if (lives < 0) {
			//RESTART?
			Invoke ("Restart", 5);
		} else {
			var i = 0;
			while (i < livesIcon.Length) {
				if (i >= lives)
					livesIcon [i].SetActive (false);
				i++;
			}
			Invoke ("NewLife", 3);
		}
	}


	void HandleEnemyDestroyed () {
		score += 100;
		scoreLabel.text = "" + score;
	}

	void Restart () {
		CancelInvoke ();
		GameEvents.RestartGame ();
	}

	void NewLife () {
		CancelInvoke ();
		GameEvents.StartNewLife ();
	}
}
