using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LevelController : MonoBehaviour {

	public Text scoreText, coinText;
	public bool[] levels;
	public Button[] levelButtons;
	public Text[] levelText;
	public Image[] levelLocks;
	public GameObject coinShopPanel;

	private void Start () {

	}

	public void OpenCoinShop () {
		coinShopPanel.SetActive (true);
	}

	public void CloseCoinShop () {
		coinShopPanel.SetActive (false);
	}

	public void GoToMenu () {
		SceneManager.LoadScene ("menu", LoadSceneMode.Single);
	}

	public void GoBackButton () {
		SceneManager.LoadScene ("palyerMenu", LoadSceneMode.Single);
	}

	public void PlayGame () {
		SceneManager.LoadScene ("main", LoadSceneMode.Single);
	}

	public void LoadLevel () {
		if (GameController.instance.isMusicOn) {
			MusicController.instance.GameIsLoadedTurnOffMusic ();
		}

		string level = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;

		for (int i = 0; i < levels.Length; i++) {
			if (string.Format ("level{0}", i) == level) {
				GameController.instance.currentLevel = i;
				LoadingScreen.instance.PlayLoadingScreen ();
			}
		}

		SceneManager.LoadScene (level, LoadSceneMode.Single);
	}

	private void InitializeLevelMenu () {
		scoreText.text = GameController.instance.highscore.ToString ();;
		coinText.text = GameController.instance.coins.ToString ();

		levels = GameController.instance.levels;

		for (int i = 1; i < levels.Length; i++) {
			if (levels [i]) {
				levelLocks [i - 1].gameObject.SetActive (false);
				levelText [i].gameObject.SetActive (true);
				levelText [i].text = (i + 1).ToString ();
			} else {
				levelButtons [i - 1].interactable = false;
				levelText [i - 1].gameObject.SetActive (false);
			}
		}
	}
}
