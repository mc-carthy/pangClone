using UnityEngine;
using System.Collections;

public class ArrowController : MonoBehaviour {

	private float arrowSpeed = 7.0f;
	private bool canShootStickyArrow;

	private void Start () {
		canShootStickyArrow = true;
	}

	private void Update () {
		ShootArrow ();
	}

	private void OnTriggerEnter2D (Collider2D trig) {
		if (trig.tag == "largestBall" || trig.tag == "largeBall" || trig.tag == "mediumBall" || trig.tag == "smallBall" || trig.tag == "smallestBall") {
			DeactivateArrow ();
		}

		if (trig.tag == "topBrick") {
			DeactivateArrow ();
		}
	}

	private void ShootArrow () {
		Vector3 temp = transform.position;
		temp.y += arrowSpeed * Time.unscaledDeltaTime;
		transform.position = temp;
	}

	private void DeactivateArrow () {
		if (gameObject.tag == "firstArrow" || gameObject.tag == "firstStickyArrow") {
			PlayerController.instance.ShootOnce (true);
		} else if (gameObject.tag == "secondArrow" || gameObject.tag == "secondStickyArrow") {
			PlayerController.instance.ShootTwice (true);
		}
		gameObject.SetActive (false);
	}
}
