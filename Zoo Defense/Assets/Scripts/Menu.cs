using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject menuScreen;

    public void OnStartScreenClick()
    {
        menuScreen.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(0);
    }
}
