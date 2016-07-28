using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

	
public class Bomb : MonoBehaviour {

	public GameObject bomb1;
	public GameObject bomb2;

	public float speed = 0.03f;

	private int intervalForBombBreak;
	private int timeForBombBreak;
	private Vector2 vector;

	void Awake () {
		vector = Vector2.zero;		
	}

	void FixedUpdate () {

		if (timeForBombBreak >= 0) timeForBombBreak++;

		if ( timeForBombBreak > intervalForBombBreak ) {
			timeForBombBreak = -1;
			bomb2.SetActive (true);
			vector.x = 0;
			vector.y = speed * 1.4f;
 		} else if (timeForBombBreak > 0) {
			bomb2.transform.localPosition = bomb1.transform.localPosition;
		}

		bomb1.transform.Translate (vector);

		if (bomb2.activeSelf) {
			var p = bomb2.transform.localPosition;
			p.x = bomb1.transform.localPosition.x;
			p.y += -1 * vector.y;
			bomb2.transform.localPosition = p;
		}


		if (!bomb1.activeSelf && !bomb2.activeSelf) {
			Destroy (gameObject);
		}
	}

	public void Drop (int direction) {

		bomb1.SetActive (true);
		bomb2.SetActive (false);

		intervalForBombBreak =  Random.Range (15, 60);
		timeForBombBreak = 0;


		vector = Vector2.zero;
		vector.y = -speed;

		if (Random.Range(0,10) > 5) {
			var targetPosition = Vector2.zero;
			targetPosition.x = transform.localPosition.x + direction * Random.Range (0.2f, 2f);
			targetPosition.y = transform.localPosition.y + Random.Range (0.2f, 2f);

			var angle = Mathf.Atan2 (targetPosition.y - transform.localPosition.y, targetPosition.x - transform.localPosition.x);
			vector.x = Mathf.Cos(angle) * speed;
			vector.y = Mathf.Sin(angle) * speed;
		}
	}

   
}
