using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour {

	public static LoadingScreen instance;

	[SerializeField]
	private GameObject backgroundImage, logoImage, text, fadePanel;
	[SerializeField]
	private Animator fadeAnim;

	private void Awake () {
		MakeSingleton ();
		Hide ();
	}

	public void PlayLoadingScreen () {
		StartCoroutine (ShowLoadingScreen ());
	}

	public void PlayFadeInAnimation () {
		StartCoroutine (FadeIn ());
	}

	public void FadeOut () {
		fadePanel.SetActive (true);
		fadeAnim.Play ("fadeOut");
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
		yield return StartCoroutine(MyCoroutine.WaitForRealSeconds (1f));
		Hide ();

		if (GameplayController.instance != null) {
			GameplayController.instance.SetHasLevelBegun (true);
		}
	}

	private IEnumerator FadeIn () {
		fadeAnim.Play ("fadeIn");

		yield return StartCoroutine(MyCoroutine.WaitForRealSeconds(0.4f));

		if (GameplayController.instance != null) {
			GameplayController.instance.SetHasLevelBegun (true);
		}

		yield return StartCoroutine(MyCoroutine.WaitForRealSeconds(0.9f));
		fadePanel.SetActive (false);
	}
}
