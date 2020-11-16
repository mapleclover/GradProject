using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMeters : MonoBehaviour
{
    public GameObject character;
    public GameObject HpBar;

    public void Update()
    {
        int hp = character.GetComponent<CharacterInfo>().HealthPoint;
        int MaxHP = character.GetComponent<CharacterInfo>().MaxHealthPoint;

        HpBar.GetComponent<Image>().fillAmount = (float)hp / MaxHP;
    }
}
