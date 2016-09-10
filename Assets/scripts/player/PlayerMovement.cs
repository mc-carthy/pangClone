using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class PlayerMovement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	public void OnPointerDown(PointerEventData data) {
		if (this.gameObject.tag == "moveLeftButton") {
			PlayerController.instance.MoveThePlayerLeft ();
		}
		if (this.gameObject.tag == "moveRightButton") {
			PlayerController.instance.MoveThePlayerRight ();
		}
	}

	public void OnPointerUp(PointerEventData data) {
		PlayerController.instance.StopMoving ();
	}
}
