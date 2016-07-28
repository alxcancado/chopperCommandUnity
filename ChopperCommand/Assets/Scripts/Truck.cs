using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

	
public class Truck : MonoBehaviour {

	private int blinkCount; 

	public void Hit () {
		blinkCount = 0;
		InvokeRepeating ("Blink", 0, 0.1f);
		GameEvents.TruckDestroyed ();
	}

	void Blink () {
		blinkCount++;
		var sr = GetComponent<SpriteRenderer> ();
		var color = sr.color;
		if (color.a == 1)
			color.a = 0.3f;
		else 
			color.a = 1f;

		if (blinkCount > 5) {
			CancelInvoke ();
			color.a = 1f;
			gameObject.SetActive (false);
		}

		sr.color = color;
	}

	public void Reset () {
		gameObject.SetActive (true);
	}
}	
