using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleControl : MonoBehaviour
{
    public List<string> characterChoice;
    public List<string> battleChoice;
    public List<string> skillChoice;
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

    public GameObject AT, HT, LW;
    public GameObject[] ATs;
    public GameObject[] HTs;
    public GameObject[] LWs;
    public bool ATt, HTt, LWt;

    State state;

    public int NumOfChoice = 4;
    public Button[] Choices;
    public TextMeshProUGUI[] textChoices;
    public Button PlayerButton;

    public GameObject Player;
    public GameObject Enemy;

    public GameObject ActionButton;
    public GameObject GameOver;
    public GameObject Victory;

    bool Q1, Q2 = false;

    public bool SkillRemember = false;

    // Start is called before the first frame update
    void Start()
    {
        //playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        for (int i = 0; i < NumOfChoice; i++)
        {
            textChoices[i] = Choices[i].GetComponentInChildren<TextMeshProUGUI>();
        }
        state = State.Character;
        ATt = true;
        AT.SetActive(true);
        HTt = false;
        LWt = false;
        ShowChoice();
    }
    private void OnEnable()
    {
        if(Player.GetComponent<PlayerBattleControl>().PlayerHP <= 0)
        {
            ActionButton.SetActive(false);
            GameOver.SetActive(true);
        }
        else
        {
            state = State.Character;
            ShowChoice();

            if (HTt)
            {
                HT.SetActive(true);
            }
            if (LWt)
            {
                LW.SetActive(true);
                if (LWs[0].activeSelf)
                {
                    LWs[0].SetActive(false);
                    LWs[1].SetActive(true);
                }
                else if (!LWs[0].activeSelf)
                {
                    LWs[0].SetActive(true);
                    LWs[1].SetActive(false);
                }
            }
        }


    }

    public void ChangeState(int i)
    {
        if(i == 90) //textChoices[i].text == "Player" && state == State.Character
        {
            if(ATt)
            {
                ATs[0].SetActive(false);
                ATs[1].SetActive(true);
                ATs[2].SetActive(false);
                ATs[3].SetActive(false);
                ATs[4].SetActive(false);
            }
            if(HTt)
            {
                HTs[0].SetActive(false);
                HTs[1].SetActive(true);
                HTs[2].SetActive(false);
                HTs[3].SetActive(false);
            }
            state = State.Battle;
            ShowChoice();
        }
        else if(textChoices[i].text == "Attack" && state == State.Battle)
        {
            if (ATt)
            {
                ATs[1].SetActive(false);
                ATs[2].SetActive(true);
            }
            if (HTt)
                return;
            
            state = State.Skill;
            ShowChoice();
        }
        else if(textChoices[i].text == "Item")
        {
            if (ATt || LWt)
                return;
            if (HTt)
            {
                HTs[1].SetActive(false);
                HTs[2].SetActive(true);
            }
            state = State.Item;
            ShowChoice();
        }
        else if((textChoices[i].text == "Sword Attack" || textChoices[i].text == "X-Slash") && state == State.Skill)//MP부족시 구현해야함
        {
            if (ATt && textChoices[i].text == "X-Slash")
                return;
            else if(ATt && textChoices[i].text == "Sword Attack")
            {
                ATs[2].SetActive(false);
                ATs[3].SetActive(true);
            }
            if (LWt && textChoices[i].text == "Sword Attack")
                return;
            if (i == 0)
            {
                characterChoice[0] = "Sword Attack";
                Player.GetComponent<PlayerBattleControl>().AttackType = 0;
                Q1 = true;
            }                
            else if( i == 1)
            {
                characterChoice[0] = "X-Slash";
                Player.GetComponent<PlayerBattleControl>().AttackType = 1;
                Q2 = true;
            }
            state = State.Enemy;
            ShowChoice();
        }
        else if(textChoices[i].text == "Slime")
        {
            ATs[3].SetActive(false);
            ATs[4].SetActive(true);
            state = State.Character;
            //Player.GetComponent<PlayerBattleControl>().attack = true;
            //Player.GetComponent<PlayerBattleControl>().heal = false;
            ShowChoice();
        }
        else if (textChoices[i].text == "Potion")
        {
            HTs[2].SetActive(false);
            HTs[3].SetActive(true);
            characterChoice[0] = "Potion";
            state = State.Character;
            //Player.GetComponent<PlayerBattleControl>().Heal(100);
            //ActionButton.SetActive(false);
            //Player.GetComponent<PlayerBattleControl>().attack = false;
            //Player.GetComponent<PlayerBattleControl>().heal = true;
            ShowChoice();
        }
        else if(textChoices[i].text == "Back")
        {
            /*
            if (state == State.Battle)
                state = State.Character;
            else if (state == State.Skill)
                state = State.Battle;
            else if (state == State.Enemy)
                state = State.Skill;
            else if (state == State.Item)
                state = State.Battle;
            ShowChoice();
            */
        }
    }
    
    public void TakeAction()
    {
        if(state == State.Character)
        {
            if (characterChoice[0] == "Potion")
            {
                if (HTt)
                {
                    HTt = false;
                    HT.SetActive(false);
                    LWt = true;
                }
                Player.GetComponent<PlayerBattleControl>().Heal(100);
                characterChoice[0] = "Sword Attack";
                ActionButton.SetActive(false);
            }
            else
            {
                if(Q1 == true || Q2 == true)
                {
                    if (ATt)
                    {
                        ATt = false;
                        AT.SetActive(false);
                        HTt = true;
                    }
                    if (LWt)
                        LW.SetActive(false);

                    Player.GetComponent<PlayerBattleControl>().Attack();
                    Q1 = Q2 = false;
                    if(!SkillRemember)
                    {
                        characterChoice[0] = "Sword Attack";
                    }
                    ActionButton.SetActive(false);
                }                
            }
        }
        else
        {
            state = State.Character;
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
                    if (i < skillChoice.Count)
                    {
                        textChoices[i].text = skillChoice[i];
                        Choices[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        Choices[i].gameObject.SetActive(false);
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
    /*
    public void takeAction() //속도에 따라 선공이 정해지는 부분
    {
        Player.GetComponent<PlayerBattleControl>().speed += (int)(Player.GetComponent<PlayerBattleControl>().speed / 10 * Random.Range(-1.0f, 1.0f));
        Enemy.GetComponent<PlayerBattleControl>().speed += (int)(Player.GetComponent<PlayerBattleControl>().speed / 10 * Random.Range(-1.0f, 1.0f));
    }
    */
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