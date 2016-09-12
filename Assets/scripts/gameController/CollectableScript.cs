using UnityEngine;
using System.Collections;

public class CollectableScript : MonoBehaviour {

	private Rigidbody2D rb;

	private void Start () {
		rb = GetComponent<Rigidbody2D> ();

		if (this.gameObject.tag != "inGameCollectable") {
			Invoke ("DeactivateGameobject", Random.Range (2, 6));
		}

	}

	private void OnTriggerEnter2D (Collider2D trig) {
		if (trig.tag == "bottomBrick") {
			Vector3 temp = trig.transform.position;
			temp.y += 0.8f;
			transform.position = new Vector2(transform.position.x, temp.y);
			rb.isKinematic = true;
		}

		if (trig.tag == "Player") {

			if (this.gameObject.tag == "inGameCollectable") {
				GameController.instance.collectedItems [GameController.instance.currentLevel] = true;
				GameController.instance.Save ();

				if (GameplayController.instance != null) {
					if (GameController.instance.currentLevel == 0) {
						GameplayController.instance.playerScore += 1 * 1000;
					} else {
						GameplayController.instance.playerScore += GameController.instance.currentLevel * 1000;
					}
				}
			}

			this.gameObject.SetActive (false);
		}
	}

	private void DeactivateGameobject () {
		this.gameObject.SetActive (false);
	}
}
