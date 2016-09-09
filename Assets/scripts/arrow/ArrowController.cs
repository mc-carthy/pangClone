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
		if (trig.tag == "topBrick") {
			gameObject.SetActive (false);
		}
	}

	private void ShootArrow () {
		Vector3 temp = transform.position;
		temp.y += arrowSpeed * Time.unscaledDeltaTime;
		transform.position = temp;
	}
}
