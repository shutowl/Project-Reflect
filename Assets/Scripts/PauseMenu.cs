using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNav : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject ScreenDim;

    private bool isPaused;
    private float bgmVolume;

    private void Start()
    {
        isPaused = false;
        PauseMenu.SetActive(isPaused);
        bgmVolume = FindObjectOfType<AudioManager>().GetVolume("BGM");
    }

    // Update is called once per frame
    void Update()
    {
        //--------PAUSE MENU---------//
        if (!isPaused && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)))
        {
            PauseGame(true);
        }
        else if (isPaused && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)))
        {
            PauseGame(false);
        }

    }

    public void PauseGame(bool paused)
    {
        isPaused = paused;
        PauseMenu.SetActive(paused);
        ScreenDim.SetActive(paused);

        if (paused)
        {
            float pauseVolume = bgmVolume / 3;
            FindObjectOfType<AudioManager>().SetVolume("BGM", pauseVolume);
            Time.timeScale = 0;
        }
        else
        {
            PauseMenu.SetActive(false);
            FindObjectOfType<AudioManager>().SetVolume("BGM", bgmVolume);
            Time.timeScale = 1;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Test Arena");
        PauseGame(false);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

}
