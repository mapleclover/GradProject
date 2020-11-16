using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;
using TMPro;

public class Handler : MonoBehaviour
{
    public GameObject Name;
    public GameObject Chat;
    public GameObject Choice1, Choice2;
    public Text Text1, Text2;
    public GameObject PartyJoin;
    public AudioManager SFXm;
    public BGMManager bm;
    public bool canSkip = true;
    public int i = 0;

    void Start()
    {
        bm = FindObjectOfType<BGMManager>();
        SFXm = FindObjectOfType<AudioManager>();
        ReadText();
    }
    public void ReadText(int k = 0)
    {
        if (k != 0)
        {
            Choice1.SetActive(false);
            Choice2.SetActive(false);
            i = k;
        }

        if (SceneManager.GetActiveScene().name == "2.1 Cut Scene")
        {
            canSkip = false;
            string[] lines = System.IO.File.ReadAllLines("Assets/Text/Chapter1.txt");
            foreach (string line in lines)
            {
                if (lines == null) return;
                else
                {
                    char[] seps = new char[] { ':' };
                    string[] StringList = lines[i].Split(seps);
                    if (StringList[0].Equals("루나"))
                    {
                        Name.GetComponent<TMPro.TextMeshProUGUI>().text = "Luna";
                        Chat.GetComponent<TMPro.TextMeshProUGUI>().text = StringList[1];
                    }
                    else if (StringList[0].Equals("주인공"))
                    {
                        Name.GetComponent<TMPro.TextMeshProUGUI>().text = "Arthur";
                        Chat.GetComponent<TMPro.TextMeshProUGUI>().text = StringList[1];
                    }
                    else if (StringList[0].Equals("End"))
                    {
                        PartyJoin.SetActive(true);
                        Destroy(this.gameObject);
                    }
                }
            }
            i++;
            SFXm.Play("Switch1");
        }
        else if(SceneManager.GetActiveScene().name == "3.2 Cut Scene")
        {
            string[] lines = System.IO.File.ReadAllLines("Assets/Text/Chapter2.txt");
            foreach (string line in lines)
            {
                if (lines == null) return;
                else
                {
                    char[] seps = new char[] { ':' };
                    string[] StringList = lines[i].Split(seps);
                    if (StringList[0].Equals("루나"))
                    {
                        Name.GetComponent<TMPro.TextMeshProUGUI>().text = "Luna";
                        Chat.GetComponent<TMPro.TextMeshProUGUI>().text = StringList[1];
                    }
                    else if (StringList[0].Equals("주인공"))
                    {
                        Name.GetComponent<TMPro.TextMeshProUGUI>().text = "Arthur";
                        Chat.GetComponent<TMPro.TextMeshProUGUI>().text = StringList[1];
                    }
                    else if (StringList[0].Equals("선택지"))
                    {
                        Name.GetComponent<TMPro.TextMeshProUGUI>().text = "";
                        Text1.text = StringList[1];
                        Text2.text = StringList[2];
                        Choice1.SetActive(true);
                        Choice2.SetActive(true);
                        i--;
                    }
                    else if (StringList[0].Equals("End"))
                    {
                        bm.Stop();
                        SFXm.Play("Friend");
                        PartyJoin.SetActive(true);
                        Destroy(this.gameObject);
                    }
                }
            }
            i++;
            SFXm.Play("Switch1");
        }
        else if (SceneManager.GetActiveScene().name == "4. Stage 1")
        {
            string[] lines = System.IO.File.ReadAllLines("Assets/Text/Chapter3.txt");
            foreach (string line in lines)
            {
                if (lines == null) return;
                else
                {
                    char[] seps = new char[] { ':' };
                    string[] StringList = lines[i].Split(seps);
                    if (StringList[0].Equals("루나"))
                    {
                        Name.GetComponent<TMPro.TextMeshProUGUI>().text = "Luna";
                        Chat.GetComponent<TMPro.TextMeshProUGUI>().text = StringList[1];
                    }
                    else if (StringList[0].Equals("주인공"))
                    {
                        Name.GetComponent<TMPro.TextMeshProUGUI>().text = "Arthur";
                        Chat.GetComponent<TMPro.TextMeshProUGUI>().text = StringList[1];
                    }
                    else if (StringList[0].Equals("End"))
                    {
                        Destroy(this.gameObject);
                    }
                }
            }
            i++;
            SFXm.Play("Switch1");
        }
        else if (SceneManager.GetActiveScene().name == "5.3 Boss")
        {
            string[] lines = System.IO.File.ReadAllLines("Assets/Text/Chapter4.txt");
            foreach (string line in lines)
            {
                if (lines == null) return;
                else
                {
                    char[] seps = new char[] { ':' };
                    string[] StringList = lines[i].Split(seps);
                    if (StringList[0].Equals("루나"))
                    {
                        Name.GetComponent<TMPro.TextMeshProUGUI>().text = "Luna";
                        Chat.GetComponent<TMPro.TextMeshProUGUI>().text = StringList[1];
                    }
                    else if (StringList[0].Equals("주인공"))
                    {
                        Name.GetComponent<TMPro.TextMeshProUGUI>().text = "Arthur";
                        Chat.GetComponent<TMPro.TextMeshProUGUI>().text = StringList[1];
                    }
                    else if (StringList[0].Equals("End"))
                    {
                        SceneManager.LoadScene(8);
                        Destroy(this.gameObject);
                    }
                }
            }
            i++;
            SFXm.Play("Switch1");
        }
    }    
        

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canSkip)
        {
            ReadText();
        }
    }
}
