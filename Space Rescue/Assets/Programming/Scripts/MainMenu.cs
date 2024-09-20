using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] string _sceneToLoad;

    [SerializeField] GameObject _quitPanel;

    public void StartBtn()
    {
        SceneManager.LoadScene(_sceneToLoad);
    }

    public void QuitYesBtn()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
