﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour {

	[SerializeField]
	private Animator settingsButtonAnim;
	[SerializeField]
	private Button musicButton, facebookButton;
	[SerializeField]
	private Sprite[] musicButtonSprites, facebookButtonSprites;
	[SerializeField]
	private GameObject infoPanel;
	[SerializeField]
	private Image infoImage;
	[SerializeField]
	private Sprite[] infoSprites;

	private bool hidden;
	private bool canTouchSettingsButton;
	private int infoIndex;

	private void Start () {
		canTouchSettingsButton = true;
		hidden = true;

		if (GameController.instance.isMusicOn) {
			musicButton.image.sprite = musicButtonSprites [1];
			MusicController.instance.PlayBGMusic ();
		} else {
			musicButton.image.sprite = musicButtonSprites [0];
			MusicController.instance.StopBGMusic ();
		}
		infoIndex = 0;
		infoImage.sprite = infoSprites [infoIndex];
	}

	public void SettingsButtonToggle () {
		StartCoroutine(DisableSettingsButtonWhilePlayingAnimation());
	}

	public void MusicButtonToggle () {
		if (GameController.instance.isMusicOn) {
			musicButton.image.sprite = musicButtonSprites [0];
			MusicController.instance.StopBGMusic ();
			GameController.instance.isMusicOn = false;
			GameController.instance.Save ();
		} else {
			musicButton.image.sprite = musicButtonSprites [1];
			MusicController.instance.PlayBGMusic ();
			GameController.instance.isMusicOn = true;
			GameController.instance.Save ();
		}
	}

	public void OpenInfoPanel () {
		infoPanel.SetActive (true);
	}

	public void CloseInfoPanel () {
		infoPanel.SetActive (false);
	}

	public void NextInfoPanel () {
		infoIndex++;

		if (infoIndex == infoSprites.Length) {
			infoIndex = 0;
		}

		infoImage.sprite = infoSprites [infoIndex];
	}

	public void PlayButton () {
		MusicController.instance.PlayClickClip ();
		SceneManager.LoadScene ("playerMenu", LoadSceneMode.Single);

	}

	public void ShopButton () {
		MusicController.instance.PlayClickClip ();
		SceneManager.LoadScene ("shopMenu", LoadSceneMode.Single);

	}

	private IEnumerator DisableSettingsButtonWhilePlayingAnimation () {
		if (canTouchSettingsButton) {
			if (hidden) {
				canTouchSettingsButton = false;
				settingsButtonAnim.Play ("slideIn");
				hidden = false;
				yield return new WaitForSeconds (1.2f);
				canTouchSettingsButton = true;
			} else {
				canTouchSettingsButton = false;
				settingsButtonAnim.Play ("slideOut");
				hidden = true;
				yield return new WaitForSeconds (1.2f);
				canTouchSettingsButton = true;
			}
		}
	}
}
