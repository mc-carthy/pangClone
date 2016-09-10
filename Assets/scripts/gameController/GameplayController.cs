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
}