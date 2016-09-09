using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {

	private float forceX, forceY;
	[SerializeField]
	private Rigidbody2D rb;
	[SerializeField]
	private bool moveLeft, moveRight;
	[SerializeField]
	private GameObject originalBall;
	[SerializeField]
	private AudioClip[] popSounds;
	private GameObject ball0, ball1;
	private BallController ball0Script, ball1Script;

	private void Awake () {
		SetBallSpeed ();
		InstantiateBalls ();
	}

	private void Update () {
		Move ();
	}

	private void OnTriggerEnter2D (Collider2D trig) {
		if (trig.tag == "firstArrow" || trig.tag == "secondArrow" || trig.tag == "firstStickyArrow" || trig.tag == "secondStickyArrow") {
			if (gameObject.tag != "smallestBall") {
				InitializeBallsAndTurnOffCurrent ();
			} else {
				gameObject.SetActive (false);
			}
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
	}

	public void SetMoveLeft (bool moveLeft) {
		this.moveLeft = moveLeft;
		this.moveRight = !moveLeft;
	}

	public void SetMoveRight (bool moveRight) {
		this.moveLeft = !moveRight;
		this.moveRight = moveRight;
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

		AudioSource.PlayClipAtPoint(popSounds[Random.Range(0, popSounds.Length)], transform.position);
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
}
