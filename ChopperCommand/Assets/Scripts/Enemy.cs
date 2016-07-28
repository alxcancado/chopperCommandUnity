using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

	
public class Enemy : MonoBehaviour {

	public GameObject boomPrefab;
	public Bomb bombPrefab;
	public Transform screenTileTransform;

	public float maxY;
	public float minY;
	public float maxX;
	public float minX;

	[HideInInspector]
	public int type;

	private Vector2 nextPosition;
	private Vector2 targetPosition;
	private int timerBetweenBombs;
	private int timerBetweenShifts;
	private int intervalBetweenBombs;
	private int intervalBetweenShifts;

	private float maxWidth;
	private float maxHeight;
	private float enemyspeed = 0.02f;
	private bool alive;
	private Bomb bomb;


	void Start () {
	
		targetPosition = Vector2.zero;
		nextPosition = Vector2.zero;

		maxWidth = maxX - minX;
		maxHeight = maxY - minY;

		intervalBetweenBombs = Random.Range (50, 200);
		intervalBetweenShifts = Random.Range (100, 200);
		alive = true;
	}


	void FixedUpdate () {

		if (!alive)
			return;
		
		var random = Random.Range (0.0f, 1.0f);

		timerBetweenShifts++;
		if (timerBetweenShifts > intervalBetweenShifts)
		{
			timerBetweenShifts = 0;
			intervalBetweenShifts = Random.Range (100, 200);
			PickMoveToPosition ();
		}

		if (type == 0)
		{
			if (random < 0.02f)
			{
				targetPosition.y = minY + Random.Range (minY, maxY);
			}
		}
		else
		{
			if (random < 0.01f)
			{
				targetPosition.y = minY + Random.Range (minY, maxY);
			}
		}

		if (nextPosition.x + enemyspeed < targetPosition.x)
		{
			nextPosition.x +=  enemyspeed;
		}

		if (nextPosition.x - enemyspeed > targetPosition.x)
		{
			nextPosition.x -=  enemyspeed;
		}

		if (nextPosition.y + enemyspeed < targetPosition.y)
		{
			nextPosition.y +=  enemyspeed;
		}

		if (nextPosition.y - enemyspeed > targetPosition.y)
		{
			nextPosition.y -=  enemyspeed;
		}

		if (nextPosition.y > maxY)
		{
			nextPosition.y = maxY;
		}
		if (nextPosition.y < minY)
		{
			nextPosition.y = minY;
		}
		if (nextPosition.x < minX)
		{
			nextPosition.x = minX;
		}
		if (nextPosition.x > maxX)
		{
			nextPosition.x = maxX;
		}

		if (Vector2.Distance (nextPosition, targetPosition) < 0.05f) {
			PickMoveToPosition ();
		}

		transform.localPosition = nextPosition;

		if (bomb == null)
		{
			timerBetweenBombs++;
			if (timerBetweenBombs > intervalBetweenBombs)
			{
				intervalBetweenBombs = Random.Range (50, 100);
				bomb = Instantiate (bombPrefab) as Bomb;

				bomb.transform.position = transform.position;
				bomb.Drop ( (int)transform.localScale.x);
				timerBetweenBombs = 0;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player" && alive) {
			var player = other.gameObject.GetComponent<Player> ();
			player.Hit ();
			Hit ();
		}
    }

	void PickMoveToPosition () {

		var p = transform.localPosition;

		if (p.x < minX + maxWidth * 0.5f)
		{
			targetPosition.x = minX + maxX;
		}
		else
		{
			targetPosition.x = minX;
		}

		targetPosition.y = minY + Random.Range(0.0f, 1.0f) * maxHeight;

		var s = transform.localScale;

		if (targetPosition.x < nextPosition.x)
		{
			s.x = -1;
		}
		else if (targetPosition.x > nextPosition.x)
		{
			s.x = 1;
		}
		transform.localScale = s;
	}

	public void Hit () {

		if (!alive)
			return;
		
		alive = false;

		GameEvents.EnemyDestroyed ();

		gameObject.SetActive (false);

		var boom = Instantiate (boomPrefab) as GameObject;
		boom.transform.position = gameObject.transform.position;
		boom.transform.SetParent (transform.parent, true);
		Destroy (boom, 1.5f);


	}


	public void Reset () {

		if (bomb != null)
			Destroy (bomb.gameObject);
	
		transform.localPosition = new Vector2 (
			Random.Range(minX, maxX),
			Random.Range(minY, maxY)
		);

		if (Random.Range (0, 10) > 5) {
			var s = transform.localScale;
			s.x *= -1;
			transform.localScale = s;
		}

		intervalBetweenBombs = 100;
		intervalBetweenShifts = 50;
		timerBetweenBombs = 0;
		timerBetweenShifts = 0;

		alive = true;
	}
}
