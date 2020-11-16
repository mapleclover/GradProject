using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseManager : MonoBehaviour
{
    public GameObject pauseUI;
    private BGMManager bm;
    private bool isPause = false;

    private void Awake()
    {
        bm = FindObjectOfType<BGMManager>();
    }
    private void Update()
    {
        if(SceneManager.GetActiveScene().name != "1. Start")
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if(!isPause)
                {
                    bm.Pause();
                    pauseUI.SetActive(true);
                    isPause = true;
                    Time.timeScale = 0;
                }
                else
                {
                    resume();
                }
            }
        }
    }

    public void resume()
    {
        bm.UnPause();
        pauseUI.SetActive(false);
        isPause = false;
        Time.timeScale = 1;
    }
}
