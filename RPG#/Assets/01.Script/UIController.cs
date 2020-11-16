using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject[] Players;       //플레이어 캐릭터 담을 변수
    private GameObject[] Enemies;       //적 캐릭터 담을 변수
    public int SelectedCharacter;       //현재 선택된 캐릭터

    public GameObject explainPanel;     //스킬설명패널
    public TextMeshProUGUI explains;    //스킬설명을 담을 텍스트

    public GameObject MasterButton;     //스킬버튼 최상위 게임오브젝트
    public GameObject[] Buttons;        //스킬 및 적 선택 버튼
    public TextMeshProUGUI[] ButtonsText;

    public GameObject MasterText;           //선택되어있는 스킬을 보여주는 글 최상위 게임오브젝트
    public TextMeshProUGUI[] SelectedSkill; //선택되어있는 스킬을 보여주는 글

    public GameObject MasterEnemy;          //선택되어있는 적을 보여주는 최상위 게임오브젝트       
    public GameObject[] SelectedEnemy;      //선택되어있는 적을 보여주는 게임오브젝트

    public GameObject ClickElsewhere;       //캐릭터를 선택한 상태에서 다른곳을 클릭했을때;

    public GameObject DisableOnAction;
    public GameObject ActionButton;

    public GameObject[] StatusGauge;
    public GameObject[] ExpGauge;

    public GameObject[] PlayerButtons;

    public GameObject HPSkillText;
    public GameObject MPSkillText;

    public AudioManager SFXm;

    public GameObject WinPanel, LosePanel;    //승패 결과에 따른 결과 화면;

    private void Awake()
    {
        SFXm = FindObjectOfType<AudioManager>();
        Players = GameObject.FindGameObjectsWithTag("Player");
        Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        SelectedEnemy = GameObject.FindGameObjectsWithTag("Aims");
        DisableOnAction = GameObject.Find("DisableOnAction");
        ActionButton = GameObject.Find("Action");
        StatusGauge = GameObject.FindGameObjectsWithTag("StatusGauge");
        ExpGauge = GameObject.FindGameObjectsWithTag("ExpGauge");
        PlayerButtons = GameObject.FindGameObjectsWithTag("PlayerButton");
        HPSkillText = GameObject.Find("HPSkill");
        MPSkillText = GameObject.Find("MPSkill");

        MasterEnemy.SetActive(false);
        for(int i = 0; i < ExpGauge.Length; i++)
        {
            ExpGauge[i].SetActive(false);
        }

        LoadFirstSkill();
    }

    public void LoadFirstSkill()
    {
        for(int i = 0; i < Players.Length; i++)
        {
            if(Players[i].GetComponent<CharacterInfo>().CharacterName == "HP")
            {
                SelectedSkill[0].text = Players[i].GetComponent<CharacterInfo>().Skill[0];
            }
            else if(Players[i].GetComponent<CharacterInfo>().CharacterName == "MP")
            {
                SelectedSkill[1].text = Players[i].GetComponent<CharacterInfo>().Skill[0];
            }
        }        
    }

    public void SelectCharacter(string name)
    {
        SFXm.Play("Switch1");
        for(int i = 0; i < Players.Length; i++)
        {
            if(name == Players[i].GetComponent<CharacterInfo>().CharacterName)
            {
                SelectedCharacter = i;
                ShowSkillList();
                break;
            }
        } 
    }

    public void SelectEnemy(string name)
    {
        SFXm.Play("Switch1");
        for (int i = 0; i < Enemies.Length; i++)
        {
            if(name == Enemies[i].GetComponent<CharacterInfo>().CharacterName)
            {
                Players[SelectedCharacter].GetComponent<CharacterInfo>().target = i;
            }
        }

        for(int i = 0; i < SelectedEnemy.Length; i++)
        {
            if(name == SelectedEnemy[i].GetComponent<AimsName>().AimName)
            {
                SelectedEnemy[i].GetComponent<Image>().color = Color.white;
            }
            else
            {
                SelectedEnemy[i].GetComponent<Image>().color = Color.clear;
            }
        }            
    }

    
    public void ShowSkillList()
    {
        MasterText.SetActive(false);                //일단 모든 버튼을 숨겼다가
        MasterButton.SetActive(false);              
        MasterEnemy.SetActive(false);
        ClickElsewhere.SetActive(true);

        for(int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].SetActive(false);
        }

        for(int i = 0; i < Players[SelectedCharacter].GetComponent<CharacterInfo>().Skill.Length; i++)      //다시 생성하며 보여줌
        {
            Buttons[i].SetActive(true);
            ButtonsText[i].text = Players[SelectedCharacter].GetComponent<CharacterInfo>().Skill[i];
            if(Players[SelectedCharacter].GetComponent<CharacterInfo>().SkillMana[i] > Players[SelectedCharacter].GetComponent<CharacterInfo>().ManaPoint)
            {
                Buttons[i].GetComponent<Button>().interactable = false;
                ButtonsText[i].color = Color.gray;
            }
            else
            {
                Buttons[i].GetComponent<Button>().interactable = true;
                ButtonsText[i].color = Color.white;
            }
        }

        for(int i = 0; i < Players[SelectedCharacter].GetComponent<CharacterInfo>().targets.Length; i++)
        {
            if (Players[SelectedCharacter].GetComponent<CharacterInfo>().targets[Players[SelectedCharacter].GetComponent<CharacterInfo>().target].GetComponent<CharacterInfo>().CharacterName == SelectedEnemy[i].GetComponent<AimsName>().AimName)
            {
                SelectedEnemy[i].GetComponent<Image>().color = Color.white;
            }
            else
            {
                SelectedEnemy[i].GetComponent<Image>().color = Color.clear;
            }
        }
        SFXm.Play("Switch1");
        MasterEnemy.SetActive(true);
        MasterButton.SetActive(true);
    }

    public void SelectSkill(int i)
    {
        SFXm.Play("Switch1");
        MasterButton.SetActive(false);

        if(Players[SelectedCharacter].GetComponent<CharacterInfo>().CharacterName == "HP")
        {
            Players[SelectedCharacter].GetComponent<CharacterInfo>().SkillNumber = i;
            SelectedSkill[0].text = ButtonsText[i].text;
        }
        else if (Players[SelectedCharacter].GetComponent<CharacterInfo>().CharacterName == "MP")
        {
            Players[SelectedCharacter].GetComponent<CharacterInfo>().SkillNumber = i;
            SelectedSkill[1].text = ButtonsText[i].text;
        }

        ClickElsewhere.SetActive(false);
        explainPanel.SetActive(false);
        MasterEnemy.SetActive(false);
        MasterText.SetActive(true);
    }

    public void ShowSelectedSkill()
    {
        MasterText.SetActive(true);
        ClickElsewhere.SetActive(false);
        MasterButton.SetActive(false);
        MasterEnemy.SetActive(false);
        explainPanel.SetActive(false);
    }

    public void ShowExplain(int i)
    {
        explains.text = Players[SelectedCharacter].GetComponent<CharacterInfo>().SkillExplain[i];
        explainPanel.SetActive(true);
    }

    public void HideExplain()
    {
        explainPanel.SetActive(false);
    }

    public void ActionUIShow()
    {
        DisableOnAction.SetActive(true);
        ActionButton.SetActive(true);
        for (int i = 0; i < PlayerButtons.Length; i++)
        {
            for(int j = 0; j < Players.Length; j++)
            {
                if ((Players[j].GetComponent<CharacterInfo>().HealthPoint > 0) && Players[j].GetComponent<CharacterInfo>().CharacterName == PlayerButtons[i].GetComponent<AimsName>().AimName)
                {
                    PlayerButtons[i].GetComponent<Button>().interactable = true;
                    if (Players[j].GetComponent<CharacterInfo>().CharacterName == "HP")
                    {
                        Players[j].GetComponent<CharacterInfo>().SkillNumber = 0;
                        HPSkillText.GetComponent<TextMeshProUGUI>().text = "Sword Attack";
                    }
                    if (Players[j].GetComponent<CharacterInfo>().CharacterName == "MP")
                    {
                        Players[j].GetComponent<CharacterInfo>().SkillNumber = 0;
                        MPSkillText.GetComponent<TextMeshProUGUI>().text = "Staff Attack";
                    }
                }
                else if (Players[j].GetComponent<CharacterInfo>().HealthPoint <= 0)
                {
                    if (Players[j].GetComponent<CharacterInfo>().CharacterName == "HP")
                    {
                        HPSkillText.GetComponent<TextMeshProUGUI>().text = " ";
                    }
                    if (Players[j].GetComponent<CharacterInfo>().CharacterName == "MP")
                    {
                        MPSkillText.GetComponent<TextMeshProUGUI>().text = " ";
                    }
                }
            }
        }
    }

    public void ActionUIHide()
    {
        SFXm.Play("Action");
        DisableOnAction.SetActive(false);
        ActionButton.SetActive(false);
        for(int i = 0; i < PlayerButtons.Length; i++)
        {
            PlayerButtons[i].GetComponent<Button>().interactable = false;
        }
    }

    public void Win()
    {
        WinPanel.SetActive(true);
        for (int i = 0; i < ExpGauge.Length; i++)
        {
            ExpGauge[i].SetActive(true);
        }
        for (int i = 0; i < StatusGauge.Length; i++)
        {
            StatusGauge[i].SetActive(false);
        }
        for(int i = 0; i < Players.Length; i++)
        {
            Players[i].GetComponent<CharacterInfo>().GainExp();
        }
    }

    public void Lose()
    {
        LosePanel.SetActive(true);

    }
}
