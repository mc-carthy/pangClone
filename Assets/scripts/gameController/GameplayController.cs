using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameplayController : MonoBehaviour {

	public static GameplayController instance;

	public static int smallBallsCount;

	public GameObject panelBG, levelFinishedPanel, playerDiedPanel, pausePanel;
	public float levelTime;
	public Text liveText, scoreText, levelTimerText, showScoreAtEndOfLevelText, countdownAndBeginLevelText, watchVideoText;
	public int playerLives, playerScore, coins;

	[SerializeField]
	private GameObject[] topAndBottomBricks, leftBricks, rightBricks;
	[SerializeField]
	private GameObject[] players;
	[SerializeField]
	private GameObject[] endOfLevelRewards;
	private GameObject topBrick, bottomBrick, leftBrick, rightBrick;
	private Vector3 coordinates;
	private float countdownBeforeLevelBegins = 3.0f;
	private bool isGamePaused, hasLevelBegun, isLevelInProgress, countdownLevel;

	private void Awake () {
		CreateInstance ();
	}

	private void Start () {
		InitializeGameplayController ();
	}

	private void Update () {
		UpdateGameplayController ();
	}

	public void SetHasLevelBegun (bool hasLevelBegun) {
		this.hasLevelBegun = hasLevelBegun;
	}

	private void CreateInstance () {
		if (instance == null) {
			instance = this;
		}
	}

	private void InitializeGameplayController () {
		if (GameController.instance.isGameStartedFromLevelMenu) {
			playerScore = 0;
			playerLives = 2;
			GameController.instance.currentScore = playerScore;
			GameController.instance.currentLives = playerLives;
			GameController.instance.isGameStartedFromLevelMenu = false;
		} else {
			playerScore = GameController.instance.currentScore;
			playerLives = GameController.instance.currentLives;
		}
		levelTimerText.text = levelTime.ToString ("F0");
		scoreText.text = "Score: " + playerScore;
		liveText.text = "x" + playerLives;
		Time.timeScale = 0;
		countdownAndBeginLevelText.text = countdownBeforeLevelBegins.ToString ("F0");
	}

	private void CountdownAndBeginLevel() {
		countdownBeforeLevelBegins -= (0.19f * 0.15f);
		countdownAndBeginLevelText.text = countdownBeforeLevelBegins.ToString ("F0");
		if (countdownBeforeLevelBegins <= 0) {
			Time.timeScale = 1;
			hasLevelBegun = true;
			isLevelInProgress = true;
			countdownLevel = true;
			countdownAndBeginLevelText.gameObject.SetActive (false);
		}
	}

	private void UpdateGameplayController () {
		scoreText.text = "Score: " + playerScore;

		if (hasLevelBegun) {
			CountdownAndBeginLevel ();
		}
	}
}