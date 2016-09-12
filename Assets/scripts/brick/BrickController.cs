using UnityEngine;
using System.Collections;
//[ExecuteInEditMode]

public class BrickController : MonoBehaviour {

	[SerializeField]
	private Animator anim;
	[SerializeField]
	private AnimationClip clip;

	public float x, y;

	private void Start () {
		transform.position = Camera.main.ViewportToWorldPoint (new Vector3 (x, y, 5));
	}

	private void Update () {
//		transform.position = Camera.main.ViewportToWorldPoint (new Vector3(x, y, 5));
	}

	public IEnumerator BreakBrick () {
		anim.SetBool ("break", true);;
		yield return new WaitForSeconds (clip.length);
		gameObject.SetActive (false);
	}
}
