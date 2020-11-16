using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillExplain : MonoBehaviour
{
    public GameObject explainPanel;
    public TextMeshProUGUI explains;
    public TextMeshProUGUI[] buttons;

    private GameObject[] players;

    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }
    public void Show(int i)
    {
        if(buttons[i].text == "Sword Attack")
        {
            explains.text = "Slash Attack on Single Enemy (Weak) MP : 0";
            explainPanel.SetActive(true);
        }
        if (buttons[i].text == "Staff Attack")
        {
            explains.text = "Blunt Attack on Single Enemy (Weak) MP : 0";
            explainPanel.SetActive(true);
        }
        if (buttons[i].text == "X-Slash")
        {
            explains.text = "Slash Attack on Single Enemy Twice (Strong) MP : 10";
            explainPanel.SetActive(true);
        }
        if (buttons[i].text == "Fire Bolt")
        {
            explains.text = "<color=red>Fire Type</color> Magic Attack on Single Enemy (Medium) MP:12";
            explainPanel.SetActive(true);
        }
        if (buttons[i].text == "Potion")
        {
            explains.text = "<color=purple>Remains : 3";
            explainPanel.SetActive(true);
        }
    }

    public void Hide()
    {
        explainPanel.SetActive(false);       
    }

}
