using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {

	private float forceX, forceY;
	[SerializeField]
	private Rigidbody2D rb;
	[SerializeField]
	private bool moveLeft, moveRight;

	private void Awake () {
		SetBallSpeed ();
	}

	private void Update () {
		Move ();
	}

	private void OnTriggerEnter2D (Collider2D trig) {
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

	private void Move () {
		if (moveLeft) {
			print ("testLeft");
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
