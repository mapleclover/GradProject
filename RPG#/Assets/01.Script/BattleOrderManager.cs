using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BattleOrderManager : MonoBehaviour
{
    private List<CharacterInfo> characters;
    private UIController UIController;
    public List<string[]> orders = new List<string[]>();
    public Stack<string> Actions = new Stack<string>();
    private BGMManager BGMPlayer;

    private void Start()
    {
        UIController = FindObjectOfType<UIController>();
        BGMPlayer = FindObjectOfType<BGMManager>();
    }
    public void PreLoadCharacter()
    {
        characters = ToList();
        
        foreach(var value in characters)//Debug
        {
            Debug.Log(value);
        }
    }
    public List<CharacterInfo> ToList()
    {
        List<CharacterInfo> tempList = new List<CharacterInfo>();
        CharacterInfo[] temps = FindObjectsOfType<CharacterInfo>();

        for(int i = 0; i < temps.Length; i++)
        {
            tempList.Add(temps[i]);
        }

        return tempList;
    }

    public int CompareString(string[] a, string[] b)
    {
        int aNum = Convert.ToInt32(a[1]);
        int bNum = Convert.ToInt32(b[1]);

        return aNum.CompareTo(bNum);
    }

    public void Action()
    {
        UIController.ActionUIHide();

        PreLoadCharacter();

        for (int i = 0; i < characters.Count; i++)
        {
            string Name = characters[i].CharacterName;
            string Speed = ((int)(characters[i].Speed + characters[i].Speed * UnityEngine.Random.Range(-0.1f, 0.1f))).ToString();
            orders.Add(new string[] { Name, Speed });

            Debug.Log(" 오브젝트 : " + Name + " / 속도 : " + Speed);
        }

        orders.Sort(CompareString);//내림차순 정렬

        Debug.Log("정렬완료");

        foreach (string[] value in orders)//Debug
        {
            Debug.Log(value[0] + " " + value[1]);
            Actions.Push(value[0]);
        }

        StartCoroutine(OrderStart());
    }

    IEnumerator OrderStart()
    {
        while (Actions.Count != 0)
        {
            if(CheckWin() || CheckLose())
            {
                break;
            }

            for (int i = 0; i < characters.Count; i++)
            {
                if (characters[i].CharacterName == Actions.Peek())
                {
                    int k = Actions.Count - 1;
                    characters[i].Action(Actions);
                    yield return new WaitUntil(() => k == Actions.Count);
                    i = characters.Count;
                    Debug.Log("남은 액션 수 : " + Actions.Count);
                }
            }
        }

        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].isPlayer)
            {
                if (characters[i].fireShield)
                {
                    characters[i].fireShield = false;
                }
            }
        }

        if (!CheckWin() && !CheckLose())
        {
            UIController.ActionUIShow();
            for (int i = 0; i < characters.Count; i++)
            {
                if (characters[i].isPlayer)
                {
                    characters[i].checkTargetStatus();
                }
            }
        }
        else
        {
            if(CheckWin())
            {
                BGMPlayer.Play(4);
                Debug.Log("승리!");
                UIController.Win();
            }
            else if(CheckLose())
            {
                BGMPlayer.Play(5);
                Debug.Log("패배!");
                UIController.Lose();
            }
        }

        orders.Clear();
        characters.Clear();
    }

    public bool CheckWin()
    {
        for(int i = 0; i < characters.Count; i++)
        {
            if (!characters[i].isPlayer && characters[i].HealthPoint > 0)
            {
                return false;
            }
        }

        return true;
    }
    public bool CheckLose()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].isPlayer && characters[i].HealthPoint > 0)
            {
                return false;
            }
        }
        return true;
    }
}