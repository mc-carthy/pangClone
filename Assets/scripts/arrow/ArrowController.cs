using UnityEngine;
using System.Collections;

public class ArrowController : MonoBehaviour {

	private float arrowSpeed = 7.0f;
	private bool canShootStickyArrow;
	[SerializeField]
	private AudioClip clip;

	private void Start () {
		canShootStickyArrow = true;
	}

	private void Update () {
		if (this.gameObject.tag == "firstStickyArrow") {
			if (canShootStickyArrow) {
				ShootArrow ();
			}
		} else if (this.gameObject.tag == "secondStickyArrow") {
			if (canShootStickyArrow) {
				ShootArrow ();
			}
		} else {
			ShootArrow ();
		}
	}

	private void OnTriggerEnter2D (Collider2D trig) {
		if (trig.tag == "largestBall" || trig.tag == "largeBall" || trig.tag == "mediumBall" || trig.tag == "smallBall" || trig.tag == "smallestBall") {
			DeactivateArrow ();
		}
		/*
		if (trig.tag == "topBrick") {
			if (this.gameObject.tag == "firstArrow") {
				PlayerController.instance.ShootOnce (true);
				this.gameObject.SetActive (false);
			} else if (this.gameObject.tag == "secondArrow") {
				PlayerController.instance.ShootTwice (true);
				this.gameObject.SetActive (false);
			} else if (this.gameObject.tag == "firstStickyArrow") {
				canShootStickyArrow = false;
				Vector3 targetPos = trig.transform.position;
				Vector3 temp = transform.position;
				targetPos.y -= 0.99f;
				temp.y = targetPos.y;
				transform.position = temp;
				AudioSource.PlayClipAtPoint (clip, transform.position);
				StartCoroutine ("ResetStickyArrow");
			} else if (this.gameObject.tag == "secondStickyArrow") {
				canShootStickyArrow = false;
				Vector3 targetPos = trig.transform.position;
				Vector3 temp = transform.position;
				targetPos.y -= 0.99f;
				temp.y = targetPos.y;
				transform.position = temp;
				AudioSource.PlayClipAtPoint (clip, transform.position);
				StartCoroutine ("ResetStickyArrow");
			}
		}
		*/
		if (
			trig.tag == "topBrick" || trig.tag == "unbreakableBrickTop" ||
			trig.tag == "unbreakableBrickBottom" || trig.tag == "unbreakableBrickLeft" ||
			trig.tag == "unbreakableBrickRight" || trig.tag == "unbreakableBrickBottomVertical"
		) {
			if (this.gameObject.tag == "firstArrow") {
			PlayerController.instance.ShootOnce (true);
			this.gameObject.SetActive (false);

			} else if (this.gameObject.tag == "secondArrow") {
				PlayerController.instance.ShootTwice (true);
				this.gameObject.SetActive (false);

			} else if (this.gameObject.tag == "firstStickyArrow") {
				canShootStickyArrow = false;
				Vector3 targetPos = trig.transform.position;
				Vector3 temp = transform.position;

				if (trig.tag == "topBrick") {
					targetPos.y -= 0.99f;
				} else if (
					trig.tag == "unbreakableBrickTop" ||
					trig.tag == "unbreakableBrickBottom" || 
					trig.tag == "unbreakableBrickLeft" ||
					trig.tag == "unbreakableBrickRight"
				) {
					targetPos.y -= 0.9f;
				} else if (trig.tag == "unbreakableBrickBottomVertical") {
					targetPos.y -= 1.65f;
				}
				temp.y = targetPos.y;
				transform.position = temp;
				AudioSource.PlayClipAtPoint (clip, transform.position);
				StartCoroutine ("ResetStickyArrow");

			} else if (this.gameObject.tag == "secondStickyArrow") {
				canShootStickyArrow = false;
				Vector3 targetPos = trig.transform.position;
				Vector3 temp = transform.position;

				if (trig.tag == "topBrick") {
					targetPos.y -= 0.99f;
				} else if (
					trig.tag == "unbreakableBrickTop" ||
					trig.tag == "unbreakableBrickBottom" || 
					trig.tag == "unbreakableBrickLeft" ||
					trig.tag == "unbreakableBrickRight"
				) {
					targetPos.y -= 0.9f;
				} else if (trig.tag == "unbreakableBrickBottomVertical") {
					targetPos.y -= 1.65f;
				}				
				temp.y = targetPos.y;
				transform.position = temp;
				AudioSource.PlayClipAtPoint (clip, transform.position);
				StartCoroutine ("ResetStickyArrow");
			}
		}
		if (
			trig.tag ==	"brokenBrickTop" ||
			trig.tag == "brokenBrickBottom" ||
			trig.tag == "brokenBrickLeft" ||
			trig.tag == "brokenBrickRight"
		) {
			if (gameObject.tag == "firstArrow" || gameObject.tag == "firstStickyArrow") {
				PlayerController.instance.ShootOnce (true);
			} else if (gameObject.tag == "secondArrow" || gameObject.tag == "secondStickyArrow") {
				PlayerController.instance.ShootTwice (true);
			}

			gameObject.SetActive (false);
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

	private IEnumerator ResetStickyArrow () {
		yield return new WaitForSeconds (2.5f);

		if (this.gameObject.tag == "firstStickyArrow") {
			PlayerController.instance.ShootOnce (true);
			this.gameObject.SetActive (false);
		} else if (this.gameObject.tag == "secondStickyArrow") {
			PlayerController.instance.ShootTwice (true);
			this.gameObject.SetActive (false);
		}
	}
}
