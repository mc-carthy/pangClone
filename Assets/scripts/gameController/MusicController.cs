using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MusicController : MonoBehaviour {

	public static MusicController instance;

	private AudioSource bgMusic, click;
	private float time;

	private void Awake () {
		MakeSingleton ();
		AudioSource[] audioSources = GetComponents<AudioSource> ();
		bgMusic = audioSources [0];
		click = audioSources [1];
	}

	private void OnLevelWasLoaded () {
		if (SceneManager.GetActiveScene ().name == "levelMenu") {
			if (GameController.instance.isMusicOn) {
				if (!bgMusic.isPlaying) {
					bgMusic.time = time;
					bgMusic.Play ();
				}
			}
		}
	}

	private void MakeSingleton () {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}
	}

	public void PlayClickClip () {
		click.Play ();
	}

	public void PlayBGMusic () {
		if (!bgMusic.isPlaying) {
			bgMusic.Play ();
		}
	}

	public void StopBGMusic () {
		if (bgMusic.isPlaying) {
			bgMusic.Stop ();
		}
	}

	public void GameIsLoadedTurnOffMusic () {
		if (bgMusic.isPlaying) {
			time = bgMusic.time;
			bgMusic.Stop ();
		}
	}
}
