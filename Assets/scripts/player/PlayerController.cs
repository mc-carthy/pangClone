using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public static PlayerController instance;

	public bool hasShield, isInvincible, singleArrow, doubleArrows, singleStickyArrow, doubleStickyArrows, shootFirstArrow, shootSecondArrow;

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
	[SerializeField]
	private GameObject shield;
	private float height;
	private bool canWalk;
	private bool shootOnce, shootTwice;
	private bool moveLeft, moveRight;
	private Button shootBtn;
	private string arrow;

	private void Awake () {
		float cameraHeight = Camera.main.orthographicSize;
		height = -cameraHeight - 0.8f;

		MakeInstance ();

		shootBtn = GameObject.FindGameObjectWithTag ("shootButton").GetComponent<Button> ();
		shootBtn.onClick.RemoveAllListeners ();
		shootBtn.onClick.AddListener (() => ShootArrow ());
	}

	private void Update () {

	}

	private void FixedUpdate () {
		PlayerMoveKeyboard ();
		MovePlayerTouch ();
	}

	private void OnTriggerEnter2D (Collider2D trig) {

		if (trig.tag == "singleArrow") {
			if (!singleArrow) {
				arrow = "arrow";
				if (!shootFirstArrow) {
					shootOnce = true;
				}
				shootTwice = false;
				singleArrow = true;
				doubleArrows = false;
				singleStickyArrow = false;
				doubleStickyArrows = false;
			}
		}

		if (trig.tag == "doubleArrow") {
			if (!doubleArrows) {
				arrow = "doubleArrows";
				if (!shootFirstArrow) {
					shootOnce = true;
				}
				if (!shootTwice) {
					shootTwice = true;
				}
				singleArrow = false;
				doubleArrows = true;
				singleStickyArrow = false;
				doubleStickyArrows = false;
			}
		}

		if (trig.tag == "singleStickyArrow") {
			if (!singleStickyArrow) {
				arrow = "stickyArrow";
				if (!shootFirstArrow) {
					shootOnce = true;
				}
				shootTwice = false;
				singleArrow = false;
				doubleArrows = false;
				singleStickyArrow = true;
				doubleStickyArrows = false;
			}
		}

		if (trig.tag == "doubleStickyArrow") {
			if (!doubleArrows) {
				arrow = "doubleStickyArrows";
				if (!shootFirstArrow) {
					shootOnce = true;
				}
				if (!shootTwice) {
					shootTwice = true;
				}
				singleArrow = false;
				doubleArrows = false;
				singleStickyArrow = false;
				doubleStickyArrows = true;
			}
		}

		if (trig.tag == "watch") {
			GameplayController.instance.levelTime += Random.Range (10, 20);
		}

		if (trig.tag == "shield") {
			hasShield = true;
			shield.SetActive (true);
		}

		if (trig.tag == "dynamite") {

		}
	}

	public void ShootArrow () {
		if (GameplayController.instance.isLevelInProgress) {
			if (shootOnce) {

				if (arrow == "arrow") {
					Instantiate (arrows [0], new Vector3 (transform.position.x, height, 0f), Quaternion.identity);
				} else if (arrow == "stickyArrow") {
					Instantiate (arrows [2], new Vector3 (transform.position.x, height, 0f), Quaternion.identity);
				}
				StartCoroutine (PlayShootAnimation ());
				shootOnce = false;
				shootFirstArrow = true;

			} else if (shootTwice) {
				if (arrow == "doubleArrows") {
					Instantiate (arrows [1], new Vector3 (transform.position.x, height, 0f), Quaternion.identity);
				} else if (arrow == "doubleStickyArrows") {
					Instantiate (arrows [3], new Vector3 (transform.position.x, height, 0f), Quaternion.identity);
				}
				StartCoroutine (PlayShootAnimation ());
				shootTwice = false;
				shootSecondArrow = true;
			}
		}
	}

	public void ShootOnce (bool shootOnce) {
		this.shootOnce = shootOnce;
		shootFirstArrow = false;
	}

	public void ShootTwice (bool shootTwice) {
		if (doubleArrows || doubleStickyArrows) {
			this.shootTwice = shootTwice;
		}
		shootSecondArrow = false;
	}

	public void StopMoving () {
		moveLeft = moveRight = false;
		anim.SetBool ("walk", false);
	}

	public void MoveThePlayerLeft () {
		moveLeft = true;
		moveRight = false;
	}	

	public void MoveThePlayerRight () {
		moveRight = true;
		moveLeft = false;
	}

	public void DestroyShield () {
		StartCoroutine (SetPlayerInvincible ());
		hasShield = false;
		shield.SetActive (false);
	}

	private void MakeInstance () {
		if (instance == null) {
			instance = this;
		}
	}

	private IEnumerator PlayShootAnimation () {
		canWalk = false;
		anim.Play ("shoot");
		shootBtn.interactable = false;
		AudioSource.PlayClipAtPoint (shootClip, transform.position);
		yield return new WaitForSeconds (clip.length);
		anim.SetBool ("shoot", false);
		shootBtn.interactable = true;
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

	private void MovePlayerTouch () {
		if (GameplayController.instance.isLevelInProgress) {
			if (moveLeft) {
				MoveLeft ();
			}
			if (moveRight) {
				MoveRight ();
			}
		}
	}

	private void MoveRight () {
		float force = 0.0f;
		float velocity = Mathf.Abs (rb.velocity.x);

		if (canWalk) {
			if (velocity < maxVelocity) {
				force = speed;
			}

			Vector3 scale = transform.localScale;
			scale.x = 1.0f;
			transform.localScale = scale;
			anim.SetBool ("walk", true);
		}
		rb.AddForce (new Vector2 (force, 0));
	}

	private void MoveLeft () {
		float force = 0.0f;
		float velocity = Mathf.Abs (rb.velocity.x);

		if (canWalk) {
			if (velocity < maxVelocity) {
				force = -speed;
			}

			Vector3 scale = transform.localScale;
			scale.x = -1.0f;
			transform.localScale = scale;
			anim.SetBool ("walk", true);
		}
		rb.AddForce (new Vector2 (force, 0));
	}

	private void InitializePlayer () {
		canWalk = true;

		switch (GameController.instance.selectedWeapon) {
		case 0:
			arrow = "arrow";
			shootOnce = true;
			shootTwice = false;

			singleArrow = true;
			doubleArrows = false;
			singleStickyArrow = false;
			doubleStickyArrows = false;

			break;
		case 1:
			arrow = "doubleArrows";
			shootOnce = true;
			shootTwice = true;

			singleArrow = false;
			doubleArrows = true;
			singleStickyArrow = false;
			doubleStickyArrows = false;

			break;
		case 2:
			arrow = "singleStickyArrow";
			shootOnce = true;
			shootTwice = false;

			singleArrow = false;
			doubleArrows = false;
			singleStickyArrow = true;
			doubleStickyArrows = false;

			break;
		case 3:
			arrow = "doubleStickyArrows";
			shootOnce = true;
			shootTwice = true;

			singleArrow = false;
			doubleArrows = false;
			singleStickyArrow = false;
			doubleStickyArrows = true;

			break;
		default:
			arrow = "arrow";
			shootOnce = true;
			shootTwice = false;

			singleArrow = true;
			doubleArrows = false;
			singleStickyArrow = false;
			doubleStickyArrows = false;

			break;
		}
		Vector3 bottomBrick = GameObject.FindGameObjectWithTag("bottomBrick").transform.position;
		Vector3 temp = transform.position;

		switch(gameObject.name) {
		case "Player(clone)":
			temp.y = bottomBrick.y + 1.5f;
			break;
		case "Pirate(clone)":
			temp.y = bottomBrick.y + 1.5f;
			break;
		case "Zombie(clone)":
			temp.y = bottomBrick.y + 1.5f;
			break;
		case "Neanderthal(clone)":
			temp.y = bottomBrick.y + 1.5f;
			break;
		case "Joker(clone)":
			temp.y = bottomBrick.y + 1.5f;
			break;		
		case "Spartan(clone)":
			temp.y = bottomBrick.y + 1.5f;
			break;
		}
	}

	private IEnumerator SetPlayerInvincible () {
		isInvincible = true;
		yield return StartCoroutine(MyCoroutine.WaitForRealSeconds(3));
		isInvincible = false;
	}
}