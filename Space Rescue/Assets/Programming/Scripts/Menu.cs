using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject _pausePanel;
    [SerializeField] GameObject _losePanel;
    [SerializeField] GameObject _winPanel;

    private void Update()
    {
        if (!_pausePanel.activeSelf || !_losePanel.activeSelf || !_losePanel.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void Resume(GameObject panelToClose)
    {
        panelToClose.SetActive(false);
    }

    public void Retry()
    {
        SceneManager.LoadScene("Real Game");
    }

    public void Quit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
