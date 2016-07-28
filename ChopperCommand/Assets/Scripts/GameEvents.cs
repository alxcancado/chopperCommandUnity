using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

	
public class GameEvents {

	public delegate void Event_Enemy_Destroyed ();
	public delegate void Event_Player_Destroyed ();
	public delegate void Event_Truck_Destroyed ();
	public delegate void Event_Start_New_Life ();
	public delegate void Event_Restart_Game ();

	public static event Event_Enemy_Destroyed OnEnemyDestroyed;
	public static event Event_Player_Destroyed OnPlayerDestroyed;
	public static event Event_Truck_Destroyed OnTruckDestroyed;
	public static event Event_Start_New_Life OnStartNewLife;
	public static event Event_Restart_Game OnRestartGame;


	public static void EnemyDestroyed () {
		if (OnEnemyDestroyed != null)
			OnEnemyDestroyed ();
	}

	public static void PlayerDestroyed () {
		if (OnPlayerDestroyed != null)
			OnPlayerDestroyed ();
	}

	public static void TruckDestroyed () {
		if (OnTruckDestroyed != null)
			OnTruckDestroyed ();
	}

	public static void StartNewLife () {
		if (OnStartNewLife != null)
			OnStartNewLife ();
	}

	public static void RestartGame () {
		if (OnRestartGame != null)
			OnRestartGame ();
	}
}
