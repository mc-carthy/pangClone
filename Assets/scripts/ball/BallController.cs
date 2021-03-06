﻿using UnityEngine;
using System.Collections;
//[ExecuteInEditMode]

public class BallController : MonoBehaviour {

	public float x, y;

	private float forceX, forceY;
	[SerializeField]
	private Rigidbody2D rb;
	[SerializeField]
	private bool moveLeft, moveRight;
	[SerializeField]
	private GameObject originalBall;
	[SerializeField]
	private AudioClip[] popSounds;
	[SerializeField]
	private GameObject[] collectableItems;
	private GameObject ball0, ball1;
	private BallController ball0Script, ball1Script;

	private void Awake () {
		
		if (this.gameObject.tag == "smallestBall") {
			GameplayController.smallBallsCount++;
		}

		SetBallSpeed ();
		InstantiateBalls ();

		int chance = Random.Range (0, 1);
		if (chance == 0) {
			SetMoveLeft (true);
		} else { 
			SetMoveRight (true);
		}
	}

	private void Start () {
		if (!GameplayController.instance.isLevelInProgress) {
			transform.position = Camera.main.ViewportToWorldPoint (new Vector3 (x, y, 5));
		}
	}

	private void Update () {
		//transform.position = Camera.main.ViewportToWorldPoint (new Vector3 (x, y, 5));
		Move ();
	}

	private void OnTriggerEnter2D (Collider2D trig) {
		if (trig.tag == "firstArrow" || trig.tag == "secondArrow" || trig.tag == "firstStickyArrow" || trig.tag == "secondStickyArrow") {
			if (gameObject.tag != "smallestBall") {
				InitializeBallsAndTurnOffCurrent ();
			} else {
				GameplayController.instance.CountSmallBalls ();
				gameObject.SetActive (false);
			}
		}

		if (
			trig.tag == "unbreakableBrickTop" ||
			trig.tag == "brokenBrickTop" ||
			trig.tag == "unbreakableBrickTopVertical"
		) {
			rb.velocity = new Vector2 (0, 5);
		} else if (
			trig.tag == "unbreakableBrickBottom" ||
			trig.tag == "brokenBrickBottom" ||
			trig.tag == "unbreakableBrickBottomVertical"
		) {
			rb.velocity = new Vector2 (0, -2);
		} else if (
			trig.tag == "unbreakableBrickLeft" ||
			trig.tag == "brokenBrickLeft" ||
			trig.tag == "unbreakableBrickLeftVertical"
		) {
			moveLeft = true;
			moveRight = false;
		} else if (
			trig.tag == "unbreakableBrickRight" ||
			trig.tag == "brokenBrickRight" ||
			trig.tag == "unbreakableBrickRightVertical"
		) {
			moveLeft = false;
			moveRight = true;	
		}

		if (trig.tag == "bottomBrick") {
			rb.velocity = new Vector2 (0, forceY);
		}
		if (trig.tag == "leftBrick") {
			moveLeft = false;
			moveRight = true;
		}
		if (trig.tag == "rightBrick") {
			moveLeft = true;
			moveRight = false;;		
		}

		if (trig.tag == "Player") {
			if (PlayerController.instance.hasShield) {
				PlayerController.instance.DestroyShield ();
			} else {
				if (!PlayerController.instance.isInvincible) {
					Destroy (trig.gameObject);
					GameplayController.instance.PlayerDied ();
				}
			}
		}
	}

	private void OnEnable () {
		PlayerController.explode += Explode;
	}

	private void OnDisable () {
		PlayerController.explode -= Explode;
	}

	public void SetMoveLeft (bool moveLeft) {
		this.moveLeft = moveLeft;
		this.moveRight = !moveLeft;
	}

	public void SetMoveRight (bool moveRight) {
		this.moveLeft = !moveRight;
		this.moveRight = moveRight;
	}

	public void Explode (bool touchedGoldBall) {
		StartCoroutine(ExplodeBall(touchedGoldBall));
	}

	private void InstantiateBalls () {
		if (gameObject.tag != "smallestBall") {
			ball0 = Instantiate (originalBall);
			ball1 = Instantiate (originalBall);

			ball0Script = ball0.GetComponent<BallController> ();
			ball1Script = ball1.GetComponent<BallController> ();

			ball0.SetActive (false);
			ball1.SetActive (false);
		}
	}

	private void InitializeBallsAndTurnOffCurrent () {
		Vector3 pos = transform.position;
		ball0.transform.position = ball1.transform.position = pos;
		ball0Script.SetMoveLeft (true);
		ball1Script.SetMoveRight (true);
		ball0.SetActive (true);
		ball1.SetActive (true);

		if (gameObject.tag != "smallestBall") {
			if (transform.position.y > 1.3f) {
				ball0.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 3.5f);
				ball1.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 3.5f);
			} else if (transform.position.y > 1 && transform.position.y <= 1.3f) {
				ball0.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 2);
				ball1.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 2);
			} else {
				ball0.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 5.5f);
				ball0.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 5.5f);
			}
		}

		AudioSource.PlayClipAtPoint(popSounds[Random.Range(0, popSounds.Length)], transform.position);
		InitializeCollectableItems (transform.position);
		GiveScoreAndCoins (this.gameObject.tag);
		gameObject.SetActive (false);
	}

	private void Move () {
		if (moveLeft) {
			Vector3 temp = transform.position;
			temp.x -= forceX * Time.deltaTime;
			transform.position = temp;
		}
		if (moveRight) {
			Vector3 temp = transform.position;
			temp.x += forceX * Time.deltaTime;
			transform.position = temp;
		}
	}

	private void SetBallSpeed () {
		forceX = 2.5f;
		switch (this.gameObject.tag) {
		case "largestBall":
			forceY = 11.5f;
			break;
		case "largeBall":
			forceY = 10.5f;
			break;
		case "mediumBall":
			forceY = 9f;
			break;
		case "smallBall":
			forceY = 8f;
			break;
		case "smallestBall":
			forceY = 7f;
			break;
		default:
			forceY = 0f;
			break;
		}
	}

	private void InitializeCollectableItems (Vector3 position) {

		if (this.gameObject.tag == "smallestBall") {
			int chance = Random.Range (0, 60);

			if (chance >= 0 && chance < 21) {
				Instantiate(collectableItems[Random.Range(4, collectableItems.Length)], position, Quaternion.identity);
			} else if (chance >= 21 && chance < 36) {
				Instantiate(collectableItems[Random.Range(0, 4)], position, Quaternion.identity);
			}
		}
	}

	private void GiveScoreAndCoins (string objTag) {
		switch (objTag) {
		case "largestBall":
			GameplayController.instance.coins += Random.Range (15, 20);
			GameplayController.instance.playerScore += Random.Range (600, 700);
			break;
		case "largeBall":
			GameplayController.instance.coins += Random.Range (13, 18);
			GameplayController.instance.playerScore += Random.Range (500, 600);
			break;
		case "mediumBall":
			GameplayController.instance.coins += Random.Range (11, 16);
			GameplayController.instance.playerScore += Random.Range (400, 500);
			break;
		case "smallBall":
			GameplayController.instance.coins += Random.Range (10, 15);
			GameplayController.instance.playerScore += Random.Range (300, 400);
			break;
		case "smallestBall":
			GameplayController.instance.coins += Random.Range (9, 14);
			GameplayController.instance.playerScore += Random.Range (200, 300);
			break;
		}
	}

	private IEnumerator ExplodeBall (bool touchedGoldBall) {
		if (this.gameObject.tag == "largestBall") {
			yield return null;
		} else {
			yield return StartCoroutine (MyCoroutine.WaitForRealSeconds (0.5f));
		}

		Vector3 pos = transform.position;

		ball0.transform.position = pos;
		ball1.transform.position = pos;

		ball0Script.SetMoveLeft (true);
		ball1Script.SetMoveRight (true);

		ball0.SetActive (true);
		ball1.SetActive (true);

		if (gameObject.tag != "smallestBall") {
			if (transform.position.y > 1.3f) {
				ball0.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 3.5f);
				ball1.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 3.5f);
			} else if (transform.position.y > 1 && transform.position.y <= 1.3f) {
				ball0.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 2);
				ball1.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 2);
			} else {
				ball0.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 5.5f);
				ball0.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 5.5f);
			}
		}

		if (touchedGoldBall) {
			if (this.gameObject.tag != "smallestBall") {
				ball0Script.Explode (true);
				ball1Script.Explode (true);
			} else {
				GameplayController.instance.CountSmallBalls ();
			}
			this.gameObject.SetActive (false);
		} else {
			if (this.gameObject.tag != "smallestBall") {
				ball0Script.Explode (true);
				ball1Script.Explode (true);
				this.gameObject.SetActive (false);
			}
		}
	}
}
