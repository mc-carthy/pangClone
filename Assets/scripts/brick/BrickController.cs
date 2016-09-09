using UnityEngine;
using System.Collections;

public class BrickController : MonoBehaviour {

	[SerializeField]
	private Animator anim;
	[SerializeField]
	private AnimationClip clip;

	public IEnumerator BreakBrick () {
		anim.SetBool ("break", true);;
		yield return new WaitForSeconds (clip.length);
		gameObject.SetActive (false);
	}
}
