using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainBattleControl : MonoBehaviour
{
    public List<string> characterChoice;
    public List<string> battleChoice;
    public List<string> skillChoice1, skillChoice2;
    public List<string> enemyChoice;
    public List<string> itemChoice;

    enum State
    {
        Character,
        Battle,
        Skill,
        Enemy,
        Item
    }

    State state;

    public int NumOfChoice = 4;
    public Button[] Choices;
    public TextMeshProUGUI[] textChoices;

    public GameObject[] Player;
    public GameObject[] Enemy;

    public GameObject ActionButton;
    public GameObject GameOver;
    public GameObject Victory;

    public int selectedCharacter;

    public bool SkillRemember = false;

    public GameObject battleStart;

    void Start()
    {
        for (int i = 0; i < NumOfChoice; i++)
        {
            textChoices[i] = Choices[i].GetComponentInChildren<TextMeshProUGUI>();
        }
        state = State.Character;
        ShowChoice();
    }
    private void OnEnable()
    {
        int count = 0;
        for(int i = 0; i <2; i++)
        {
            if (Player[i].GetComponent<PlayerBattleControl>().PlayerHP <= 0)
            {
                count++;
            }
        }
        if (count == 2)
        {
            ActionButton.SetActive(false);
            GameOver.SetActive(true);
        }
        else
        {
            state = State.Character;
            ShowChoice();
        }


    }

    public void SelectCharacter(int i)
    {
        if (i == 90)
        {
            state = State.Battle;
            selectedCharacter = 0;
            ShowChoice();
        }
        else if (i == 91)
        {
            state = State.Battle;
            selectedCharacter = 1;
            ShowChoice();
        }
    }

    public void ChangeState(int i)
    {        
        if (textChoices[i].text == "Attack" && state == State.Battle)
        {
            state = State.Skill;
            ShowChoice();
        }
        else if (textChoices[i].text == "Item")
        {
            state = State.Item;
            ShowChoice();
        }
        else if (textChoices[i].text != "Back" && state == State.Skill)
        {
            if((Player[selectedCharacter].GetComponent<PlayerBattleControl>().PlayerMP < 10.0f && textChoices[i].text == "X-Slash")
                || (Player[selectedCharacter].GetComponent<PlayerBattleControl>().PlayerMP < 12.0f && textChoices[i].text == "Fire Bolt"))
            {
                state = State.Skill;
            }
            else
            {
                characterChoice[selectedCharacter] = textChoices[i].text;
                Player[selectedCharacter].GetComponent<PlayerBattleControl>().AttackType = i;

                state = State.Enemy;
                ShowChoice();
            }
        }
        else if (textChoices[i].text == "Slime")
        {
            Player[selectedCharacter].GetComponent<PlayerBattleControl>().ChangeTarget(0);
            state = State.Character;
            ShowChoice();
        }
        else if (textChoices[i].text == "Lich")
        {
            Player[selectedCharacter].GetComponent<PlayerBattleControl>().ChangeTarget(1);
            state = State.Character;
            ShowChoice();
        }
        else if (textChoices[i].text == "Potion")
        {
            Player[selectedCharacter].GetComponent<PlayerBattleControl>().AttackType = 10;
            characterChoice[selectedCharacter] = "Potion";
            state = State.Character;
            ShowChoice();
        }
        else if (textChoices[i].text == "Back")
        {
            if (state == State.Battle)
                state = State.Character;
            else if (state == State.Skill)
                state = State.Battle;
            else if (state == State.Enemy)
                state = State.Skill;
            else if (state == State.Item)
                state = State.Battle;
            ShowChoice();
        }
    }

    void ShowChoice()
    {
        switch (state)
        {
            case State.Character:
                for (int i = 0; i < NumOfChoice; i++)
                {
                    if (i < characterChoice.Count)
                    {
                        textChoices[i].text = characterChoice[i];
                        Choices[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        Choices[i].gameObject.SetActive(false);
                    }
                }
                break;
            case State.Battle:
                for (int i = 0; i < NumOfChoice; i++)
                {
                    if (i < battleChoice.Count)
                    {
                        textChoices[i].text = battleChoice[i];
                        Choices[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        Choices[i].gameObject.SetActive(false);
                    }
                }
                break;
            case State.Skill:
                for (int i = 0; i < NumOfChoice; i++)
                {
                    if (selectedCharacter == 0)
                    {
                        if (i < skillChoice1.Count)
                        {
                            textChoices[i].text = skillChoice1[i];
                            Choices[i].gameObject.SetActive(true);
                        }
                        else
                        {
                            Choices[i].gameObject.SetActive(false);
                        }
                    }
                    else if (selectedCharacter == 1)
                    {
                        if (i < skillChoice2.Count)
                        {
                            textChoices[i].text = skillChoice2[i];
                            Choices[i].gameObject.SetActive(true);
                        }
                        else
                        {
                            Choices[i].gameObject.SetActive(false);
                        }
                    }
                }
                break;
            case State.Enemy:
                for (int i = 0; i < NumOfChoice; i++)
                {
                    if (i < enemyChoice.Count)
                    {
                        textChoices[i].text = enemyChoice[i];
                        Choices[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        Choices[i].gameObject.SetActive(false);
                    }
                }
                break;
            case State.Item:
                for (int i = 0; i < NumOfChoice; i++)
                {
                    if (i < itemChoice.Count)
                    {
                        textChoices[i].text = itemChoice[i];
                        Choices[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        Choices[i].gameObject.SetActive(false);
                    }
                }
                break;
        }
    }

    public void TakeAction()
    {
        if (state == State.Character)
        {
            if (!SkillRemember)
            {
                characterChoice[0] = "Sword Attack";
                characterChoice[1] = "Staff Attack";
            }
            //battleStart.GetComponent<BattleAlgorithm>().StartAlgo();

            ActionButton.SetActive(false);
        }
        else
        {
            state = State.Character;
            ShowChoice();
        }
    }
   
    public void ClickRetry()
    {
        SceneManager.LoadScene(2);
    }
    public void ClickTitle()
    {
        SceneManager.LoadScene(0);
    }
    public void NextStage()
    {
        SceneManager.LoadScene(3);
    }
}