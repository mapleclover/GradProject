using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMeters : MonoBehaviour
{
    public GameObject character;
    public GameObject HP, MP;
    public GameObject HpBar, MpBar;

    public void Update()
    {
        int hp = character.GetComponent<CharacterInfo>().HealthPoint;
        int MaxHP = character.GetComponent<CharacterInfo>().MaxHealthPoint;
        int mp = character.GetComponent<CharacterInfo>().ManaPoint;
        int MaxMP = character.GetComponent<CharacterInfo>().MaxManaPoint;
      
        HP.GetComponent<TextMeshProUGUI>().text = hp.ToString();
        HpBar.GetComponent<Image>().fillAmount = (float)hp / MaxHP;

        MP.GetComponent<TextMeshProUGUI>().text = mp.ToString();
        MpBar.GetComponent<Image>().fillAmount = (float)mp / MaxMP;
    }
}
