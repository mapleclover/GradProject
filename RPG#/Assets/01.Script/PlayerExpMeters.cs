using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExpMeters : MonoBehaviour
{
    public GameObject character;
    public GameObject ExpBar;

    public void Update()
    {
        int Exp = character.GetComponent<CharacterInfo>().Experience;
        int MaxExp = character.GetComponent<CharacterInfo>().RequiredExperience;

        ExpBar.GetComponent<Image>().fillAmount = (float)Exp / MaxExp;
    }
}
