using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject ScreenDim;

    private bool isPaused;
    private float bgmVolume;

    private void Start()
    {
        isPaused = false;

        try
        {
            PauseMenu.SetActive(isPaused);
        } catch (UnassignedReferenceException) {
            //Debug.Log("PauseMenu not set in Inspector");
        }

        bgmVolume = FindObjectOfType<AudioManager>().GetVolume("BGM");
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)   //Main Menu
        {
            
        }

        if (SceneManager.GetActiveScene().buildIndex == 1)   //Game Screen
        {
            //--------PAUSE MENU---------//
            if ((!isPaused && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))) 
               || !Application.isFocused)
            {
                PauseGame(true);
            }
            else if (isPaused && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)))
            {
                PauseGame(false);
            }

        }
    }

    public void PauseGame(bool paused)
    {
        isPaused = paused;
        PauseMenu.SetActive(paused);
        ScreenDim.SetActive(paused);
        float pauseVolume = bgmVolume / 3;

        if (paused)
        {
            FindObjectOfType<AudioManager>().SetVolume("BGM", pauseVolume);
            //StartCoroutine(fadeAudio("BGM", bgmVolume, pauseVolume, 2f));
            Time.timeScale = 0;
        }
        else
        {
            FindObjectOfType<AudioManager>().SetVolume("BGM", bgmVolume);
            //StartCoroutine(fadeAudio("BGM", pauseVolume, bgmVolume, 2f));
            Time.timeScale = 1;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
        PauseGame(false);
    }

    public void BackToMenu()
    {
        FindObjectOfType<AudioManager>().Mute("BGM", true);
        SceneManager.LoadScene(0);
        PauseGame(false);
    }

    public void StartGame()
    {
        //additional stuff here
        FindObjectOfType<AudioManager>().Play("BGM");
        SceneManager.LoadScene(1);
    }

    public void OptionsMenu(bool open)
    {

    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator fadeAudio(string sound, float start, float end, float duration)
    {
        float volume = start;
        float timeElapsed = 0f;

        while (volume != end)
        {
            if (start < end) //increase volume
            {
                if (timeElapsed < duration)
                {
                    volume = Mathf.Lerp(start, end, timeElapsed / duration);
                    timeElapsed += Time.deltaTime;
                    FindObjectOfType<AudioManager>().SetVolume(sound, volume);
                    Debug.Log("increase Lerp: " + volume);
                    yield return null;
                }
                else
                {
                    volume = end;
                    FindObjectOfType<AudioManager>().SetVolume(sound, volume);
                }
            }
            else //decrease volume
            {
                if (timeElapsed < duration)
                {
                    volume = Mathf.Lerp(start, end, 1-(timeElapsed / duration));
                    timeElapsed += Time.deltaTime;
                    Debug.Log("decrease Lerp: " + volume);
                    FindObjectOfType<AudioManager>().SetVolume(sound, volume);
                    yield return null;
                }
                else
                {
                    volume = end;
                    FindObjectOfType<AudioManager>().SetVolume(sound, volume);
                }
            }
        }
    }

    public void HighlightButtonOn(Image image)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)   //Main Menu
        {
            image.enabled = true;
        }

        if (SceneManager.GetActiveScene().buildIndex == 1)   //Game Screen
        {
            if (isPaused)
                image.enabled = true;
        }
    }

    public void HighlightButtonOff(Image image)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)   //Main Menu
        {
            image.enabled = false;
        }

        if (SceneManager.GetActiveScene().buildIndex == 1)   //Game Screen
        {
            if (isPaused)
                image.enabled = false;
        }
    }


    public bool getPaused()
    {
        return isPaused;
    }
}
