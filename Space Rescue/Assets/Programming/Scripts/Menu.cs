using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject _pausePanel;
    [SerializeField] GameObject _losePanel;
    [SerializeField] GameObject _winPanel;

    public void Resume(GameObject panelToClose)
    {
        panelToClose.SetActive(false);

        Time.timeScale = 1;
    }

    public void Retry()
    {
        SceneManager.LoadScene("Real Game");
        Time.timeScale = 1;
    }

    public void Quit()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
}
