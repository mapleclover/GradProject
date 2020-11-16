using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneOrder1 : MonoBehaviour
{
    private GameObject MainCamera;
    private GameObject HP;
    private GameObject MP;
    private GameObject Slime;
    private GameObject MsgBox;
    private BGMManager bm;
    private FadeManager fm;

    // Start is called before the first frame update
    void Awake()
    {
        MainCamera = GameObject.Find("Main Camera");
        HP = GameObject.Find("RPGHeroHP");
        MP = GameObject.Find("RPGHeroMP");
        Slime = GameObject.Find("Slime");
        MsgBox = GameObject.Find("ChatBox");
        bm = FindObjectOfType<BGMManager>();
        fm = FindObjectOfType<FadeManager>();
    }

    private void Start()
    {
        MsgBox.GetComponent<Animator>().SetBool("Show", false);
        StartCoroutine(order());
    }
    // Update is called once per frame
    IEnumerator order()
    {
        MP.GetComponent<Animator>().SetTrigger("Died");
        HP.GetComponent<Animator>().SetBool("Run", true);
        Vector3 Destination = new Vector3(Slime.GetComponent<Transform>().position.x, HP.GetComponent<Transform>().position.y, Slime.GetComponent<Transform>().position.z);
        Vector3 Original = HP.GetComponent<Transform>().position;
        Vector3 Move = (Destination - Original) / 1000;
        yield return new WaitForSeconds(1f);
        MsgBox.GetComponent<Animator>().SetBool("Show", true);
        for (int i = 0; i < 500; i++)
        {
            HP.GetComponent<Transform>().position += Move;
            MainCamera.GetComponent<Transform>().position -= new Vector3(0.01f,0,0);
            yield return null;
        }
        HP.GetComponent<Animator>().SetBool("Run", false);
        
        yield return new WaitForSeconds(2f);
        MsgBox.GetComponent<Handler>().ReadText();
        yield return new WaitForSeconds(2f);

        Vector3 Destination2 = new Vector3(Slime.GetComponent<Transform>().position.x + 5, MainCamera.GetComponent<Transform>().position.y, Slime.GetComponent<Transform>().position.z - 5);
        Vector3 Original2 = MainCamera.GetComponent<Transform>().position;
        Vector3 Move2 = (Destination2 - Original2) / 50;

        for(int i = 0; i < 50; i++)
        {
            MainCamera.GetComponent<Transform>().position += Move2;
            yield return null;
        }
        MsgBox.GetComponent<Handler>().ReadText();
        HP.GetComponent<Animator>().SetBool("Run", true);
        for (int i = 0; i < 100; i++)
        {
            HP.GetComponent<Transform>().position += (Move * 4.5f);
            yield return null;
        }
        fm.FadeOut();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(3);
    }
}
