using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameController : MonoBehaviour {

	public static GameController instance;
	private GameData data;

	public int currentLevel;
	public int currentScore;

	public bool isGameStartedForFirstTime;
	public bool isMusicOn;
	public int selectedPlayer;
	public int selectedWeapon;
	public int coins;
	public int highscore;
	public bool[] players;
	public bool[] levels;
	public bool[] weapons;
	public bool[] achievements;
	public bool[] collectedItems;

	private void Awake () {
		MakeSingleton ();
		InitializeVariables ();
	}

	public void Save () {
		FileStream file = null;

		try {
			BinaryFormatter bf = new BinaryFormatter();
			file = File.Create(Application.persistentDataPath + "/gameData.dat");

			if (data != null) {
				data.SetIsGameStartedForFirstTime(isGameStartedForFirstTime);
				data.SetIsMusicOn(isMusicOn);
				data.SetSelectedPlayer(selectedPlayer);
				data.SetSelectedWeapon(selectedWeapon);
				data.SetCoins(coins);
				data.SetHighscore(highscore);
				data.SetPlayers(players);
				data.SetLevels(levels);
				data.SetWeapons(weapons);
				data.SetAchievements(achievements);
				data.SetCollectedItems(collectedItems);

				bf.Serialize(file, data);
			}
		} catch (Exception e) {

		} finally {
			if (file != null) {
				file.Close ();
			}
		}
	}

	public void Load () {
		FileStream file = null;

		try {
			BinaryFormatter bf = new BinaryFormatter();
			file = File.Open(Application.persistentDataPath + "/gameData.dat", FileMode.Open);

			data = (GameData)bf.Deserialize(file);

		} catch (Exception e) {

		} finally {
			if (file != null) {
				file.Close ();
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

	private void InitializeVariables () {
		Load ();
		if (data != null) {
			isGameStartedForFirstTime = data.GetIsGameStartedForFirstTime ();
		} else {
			isGameStartedForFirstTime = true;
		}

		if (isGameStartedForFirstTime) {
			selectedPlayer = 0;
			selectedWeapon = 0;
			highscore = 0;
			coins = 0;

			isGameStartedForFirstTime = false;
			isMusicOn = false;

			players = new bool[6];
			levels = new bool[40];
			weapons = new bool[4];
			achievements = new bool[8];
			collectedItems = new bool[40];

			players [0] = true;
			for (int i = 1; i < players.Length; i++) {
				players [i] = false;
			}

			levels [0] = true;
			for (int i = 1; i < levels.Length; i++) {
				levels [i] = false;
			}

			weapons [0] = true;
			for (int i = 1; i < weapons.Length; i++) {
				weapons [i] = false;
			}

			for (int i = 0; i < achievements.Length; i++) {
				achievements [i] = false;
			}

			for (int i = 0; i < collectedItems.Length; i++) {
				collectedItems [i] = false;
			}

			data = new GameData ();

			data.SetIsGameStartedForFirstTime (isGameStartedForFirstTime);
			data.SetIsMusicOn (isMusicOn);
			data.SetSelectedPlayer (selectedPlayer);
			data.SetSelectedWeapon (selectedWeapon);
			data.SetCoins (coins);
			data.SetHighscore (highscore);
			data.SetPlayers (players);
			data.SetLevels (levels);
			data.SetWeapons (weapons);
			data.SetAchievements (achievements);
			data.SetCollectedItems (collectedItems);

			Save ();

			Load ();
		} else {
			isGameStartedForFirstTime = data.SetIsGameStartedForFirstTime ();
			isMusicOn = data.SetIsMusicOn ();
			selectedPlayer = data.SetSelectedPlayer ();
			selectedWeapon = data.SetSelectedWeapon ();
			coins = data.SetCoins ();
			highscore = data.SetHighscore ();
			players = data.SetPlayers ();
			levels = data.SetLevels ();
			weapons = data.SetWeapons ();
			achievements = data.SetAchievements ();
			collectedItems = data.SetCollectedItems ();
		}
	}
}

[Serializable]
class GameData {

	private bool isGameStartedForFirstTime;
	private bool isMusicOn;
	private int selectedPlayer;
	private int selectedWeapon;
	private int coins;
	private int highscore;
	private bool[] players;
	private bool[] levels;
	private bool[] weapons;
	private bool[] achievements;
	private bool[] collectedItems;

	public void SetIsGameStartedForFirstTime (bool isGameStartedForFirstTime) {
		this.isGameStartedForFirstTime = isGameStartedForFirstTime;
	}

	public bool GetIsGameStartedForFirstTime () {
		return isGameStartedForFirstTime;
	}

	public void SetIsMusicOn (bool isMusicOn) {
		this.isMusicOn = isMusicOn;
	}

	public bool GetIsMusicOn () {
		return isMusicOn;
	}

	public void SetSelectedPlayer (int selectedPlayer) {
		this.selectedPlayer = selectedPlayer;
	}

	public int GetIsSelectedPlayer () {
		return selectedPlayer;
	}

	public void SetSelectedWeapon (int selectedWeapon) {
		this.selectedWeapon = selectedWeapon;
	}

	public int GetSelectedWeapon () {
		return selectedWeapon;
	}

	public void SetCoins (int coins) {
		this.coins = coins;
	}

	public int GetCoins () {
		return coins;
	}

	public void SetHighscore (int highscore) {
		this.highscore = highscore;
	}

	public int GetHighscore() {
		return highscore;
	}

	public void SetPlayers (bool[] players) {
		this.players = players;
	}

	public bool[] GetPlayers () {
		return players;
	}

	public void SetLevels (bool[] levels) {
		this.levels = levels;
	}

	public bool[] GetLevels () {
		return levels;
	}

	public void SetWeapons (bool[] weapons) {
		this.weapons = weapons;
	}

	public bool[] GetWeapons () {
		return weapons;
	}

	public void SetAchievements (bool[] achievements) {
		this.achievements = achievements;
	}

	public bool[] GetAchievements () {
		return achievements;
	}

	public void SetCollectedItems (bool[] collectedItems) {
		this.collectedItems = collectedItems;
	}

	public bool[] GetCollectedItems () {
		return collectedItems;
	}

}