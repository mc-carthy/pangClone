using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerMenuController : MonoBehaviour {

	public Text scoreText, coinText;
	public bool[] players;
	public bool[] weapons;
	public Image[] priceTags;
	public Image[] weaponIcons;
	public Sprite[] weaponArrows;
	public int selectedWeapon;
	public int selectedPlayer;

	public void GoToLevelMenu () {
		SceneManager.LoadScene ("levelMenu", LoadSceneMode.Single);
	}

	public void GoToMainMenu () {
		SceneManager.LoadScene ("menu", LoadSceneMode.Single);
	}

	public void Player1Button () {
		if (selectedPlayer != 0) {
			selectedPlayer = 0;
			selectedWeapon = 0;

			weaponIcons [selectedPlayer].gameObject.SetActive (true);
			weaponIcons [selectedPlayer].sprite = weaponArrows [selectedWeapon];

			for (int i = 0; i < weaponIcons.Length; i++) {
				if (i == selectedPlayer) {
					continue;
				}
				weaponIcons [i].gameObject.SetActive (false);
			}
			GameController.instance.selectedPlayer = selectedPlayer;
			GameController.instance.selectedWeapon = selectedWeapon;
			GameController.instance.Save ();
		} else {
			selectedWeapon++;
			if (selectedWeapon == weapons.Length) {
				selectedWeapon = 0;
			}
			bool foundWeapon = true;

			while (foundWeapon) {
				if (weapons [selectedWeapon]) {
					weaponIcons [selectedPlayer].sprite = weaponArrows [selectedWeapon];
					GameController.instance.selectedWeapon = selectedWeapon;
					GameController.instance.Save ();
					foundWeapon = false;
				} else {
					if (selectedWeapon == weapons.Length) {
						selectedWeapon = 0;
					}
				}
			}
		}
	}

	private void InitializePlayerController () {
		scoreText.text = GameController.instance.highscore.ToString ();
		coinText.text = GameController.instance.coins.ToString ();

		players = GameController.instance.players;
		weapons = GameController.instance.weapons;
		selectedPlayer = GameController.instance.selectedPlayer;
		selectedWeapon = GameController.instance.selectedWeapon;

		for (int i = 0; i < weaponIcons.Length; i++) {
			weaponIcons [i].gameObject.SetActive (false);
		}

		for (int i = 1; i < players.Length; i++) {
			if (players [i]) {
				priceTags [i - 1].gameObject.SetActive (false);
			}
		}

		weaponIcons [selectedPlayer].gameObject.SetActive (true);
		weaponIcons [selectedPlayer].sprite = weaponArrows [selectedWeapon];
	}
}
