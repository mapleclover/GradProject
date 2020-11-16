using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMPlayer : MonoBehaviour
{
    static public BGMPlayer instance;

    public bool play = true;

    private BGMManager theBGM;
    public string MapCheck = null;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }

    private void Start()
    {
        theBGM = FindObjectOfType<BGMManager>();
    }

    private void Update()
    {
        if(MapCheck != SceneManager.GetActiveScene().name)
        {
            MapCheck = SceneManager.GetActiveScene().name;
 
            if (SceneManager.GetActiveScene().name.Contains("Start"))
            {
                theBGM.Play(0);

            }
            else if (SceneManager.GetActiveScene().name.Contains("Boss"))
            {
                theBGM.Play(3);
            }
            else if (SceneManager.GetActiveScene().name.Contains("Battle"))
            {
                theBGM.Play(2);
            }
            else
            {
                theBGM.Play(1);
            }
        }
    }
}
