using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ShopMenuController : MonoBehaviour {

	public static ShopMenuController instance;

	public Text coinText, scoreText, buyArrowsText, watchVideoText;
	public Button weaponsTabBtn, specialTabBtn, earnCoinsBtn, yesBtn;
	public GameObject weaponItemsPanel, specialItemsPanel, earnCoinsItemsPanel, coinShopPanel, buyArrowsPanel;

	private void Awake () {
		MakeInstance ();
	}

	private void Start () {
		InitializeShopMenuController ();
	}

	public void GenericBuyArrows (int index) {
		if (!GameController.instance.weapons [index]) {
			if (GameController.instance.coins >= 7000) {
				buyArrowsPanel.SetActive (true);
				string arrowDescription;
				switch (index) {
				case 1:
					arrowDescription = "Double Arrows";
					break;
				case 2:
					arrowDescription = "Sticky Arrows";
					break;
				case 3:
					arrowDescription = "Double Sticky Arrows";
					break;
				default:
					arrowDescription = "This Item";
					break;
				}
				buyArrowsText.text = "Do You Want To Purchase " + arrowDescription + " ?";
				yesBtn.onClick.RemoveAllListeners ();
				yesBtn.onClick.AddListener (() => BuyArrow (index));
			} else { 
				buyArrowsPanel.SetActive (true);
				buyArrowsText.text = "You Don't Have Enough Coins. Do You Want To Purchase More Coins?";
				yesBtn.onClick.RemoveAllListeners ();
				yesBtn.onClick.AddListener (() => OpenCoinShop ());
			}
		}
	}

	public void BuyArrow (int index) {
		GameController.instance.weapons [index] = true;
		GameController.instance.coins -= 7000;
		GameController.instance.Save ();
		buyArrowsPanel.SetActive (false);
		coinText.text = GameController.instance.coins.ToString ();
	}

	public void OpenCoinShop () {
		if (buyArrowsPanel.activeInHierarchy) {
			buyArrowsPanel.SetActive (false);
		}
		coinShopPanel.SetActive (true);
	}

	public void CloseCoinShop () {
		coinShopPanel.SetActive (false);
	}

	public void OpenWeaponItemsPanel () {
		weaponItemsPanel.SetActive (true);
		specialItemsPanel.SetActive (false);
		earnCoinsItemsPanel.SetActive (false);
	}

	public void OpenSpecialItemsPanel () {
		weaponItemsPanel.SetActive (false);
		specialItemsPanel.SetActive (true);
		earnCoinsItemsPanel.SetActive (false);
	}

	public void OpenEarnCoinsPanel () {
		weaponItemsPanel.SetActive (false);
		specialItemsPanel.SetActive (false);
		earnCoinsItemsPanel.SetActive (true);
	}

	public void PlayGame () {
		SceneManager.LoadScene ("playerMenu", LoadSceneMode.Single);
	}

	public void GoToMenu () {
		SceneManager.LoadScene ("menu", LoadSceneMode.Single);
	}

	public void DontBuyArrows () {
		buyArrowsPanel.SetActive (false);
	}

	private void MakeInstance () {
		if (instance == null) {
			instance = this;
		}
	}

	private void InitializeShopMenuController () {
		coinText.text = GameController.instance.coins.ToString ();
		scoreText.text = GameController.instance.highscore.ToString ();
	}
}
