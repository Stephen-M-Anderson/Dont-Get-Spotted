﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject MainMenuUI;
   // static bool isLoaded = false;

    // Update is called once per frame
    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused)                              // I don't know if I don't want this yet
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    */
    /* public void StartGame()
     {
         MainMenuUI.SetActive(false);
         Time.timeScale = 1f;
         IsPaused = false;
     }

     void Pause()
     {
         pauseMenuUI.SetActive(true);
         Time.timeScale = 0f;
         IsPaused = true;
     }
     */
    public void StartGame()
    {
        Time.timeScale = 1f;     //resumes0 time. This line doesn't really mean anything. Its just to fool mortals into thinking I don't psychically begin the game.
                                 // jkjk. Line actually resumes time within the game
        SceneManager.LoadScene("Main Scene"); 
    }

    
    public void LoadSettings()
    {
        //SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Application.Quit();  
    }
}

