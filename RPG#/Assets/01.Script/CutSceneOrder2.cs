using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneOrder2 : MonoBehaviour
{
    private GameObject MainCamera;
    private GameObject HP;
    private GameObject MP;
    private GameObject Slime;
    private GameObject MsgBox;
    private BGMManager bm;
    private FadeManager fm;
    private Handler hd;
    private PlayerMoveControl pmc;

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
        hd = FindObjectOfType<Handler>();
        pmc = FindObjectOfType<PlayerMoveControl>();
    }

    private void Start()
    {
        MsgBox.GetComponent<Animator>().SetBool("Show", false);
        pmc.canMove = false;
        StartCoroutine(order());
    }
    // Update is called once per frame
    IEnumerator order()
    {
        yield return new WaitForSeconds(1f);

        MsgBox.GetComponent<Animator>().SetBool("Show", true);

        yield return new WaitUntil(() => hd.i == 9);

        MsgBox.GetComponent<Animator>().SetBool("Show", false);

        MP.SetActive(false);

        pmc.canMove = true;
    }
}
