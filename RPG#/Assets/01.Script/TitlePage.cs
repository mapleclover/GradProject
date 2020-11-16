using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitlePage : MonoBehaviour
{
    public GameObject OptionMenu;
    private FadeManager fm;
    private AudioManager SFXm;

    private void Start()
    {
        fm = FindObjectOfType<FadeManager>();
        SFXm = FindObjectOfType<AudioManager>();
    }
    public void ClickStart()
    {
        fm.FadeOut();
        SFXm.Play("Decision");
        StartCoroutine(StartClicked());
    }
    IEnumerator StartClicked()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }

    public void ClickLoad()
    {

    }

    public void ClickEnd()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
        Application.OpenURL(webplayerQuitURL);
#else
        Application.Quit();
#endif
    }

    public void ClickOption()
    {
        SFXm.Play("Switch3");
        OptionMenu.GetComponent<Animator>().SetBool("Show",true);
    }

    public void CloseOption()
    {
        SFXm.Play("Switch2");
        OptionMenu.GetComponent<Animator>().SetBool("Show", false);
    }
    
    public void ClickCredit()
    {

    }
}
