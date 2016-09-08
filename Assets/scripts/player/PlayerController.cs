using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private float speed = 8.0f, maxVelocity = 4.0f;
	[SerializeField]
	private Rigidbody2D rb;
	[SerializeField]
	private Animator anim;

	private void Awake () {
	}

	private void FixedUpdate () {
		PlayerMoveKeyboard ();
	}

	private void PlayerMoveKeyboard () {
		float force = 0.0f;
		float velocity = Mathf.Abs (rb.velocity.x);

		float h = Input.GetAxisRaw ("Horizontal");

		if (h > 0) {
			if (velocity < maxVelocity) {
				force = speed;
			}

			Vector3 scale = transform.localScale;
			scale.x = 1.0f;
			transform.localScale = scale;
			anim.SetBool ("walk", true);
		} else if (h < 0) {
			if (velocity < maxVelocity) {
				force = -speed;
			}

			Vector3 scale = transform.localScale;
			scale.x = -1.0f;
			transform.localScale = scale;
			anim.SetBool ("walk", true);
		} else if (h == 0) {
			anim.SetBool ("walk", false);
		}
		rb.AddForce (new Vector2 (force, 0));
	}
}
