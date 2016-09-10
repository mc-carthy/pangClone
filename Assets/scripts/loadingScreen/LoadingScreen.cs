using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour {

	public static LoadingScreen instance;

	[SerializeField]
	private GameObject backgroundImage, logoImage, text;

	private void Awake () {
		MakeSingleton ();
	}

	public void PlayLoadingScreen () {
		StartCoroutine (ShowLoadingScreen ());
	}

	private void MakeSingleton () {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}
	}

	private void Show () {
		backgroundImage.SetActive (true);
		logoImage.SetActive (true);
		text.SetActive (true);
	}

	private void Hide () {
		backgroundImage.SetActive (false);
		logoImage.SetActive (false);
		text.SetActive (false);
	}

	private IEnumerator ShowLoadingScreen () {
		Show ();
		yield return new WaitForSeconds (1f);
		Hide ();

		// GameplayController.play
	}
}
