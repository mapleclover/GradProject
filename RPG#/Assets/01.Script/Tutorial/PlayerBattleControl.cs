using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBattleControl : MonoBehaviour
{
    public GameObject[] Enemy;

    public GameObject Target;
    public GameObject Victory;

    public bool movable = true;

    public float PlayerHP;
    public float FullPlayerHP;
    public float PlayerMP;
    public float FullPlayerMP;
    public int AttackType = 0;
    public float speed;
    public int AttackPower;
    public int SpecialAttackMP;

    public GameObject PlayerStatus;
    public GameObject PlayerHpBar;
    public GameObject PlayerMpBar;

    //public bool attack, heal;

    void Start()
    {
        Target = Enemy[0];
    }

    public void ChangeTarget(int i)
    {
        Target = Enemy[i];
    }

    public void Attack()
    {
        if(PlayerHP > 0)
            StartCoroutine(AttackSequence());   
    }

    public void Heal(int i)
    {
        PlayerHP += i;
        if (PlayerHP > FullPlayerHP)
            PlayerHP = FullPlayerHP;

        Target.GetComponent<EnemyBattleControl>().Attack();
    }
    public void ManaHeal(int i)
    {
        PlayerMP += i;
        if (PlayerMP > FullPlayerMP)
            PlayerMP = FullPlayerMP;

        Target.GetComponent<EnemyBattleControl>().Attack();
    }

    public void Damaged(int i)
    {
        PlayerHP -= i;
        if (PlayerHP <= 0)
        {
            PlayerHP = 0;
            PlayerDied();
        }
        else
        {
            this.GetComponent<Animator>().SetTrigger("GetHit");
        }
    }

    void PlayerDied()
    {
        StopAllCoroutines();
        //PlayerStatus.SetActive(false);
        this.GetComponent<Animator>().SetTrigger("Died");
    }

    IEnumerator AttackSequence()
    {
        yield return new WaitForSeconds(1.0f);

        Vector3 OriginalPosition = this.transform.position;
        Vector3 EnemyPosition = Target.transform.position + new Vector3(.5f, 0, -1f);
        Vector3 Move = (EnemyPosition - OriginalPosition) / 100;
        this.GetComponent<Animator>().SetBool("Run", true);
        this.GetComponent<Animator>().SetBool("Idle", false);
        for (int j = 0; j < 100; j++)
        {
            this.gameObject.transform.Translate(Move, Space.World);
            yield return null;
        }
        this.GetComponent<Animator>().SetBool("Idle", true);
        this.GetComponent<Animator>().SetBool("Run", false);

        yield return new WaitForSeconds(1f);

        if (AttackType == 0)
        {
            this.GetComponent<Animator>().SetTrigger("NormalAtt");
            Target.GetComponent<EnemyBattleControl>().Damaged(AttackPower);
        }
        else if(AttackType == 1)
        {
            this.GetComponent<Animator>().SetTrigger("SpecialAtt");
            PlayerMP -= SpecialAttackMP;
            Target.GetComponent<EnemyBattleControl>().Damaged(AttackPower * 2);
        }
        else if(AttackType == 10)//힐
        {
            Heal(100);
        }

        yield return new WaitForSeconds(1f);

        this.GetComponent<Animator>().SetBool("Run", true);
        this.GetComponent<Animator>().SetBool("Idle", false);
        for (int j = 0; j < 100; j++)
        {
            this.gameObject.transform.Translate(-Move, Space.World);
            yield return null;
        }
        this.GetComponent<Animator>().SetBool("Idle", true);
        this.GetComponent<Animator>().SetBool("Run", false);

        if (checkWin())
        {
            Victory.SetActive(true);
            PlayerHP = 120;
            FullPlayerHP = 120;
            PlayerMP = 65;
            FullPlayerMP = 65;
            StopAllCoroutines();
        }
    }

    public bool checkWin()
    {
        for(int i = 0; i < Enemy.Length; i++)
        {
            if (Enemy[i].GetComponent<EnemyBattleControl>().EnemyHP > 0)
                return false;
        }
        return true;
    }
    private void Update()
    {
        PlayerHpBar.GetComponent<Image>().fillAmount = PlayerHP /FullPlayerHP;
        PlayerMpBar.GetComponent<Image>().fillAmount = PlayerMP /FullPlayerMP;
    }
}