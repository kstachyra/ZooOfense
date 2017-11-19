using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject menuScreen;

    public GameObject upgradeScreen;

    private void Start()
    {
        if (PlayerPrefs.HasKey("back") && PlayerPrefs.GetInt("back") == 1)
        {
            PlayerPrefs.SetInt("back", 0);
            menuScreen.SetActive(false);
        }
    }

    public void OnStartScreenClick()
    {  
        menuScreen.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void UpgradeMenu()
    {
        upgradeScreen.SetActive(true);
    }

    public void HideUpgradeMenu()
    {
        upgradeScreen.SetActive(false);
    }
}
