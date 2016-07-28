using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

	
public class Player : MonoBehaviour {

	public GameObject radarPlayer;

	public float minX;
	public float maxX;
	public float minY;
	public float maxY;
	public Sprite boomSprite;
	public Sprite playerSprite;
	public bool alive;


	[HideInInspector]
	public Vector2 moveVector;
	[HideInInspector]
	public float screenScroll = 0;

	private Vector2 vector;
	private Vector2 nextPosition;
	private Vector2 acceleration = new Vector2(0.005f, 0.02f);
	private float range;
	private float maxSpeed = 0.04f;
	private float transition;
	private float screenScrollNow;
	private float screenScrollTarget;


	void Awake () {
		moveVector = Vector2.zero;
		vector = Vector2.zero;

	}

	void Start () {
		range = (maxX - minX) * 0.35f;
		alive = true;
	}

	void FixedUpdate () {

		if (!alive)
			return;
		
		nextPosition = transform.position;

		vector.y = 0;

		if (moveVector.y < 0) {
			vector.y = - acceleration.y;
		} else if (moveVector.y > 0) {
			vector.y =  acceleration.y;
		}

		nextPosition.y += vector.y;
		if (nextPosition.y < minY) {
			nextPosition.y = minY;
		}

		if (nextPosition.y > maxY) {
			nextPosition.y = maxY;
		}

		//handle flip
		var scale = transform.localScale;

		if (scale.x > 0 && moveVector.x < 0) {
			scale.x = -1;
		}

		if (scale.x < 0 && moveVector.x > 0) {
			scale.x = 1;
		}

		transform.localScale = scale;

		//if player is inside update region
		if ((scale.x > 0 && transform.position.x <= minX + range) || (scale.x < 0 && transform.position.x >= maxX - range )) {

			if (moveVector.x != 0) {
				vector.x +=  acceleration.x;
			} else {
				vector.x =  - acceleration.x * 16;
			}

			if (scale.x > 0) {
				screenScrollTarget = maxSpeed * ((transform.position.x - minX)/(minX + range));
			}

			if (scale.x < 0) {
				screenScrollTarget = -maxSpeed * ((maxX - transform.position.x )/(minX + range));
			}
		} else {
			//player is flipping
			vector.x =  - acceleration.x * 16;
			if (screenScrollTarget != vector.x * scale.x) {
				transition = 0;
				screenScrollTarget = vector.x * scale.x;
			}
		}

		if (vector.x > maxSpeed) {
			vector.x = maxSpeed;
		}

		if (vector.x <  - maxSpeed) {
			vector.x =  - maxSpeed;
		}

		nextPosition.x +=  vector.x * scale.x;

		if (scale.x > 0 && transform.position.x <= minX + range) {
			if (nextPosition.x >= minX + range) {
				nextPosition.x = minX + range;
			}
		}

		if (scale.x < 0 && transform.position.x >= maxX - range) {
			if (nextPosition.x <= maxX - range) {
				nextPosition.x = maxX - range;
			}
		}

		if (nextPosition.x < minX) {
			nextPosition.x = minX;
		}

		if (nextPosition.x > maxX) {
			nextPosition.x = maxX;
		}

		transform.position = nextPosition;


		UpdateScreenSpeed ();
	}


	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Truck" && alive) {
			var truck = other.gameObject.GetComponent<Truck> ();
			truck.Hit ();
			Hit ();
		}
	}

	public void Hit () {
		#if !UNITY_EDITOR
		if (!alive)
			return;
		
		alive = false;

		moveVector = Vector2.zero;
		screenScroll = 0;

		var anim = GetComponent<Animator> ();
		anim.enabled = false;
		var sp = GetComponent<SpriteRenderer> ();
		sp.sprite = boomSprite;

		GameEvents.PlayerDestroyed ();
		#endif
	}
    

	void UpdateScreenSpeed () {
		if (screenScrollNow != screenScrollTarget) {
			screenScrollNow = screenScrollNow + (screenScrollTarget - screenScrollNow) * transition;
			transition +=  0.01f;
			screenScroll = screenScrollNow;
		}

		if (transition > 1) {
			transition = 0;
		}
	}

	public void Reset () {

		var anim = GetComponent<Animator> ();
		anim.enabled = true;
		var sp = GetComponent<SpriteRenderer> ();
		sp.sprite = playerSprite;

		var scale = transform.localScale;
		scale.x = 1;
		transform.localScale = scale;
		transform.position = new Vector2 (-0.2f, 0);

		nextPosition = transform.position;

		vector = Vector2.zero;
		moveVector = Vector2.zero;

		screenScrollNow = 0;
		screenScrollTarget = 0;
		transition = 0;
		screenScroll = 0;

		alive = true;
	}
}