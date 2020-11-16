using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    private FadeManager fm;
    private BGMPlayer bm;
    private AudioManager SFXm;
    private void Start()
    {
        fm = FindObjectOfType<FadeManager>();
        bm = FindObjectOfType<BGMPlayer>();
        SFXm = FindObjectOfType<AudioManager>();
    }

    public void LoadScene(string nextSceneName)
    {
        Time.timeScale = 1;
        StartCoroutine(LoadCertainScene(nextSceneName));
    }

    IEnumerator LoadCertainScene(string nextSceneName)
    {
        fm.FadeOut();
        SFXm.Play("Decision");
        yield return new WaitForSeconds(1f);
        bm.MapCheck = null;
        if(nextSceneName == "")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
