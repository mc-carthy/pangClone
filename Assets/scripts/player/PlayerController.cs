using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public static PlayerController instance;

	private float speed = 8.0f, maxVelocity = 4.0f;
	[SerializeField]
	private Rigidbody2D rb;
	[SerializeField]
	private Animator anim;
	[SerializeField]
	private GameObject[] arrows;
	[SerializeField]
	private AnimationClip clip;
	[SerializeField]
	private AudioClip shootClip;
	private float height;
	private bool canWalk;
	private bool shootOnce, shootTwice;

	private void Awake () {
		float cameraHeight = Camera.main.orthographicSize;
		height = -cameraHeight - 0.8f;
		canWalk = true;
		shootOnce = shootTwice = true;
		MakeInstance ();
	}

	private void Update () {
		ShootArrow ();
	}

	private void FixedUpdate () {
		PlayerMoveKeyboard ();
	}

	public void ShootArrow () {
		if (Input.GetMouseButtonDown (0)) {
			if (shootOnce) {
				shootOnce = false;
				StartCoroutine (PlayShootAnimation ());
				Instantiate (arrows [2], new Vector3 (transform.position.x, height, 0f), Quaternion.identity);
			} else if (shootTwice) {
				shootTwice = false;
				StartCoroutine (PlayShootAnimation ());
				Instantiate (arrows [3], new Vector3 (transform.position.x, height, 0f), Quaternion.identity);
			}
		}
	}

	public void ShootOnce (bool shootOnce) {
		this.shootOnce = shootOnce;
	}

	public void ShootTwice (bool shootTwice) {
		this.shootTwice = shootTwice;
	}

	private void MakeInstance () {
		if (instance == null) {
			instance = this;
		}
	}

	private IEnumerator PlayShootAnimation () {
		canWalk = false;
		anim.Play ("shoot");
		AudioSource.PlayClipAtPoint (shootClip, transform.position);
		yield return new WaitForSeconds (clip.length);
		anim.SetBool ("shoot", false);
		canWalk = true;
	}

	private void PlayerMoveKeyboard () {
		float force = 0.0f;
		float velocity = Mathf.Abs (rb.velocity.x);

		float h = Input.GetAxisRaw ("Horizontal");

		if (canWalk) {
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
		}
		rb.AddForce (new Vector2 (force, 0));
	}
}
