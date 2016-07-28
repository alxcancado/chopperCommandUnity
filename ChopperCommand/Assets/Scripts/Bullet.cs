using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

	
public class Bullet : MonoBehaviour {

	public Player player;
	private SpriteRenderer sp;
	private static Color START_COLOR = new Color32 (34, 34, 34, 255);
	private static Color END_COLOR = new Color32 (51, 102, 204, 180);
	private float progress;
	private float speed = 0.32f;
	private Color color;


	void Start () {
		sp = GetComponent<SpriteRenderer> ();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Enemy") {
			var enemy = other.gameObject.GetComponent<Enemy> ();
			if (enemy != null) {
				gameObject.SetActive (false);
				enemy.Hit ();
			}
		} else if (other.gameObject.tag == "Border") {
			gameObject.SetActive (false);
		}

    }

	void FixedUpdate () {

		var s = transform.localScale;
		var p = transform.localPosition;
	
		color = color + (END_COLOR - START_COLOR) * progress;
		progress += 0.01f; 

		sp.color = color;

		s.x += 2.5f;
		p.x += 0.1f;

		transform.localScale = s;
		transform.localPosition = p;

	}

	public void Fire () {
		transform.localScale = Vector3.one;
		gameObject.SetActive (true);
		color = START_COLOR;
		progress = 0;
		transform.localPosition = new Vector2 ( (0.16f), 0);
	}
}
