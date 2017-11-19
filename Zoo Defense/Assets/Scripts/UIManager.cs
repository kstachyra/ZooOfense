using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public static int selectedTower = -1;

    public Button upgradeButton;
    public static event Action<bool> onTowerUIChange;

    public Transform sellCanvas;
    public Button sellButton;
    public Text levelText;
    public Text moneyText;
    public Text capturedText;
    public Text lifeText;

    public ToggleGroup toggleGroup;
    public Image[] towersImage;
    public Text[] towersPrice;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {

    }

    #region UI

    public void SetLevel(int level)
    {
        levelText.text = level.ToString();
    }

    public void SetMoney(int money)
    {
        moneyText.text = money.ToString();
    }

    public void SetCaptured(int captured)
    {
        capturedText.text = captured.ToString();
    }

    public void SetLife(int current, int max)
    {
        lifeText.text = current + "/" + max;
    }

    public void DisableTowerClick()
    {
        if(onTowerUIChange != null)
        {
            onTowerUIChange(false);
        }

        toggleGroup.SetAllTogglesOff();
    }

    public void OnTowerClick(bool active)
    {
        if(onTowerUIChange != null)
        {
            onTowerUIChange(active);
        }

        if(active && toggleGroup.ActiveToggles().Count() > 0)
        {
            UIManager.instance.HideSellButton();
            selectedTower = int.Parse(toggleGroup.ActiveToggles().First().name);
        }
        else
        {
            selectedTower = -1;
        }
    }

    public void SetTower(int id, int cost, Sprite image)
    {
        towersImage[id].sprite = image;
        towersPrice[id].text = cost + "$";
    }

    public void SetSellCanvas(Action onClick, Vector2 position)
    {
        sellButton.onClick.RemoveAllListeners();
        sellCanvas.gameObject.SetActive(true);
        sellButton.transform.localScale = Vector3.zero;
        sellButton.transform.DOScale(1, 0.1f);
        sellButton.onClick.AddListener(() => onClick());
        sellCanvas.transform.position = new Vector3(position.x, position.y, -0.5f);
    }

    public void SetUpgradeCanvas(bool noUpgrades, Action onClick, Vector2 position)
    {
        if(noUpgrades)
        {
            upgradeButton.gameObject.SetActive(false);
        }
        else
        {
            upgradeButton.onClick.RemoveAllListeners();
            sellCanvas.gameObject.SetActive(true);
            upgradeButton.transform.localScale = Vector3.zero;
            upgradeButton.transform.DOScale(1, 0.1f);
            upgradeButton.interactable = onClick != null;
            upgradeButton.onClick.AddListener(() => onClick());
            sellCanvas.transform.position = new Vector3(position.x, position.y, -0.5f);
        }
    }

    public void HideSellButton()
    {
        sellButton.transform.DOScale(0, 0.1f).OnComplete(() =>
        {
            sellCanvas.gameObject.SetActive(false);
            sellButton.transform.localScale = Vector3.one;
        });
    }

    #endregion
}
