using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public static int selectedTower = -1;

    public Text levelText;
    public Text moneyText;
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
        levelText.text = "Level: " + level.ToString();
    }

    public void SetMoney(int money)
    {
        moneyText.text = "Money: " + money + "$";
    }

    public void OnTowerClick(bool active)
    {
        if(active && toggleGroup.ActiveToggles().Count() > 0)
        {
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

    #endregion
}
