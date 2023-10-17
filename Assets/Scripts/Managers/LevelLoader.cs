using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] int mode;
    bool isPaused;
    [SerializeField] Animator transitionAnimator;
    [SerializeField] GameObject transition;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject gameOverMenu;
    private void Awake()
    {
        isPaused = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && mode == 1)
            TogglePause();
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
            Cursor.visible = false;
            isPaused = false;
        }
        else
        {
            pauseMenu.SetActive(true);
            Cursor.visible = true;
            Time.timeScale = 0;
            isPaused = true;
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        gameOverMenu.SetActive(true);
    }

    public void StartTransition()
    {
        Time.timeScale = 1;
        transition.SetActive(true);
        transitionAnimator.Play("Transition_Close_1");
    }

    public void Exit()
    {
        Time.timeScale = 1;
        transition.SetActive(true);
        transitionAnimator.Play("Transition_Close");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadIndexScene(int index)
    {
        SceneManager.LoadScene(index);
    }

}
