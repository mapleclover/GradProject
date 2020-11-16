using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBattleControl : MonoBehaviour
{
    public float EnemyHP;
    public float FullEnemyHP;
    public GameObject[] Player;
    public int speed;
    public int AttackPower;

    public GameObject ActionButton;

    public GameObject EnemyStatus;
    public GameObject EnemyHpBar;
    public GameObject EnemyName;

    public void Damaged(int i)
    {

        EnemyHP -= i;

        if (EnemyHP <= 0)
        {
            EnemyHP = 0;
            EnemyDied();
        }
        else
        { 
            this.GetComponent<Animator>().SetTrigger("GetHit"); 
        }

    }

    public void Attack()
    {
        if(EnemyHP> 0)
        {
            StartCoroutine(AttackSequence());
        }
    }
    public void EnemyDied()
    {
        StopAllCoroutines();
        //EnemyStatus.SetActive(false);
        
        this.GetComponent<Animator>().SetTrigger("Died");
    }

    IEnumerator AttackSequence()
    {
        yield return new WaitForSeconds(1.0f);

        Vector3 OriginalPosition = this.transform.position;
        Vector3 PlayerPosition = Player[0].transform.position + new Vector3(-.5f, 0, 1f);
        Vector3 Move = (PlayerPosition - OriginalPosition) / 100;
        this.GetComponent<Animator>().SetBool("Run", true);
        for (int j = 0; j < 100; j++)
        {
            this.gameObject.transform.Translate(Move, Space.World);
            yield return null;
        }
        this.GetComponent<Animator>().SetBool("Run", false);

        yield return new WaitForSeconds(1f);

        this.GetComponent<Animator>().SetTrigger("Attack");
        Player[0].GetComponent<PlayerBattleControl>().Damaged(AttackPower);

        yield return new WaitForSeconds(2f);

        this.GetComponent<Animator>().SetBool("Run", true);

        for (int j = 0; j < 100; j++)
        {
            this.gameObject.transform.Translate(-Move, Space.World);
            yield return null;
        }
        this.GetComponent<Animator>().SetBool("Run", false);
    }
    private void Update()
    {
        EnemyHpBar.GetComponent<Image>().fillAmount = EnemyHP / FullEnemyHP;
    }
}
