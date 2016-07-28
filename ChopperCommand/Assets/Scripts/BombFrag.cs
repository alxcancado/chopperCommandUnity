using UnityEngine;
using System.Collections;

public class BombFrag : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {

		if (other.gameObject.tag == "Player") {
			var player = other.gameObject.GetComponent <Player> ();
			player.Hit ();
			gameObject.SetActive (false);
		} else if (other.gameObject.tag == "Truck") {
			var truck = other.gameObject.GetComponent<Truck> ();
			truck.Hit ();
			gameObject.SetActive (false);

		} else if (other.gameObject.tag == "Border") {
			gameObject.SetActive (false);
		}
	}
}
