using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameplayController : MonoBehaviour {

	public static GameplayController instance;

	public static int smallBallsCount;

	public GameObject panelBG, levelFinishedPanel, playerDiedPanel, pausePanel;
	public float levelTime = 300;
	public Text liveText, scoreText, levelTimerText, showScoreAtEndOfLevelText, countdownAndBeginLevelText, watchVideoText;
	public int playerLives, playerScore, coins;
	public bool isLevelInProgress;

	[SerializeField]
	private GameObject[] topAndBottomBricks, leftBricks, rightBricks;
	[SerializeField]
	private GameObject[] players;
	[SerializeField]
	private GameObject[] endOfLevelRewards;
	[SerializeField]
	private Button pauseBtn;
	private GameObject topBrick, bottomBrick, leftBrick, rightBrick;
	private Vector3 coordinates;
	private float countdownBeforeLevelBegins = 3.0f;
	private bool isGamePaused, hasLevelBegun, countdownLevel;

	private void Awake () {
		CreateInstance ();
		InitializeBricksAndPlayer ();
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

	public void PlayerDied () {
		countdownLevel = false;
		pauseBtn.interactable = false;
		isLevelInProgress = false;

		smallBallsCount = 0;
		playerLives--;
		GameController.instance.currentLives = playerLives;
		GameController.instance.currentScore = playerScore;

		if (playerLives < 0) {
			StartCoroutine (PromptUserToWatchVideo ());
		} else {
			StartCoroutine (PlayerDiedRestartLevel ());
		}
	}

	public void CountSmallBalls () {
		smallBallsCount--;
		if (smallBallsCount <= 0) {
			StartCoroutine (LevelCompleted ());
		}
	}

	public void GoToMapButton () {
		GameController.instance.currentScore = playerScore;

		if (GameController.instance.highscore < GameController.instance.currentScore) {
			GameController.instance.highscore = GameController.instance.currentScore;
			GameController.instance.Save ();
		}

		if (Time.timeScale == 0) {
			Time.timeScale = 1;
		}

		SceneManager.LoadScene ("levelMenu", LoadSceneMode.Single);

		if (LoadingScreen.instance != null) {
			LoadingScreen.instance.PlayLoadingScreen ();
		}
	}

	public void RestartCurrentLevel () {
		smallBallsCount = 0;
		coins = 0;

		GameController.instance.currentLives = playerLives;
		GameController.instance.currentScore = playerScore;

		SceneManager.LoadScene (SceneManager.GetActiveScene ().name, LoadSceneMode.Single);

		if (LoadingScreen.instance != null) {
			LoadingScreen.instance.PlayLoadingScreen ();
		}
	}

	public void GoToNextLevel () {
		GameController.instance.currentScore = playerScore;
		GameController.instance.currentLives = playerLives;

		if (GameController.instance.highscore < GameController.instance.currentScore) {
			GameController.instance.highscore = GameController.instance.currentScore;
			GameController.instance.Save ();
		}

		int nextLevel = GameController.instance.currentLevel;
		nextLevel++;

		if (nextLevel < GameController.instance.levels.Length) {
			GameController.instance.currentLevel = nextLevel;
		}

		SceneManager.LoadScene ("level" + nextLevel, LoadSceneMode.Single);

		if (LoadingScreen.instance != null) {
			LoadingScreen.instance.PlayLoadingScreen ();
		}
	}

	public void PauseGame () {
		if (!hasLevelBegun) {
			if (isLevelInProgress) {
				if (!isGamePaused) {
					countdownLevel = false;
					isLevelInProgress = false;
					isGamePaused = true;

					panelBG.SetActive (true);
					pausePanel.SetActive (true);

					Time.timeScale = 0;
				}
			}
		}
	}

	public void ResumeGame () {
		countdownLevel = true;
		isLevelInProgress = true;
		isGamePaused = false;

		panelBG.SetActive (false);
		pausePanel.SetActive (false);

		Time.timeScale = 1;
	}

	public void DontWatchVideoAndQuit () {
		GameController.instance.currentScore = playerScore;

		if (GameController.instance.highscore < GameController.instance.currentScore) {
			GameController.instance.highscore = GameController.instance.currentScore;
			GameController.instance.Save ();
		}

		Time.timeScale = 1;
		SceneManager.LoadScene ("levelMenu", LoadSceneMode.Single);

		if (LoadingScreen.instance != null) {
			LoadingScreen.instance.PlayLoadingScreen ();
		}
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

		if (countdownLevel) {
			LevelCountdownTime ();
		}
	}

	private void InitializeBricksAndPlayer () {

		coordinates = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, Screen.height, 0));

		int index = Random.Range (0, topAndBottomBricks.Length);

		topBrick = Instantiate(topAndBottomBricks[index]);
		bottomBrick = Instantiate (topAndBottomBricks [index]);
		leftBrick = Instantiate (leftBricks [index], new Vector3 (0, 0, 0), Quaternion.Euler (new Vector3 (0, 0, 270))) as GameObject;
		rightBrick = Instantiate (rightBricks [index], new Vector3 (0, 0, 0), Quaternion.Euler (new Vector3 (0, 0, 90))) as GameObject;

		topBrick.tag = "topBrick";

		topBrick.transform.position = new Vector3 (-coordinates.x + 9, coordinates.y, 0);
		bottomBrick.transform.position = new Vector3 (-coordinates.x + 9, -coordinates.y, 0);
		leftBrick.transform.position = new Vector3 (coordinates.x, coordinates.y - 5, 0);
		rightBrick.transform.position = new Vector3 (-coordinates.x, coordinates.y - 5, 0);

		Instantiate (players [GameController.instance.selectedPlayer]);
	}

	private void LevelCountdownTime () {
		if (Time.timeScale == 1) {
			levelTime -= Time.deltaTime;
			levelTimerText.text = levelTime.ToString ("F0");

			if (levelTime <= 0) {
				playerLives--;
				GameController.instance.currentLives = playerLives;
				GameController.instance.currentScore = playerScore;

				if (playerLives < 0) {
					StartCoroutine (PromptUserToWatchVideo ());
				} else {
					StartCoroutine (PlayerDiedRestartLevel ());
				}
			}
		}
	}

	private IEnumerator PlayerDiedRestartLevel () {
		isLevelInProgress = false;

		coins = 0;
		smallBallsCount = 0;

		Time.timeScale = 0;

		if (LoadingScreen.instance != null) {
			LoadingScreen.instance.FadeOut ();
		}

		yield return StartCoroutine (MyCoroutine.WaitForRealSeconds (1.25f));

		SceneManager.LoadScene (SceneManager.GetActiveScene ().name, LoadSceneMode.Single);

		if (LoadingScreen.instance != null) {
			LoadingScreen.instance.PlayLoadingScreen ();
		}
	}

	private IEnumerator LevelCompleted () {
		countdownLevel = false;
		pauseBtn.interactable = false;

		int unlockedLevel = GameController.instance.currentLevel;
		unlockedLevel++;

		if (unlockedLevel < GameController.instance.levels.Length) {
			GameController.instance.levels[unlockedLevel] = true;
		}

		Instantiate (endOfLevelRewards [GameController.instance.currentLevel], new Vector3 (0, Camera.main.orthographicSize, 0), Quaternion.identity);

		if (GameController.instance.doubleCoins) {
			coins *= 2;
		}
		GameController.instance.coins = coins;
		GameController.instance.Save ();

		yield return StartCoroutine (MyCoroutine.WaitForRealSeconds (4f));
		isLevelInProgress = false;
		PlayerController.instance.StopMoving ();
		Time.timeScale = 0;

		levelFinishedPanel.SetActive (true);
		showScoreAtEndOfLevelText.text = playerScore.ToString ();
	}

	private IEnumerator PromptUserToWatchVideo () {
		isLevelInProgress = false;
		countdownLevel = false;
		pauseBtn.interactable = false;
		Time.timeScale = 0;

		yield return StartCoroutine(MyCoroutine.WaitForRealSeconds(0.7f));

		playerDiedPanel.SetActive (true);
	}

	private IEnumerator GivePlaterLivesAfterWatchingVideo () {
		watchVideoText.text = "Thank You For Watching!";

		yield return StartCoroutine (MyCoroutine.WaitForRealSeconds (2f));

		coins = 0;
		playerLives = 2;
		smallBallsCount = 0;

		GameController.instance.currentLives = playerLives;
		GameController.instance.currentScore = playerScore;

		Time.timeScale = 0;

		if (LoadingScreen.instance != null) {
			LoadingScreen.instance.FadeOut ();
		}
	}
}