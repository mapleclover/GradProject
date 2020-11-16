using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    public string CharacterName;

    public int Level;               //현재 레벨
    public int AccquiredExperience; //얻은경험치
    public int Experience;          //현재경험치
    public int RequiredExperience;  //레벨업까지 필요한 경험치

    public int HealthPoint;         //체력
    public int MaxHealthPoint;      //최대 체력

    public int ManaPoint;           //마력
    public int MaxManaPoint;        //최대 마력

    public int Attack;              //공격력
    public int Speed;               //속도
    public int Luck;                //행운

    public string[] Skill;          //스킬리스트
    public int[] SkillMana;         //스킬소모마나
    public int[] SkillHitCount;     //스킬타격횟수
    public float[] SkillMultiplier; //스킬데미지 배율
    public int[] SkillDistance;     //True = 원거리; False = 근거리;
    public bool[] SkillArea;        //True = 광역; False = 단일;
    public string[] SkillExplain;   //스킬설명
    public int SkillNumber;         //선택된 스킬

    private bool isCrit;             //맞은 공격이 크리티컬이 였는가?

    public bool isPlayer;           //적인가 아군인가
    public bool isBoss;             
    public GameObject[] targets;    //적대적 대상
    public int target;              //공격하려는 대상

    public bool isFaint;            //HP가 0인가?

    private AudioManager SFXm;

    public GameObject[] SkillEffects;

    private int steps;

    public bool fireShield;

    private static FloatingDamage popupDamage;
    private static FloatingDamage popupCritDamage;
    private static FloatingDamage popupHeal;
    private static GameObject canvas;

    private GameObject Warning;
    public Transform DragonsMouth;

    public static void Initialize()
    {
        canvas = GameObject.Find("Canvas");
        popupDamage = Resources.Load<FloatingDamage>("prefabs/ShowDamage");
        popupCritDamage = Resources.Load<FloatingDamage>("prefabs/ShowCritDamage");
        popupHeal = Resources.Load<FloatingDamage>("prefabs/ShowHeal");
    }

    public static void CreateFloatingDamage(int damage, bool isCritical,Transform location)
    {
        Initialize();
        FloatingDamage instance;
        instance = Instantiate(popupDamage);
        if (isCritical)
        {
             instance = Instantiate(popupCritDamage);
        }

        Vector2 screenPosition = Camera.main.WorldToScreenPoint(location.position);
        instance.transform.SetParent(canvas.transform, false);
        instance.transform.position = new Vector2(screenPosition.x + Random.Range(-15f, 15f), screenPosition.y + 100.0f + Random.Range(-15f, 15f));
        instance.SetText(damage.ToString());
    }

    private void Start()
    {
        isFaint = false;
        SFXm = FindObjectOfType<AudioManager>();

        if(isPlayer)
        {
            targets = GameObject.FindGameObjectsWithTag("Enemy");
            RequiredExperience = Level * 100;
            target = 0;
        }
        else
        {
            targets = GameObject.FindGameObjectsWithTag("Player");
            target = 0;
            if(isBoss)
            {
                Warning = GameObject.Find("Warning");
                Warning.SetActive(false);
            }
        }
    }

    public void LevelUp()           //레벨업
    {
        FloatingDamage instance;
        instance = Instantiate(popupHeal);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        instance.transform.SetParent(canvas.transform, false);
        instance.transform.position = new Vector2(screenPosition.x, screenPosition.y + 150.0f);
        SFXm.Play("LevelUp");
        Experience = 0;
        Level++;
        RequiredExperience = Level * 100;
        MaxHealthPoint += 50;
        MaxManaPoint += 20;
        HealthPoint = MaxHealthPoint;
        ManaPoint = MaxManaPoint;
        Attack += 10;
        Speed += 5;
        Luck += 5;
    }

    public void GainExp()           //경험치 획득
    {
        StartCoroutine(GainingExp());
    }

    IEnumerator GainingExp()        
    {
        for(int i = 0; i < AccquiredExperience; i++)
        {
            Experience++;
            if(Experience >= RequiredExperience)
            {
                LevelUp();
            }
            yield return null;
        }
        AccquiredExperience = 0;
    }

    public void Damage(int damage, bool isCritical)  //피격
    {
        GetComponent<Animator>().SetTrigger("GetHit");
        if (fireShield)
        {
            damage /= 99;
        }
        CreateFloatingDamage(damage, isCritical, transform);

        StartCoroutine(Damaged(damage));
    }

    IEnumerator Damaged(int damage)
    {
        for(int i = 0; i < damage; i++)
        {
            HealthPoint--;
            if(HealthPoint <= 0)
            {
                HealthPoint = 0;
                isFaint = true;
                SFXm.Play("Faint");
                if(!isPlayer)
                {
                    for (int j = 0; j < targets.Length; j++)
                    {
                        targets[j].GetComponent<CharacterInfo>().AccquiredExperience += Experience;
                    }
                    Experience = 0;
                }
                GetComponent<Animator>().SetTrigger("Died");
                break;
            }
            yield return null;
        }
    }

    public void UseMana(int ManaUsage)
    {
        StartCoroutine(UsedMana(ManaUsage));
    }

    IEnumerator UsedMana(int ManaUsage)
    {
        for (int i = 0; i < ManaUsage; i++)
        {
            ManaPoint--;
            yield return null;
        }
    }

    public void ChangeTarget(int i)
    {
        target = i;
    }


    public void Action(Stack<string> i)
    {
        if(!isFaint)
        {
            StartCoroutine(TakeAction(i));
        }
        else
        {
            Debug.Log(i.Pop() + " 행동 불가!");
        }
    }
    
    public void checkTargetStatus()
    {
        while (targets[target].GetComponent<CharacterInfo>().isFaint == true)//타겟이 아직도 공격 가능한 상태인지 확인 선택했던 타켓이 이미 기절상태라면 다음 타켓으로 자동이동
        {
            target++;
            if(target>=targets.Length)
            {
                target = 0;
            }
        }
    }

    IEnumerator TakeAction(Stack<string> i)
    {
        checkTargetStatus();

        CharacterInfo targetEnemy = targets[target].GetComponent<CharacterInfo>();
        Vector3 OriginalPosition = this.transform.position;
        Vector3 TargetPosition = new Vector3(targets[target].transform.position.x, OriginalPosition.y, targets[target].transform.position.z);
        Vector3 Move = (TargetPosition - OriginalPosition) / 100;

        if(!isPlayer && !isBoss)
        {
            SkillNumber = Random.Range(0, Skill.Length);
            target = Random.Range(0, targets.Length);
        }
        else if(isBoss)
        {
            target = Random.Range(0, targets.Length);
            SkillNumber++;
            if(SkillNumber >= Skill.Length)
            {
                SkillNumber = 0;
            }
        }

        if(this.targets[target].GetComponent<CharacterInfo>().isBoss)
        {
            steps = 40;
        }
        else
        {
            steps = 80;
        }
        if (SkillDistance[SkillNumber] == 0)
        {
            Debug.Log(CharacterName + "이" + targetEnemy.CharacterName + "에게 이동!");//적에게 이동
            GetComponent<Animator>().SetBool("Run", true);
            for (int j = 0; j < steps; j++)
            {
                this.gameObject.transform.Translate(Move, Space.World);
                yield return null;
            }
            GetComponent<Animator>().SetBool("Run", false);
            yield return new WaitForSeconds(1f);
        }        

        Debug.Log(CharacterName + " 공격!");//적에게 공격
        if (isBoss)
        {
            if (SkillNumber == 3)
            {
                Transform Location = this.transform;
                Warning.SetActive(true);
                GameObject Effect = Instantiate(SkillEffects[SkillNumber], DragonsMouth);
                Destroy(Effect, 5f);
            }
        
            else if(SkillNumber == 0)
            {
                GetComponent<Animator>().SetTrigger("NormalAtt");              
                GameObject Effect = Instantiate(SkillEffects[SkillNumber], DragonsMouth);
                Destroy(Effect, 2f);
            }
            else
            {
                GetComponent<Animator>().SetTrigger("NormalAtt");
                GameObject Effect = Instantiate(SkillEffects[SkillNumber], targets[target].transform);
                Destroy(Effect, 2f);
            }
        }
        else if (SkillNumber == 0)
        {
            GetComponent<Animator>().SetTrigger("NormalAtt");
        }
        else if(SkillNumber == 1)
        {
            GetComponent<Animator>().SetTrigger("SpecialAtt");
            UseMana(SkillMana[SkillNumber]);
        }
        else if(SkillNumber == 2 && Skill[2] == "Fire Shield")
        {
            GetComponent<Animator>().SetTrigger("SpecialAtt");
            UseMana(SkillMana[SkillNumber]);
            GameObject[] x = GameObject.FindGameObjectsWithTag("Player");
            for(int z = 0; z < x.Length; z++)
            {
                x[z].GetComponent<CharacterInfo>().fireShield = true;
            }            
        }
        yield return new WaitForSeconds(0.3f);
        if(SkillNumber != 3)
        {
            GameObject Effect = Instantiate(SkillEffects[SkillNumber], targets[target].transform);
            Destroy(Effect, 2f);
        }        

        SFXm.Play(Skill[SkillNumber]);
        if(SkillNumber==3)
        {
            yield return new WaitForSeconds(5f);
            Warning.SetActive(false);
        }
        
        for (int j = 0; j < SkillHitCount[SkillNumber]; j++)//공격 타수
        {
            if (SkillArea[SkillNumber])
            {
                
                for (int k = 0; k < targets.Length; k++)//적 광역 피격데미지
                {
                    if (!targets[k].GetComponent<CharacterInfo>().isFaint)//적이 기절한 상태가 아니라면
                    {
                        targets[k].GetComponent<CharacterInfo>().Damage(DamageCalculate(), isCrit);
                    }
                }
            }
            else
            {
                targets[target].GetComponent<CharacterInfo>().Damage(DamageCalculate(), isCrit);//적 단일 피격데미지
            }
            yield return new WaitForSeconds(0.25f);
        }

        yield return new WaitForSeconds(2.0f);

        if (SkillDistance[SkillNumber] == 0)
        {
            Debug.Log(CharacterName + " 원래 자리로 이동!");//원래자리로 복귀
            GetComponent<Animator>().SetBool("Run", true);
            for (int j = 0; j < steps; j++)
            {
                this.gameObject.transform.Translate(-Move, Space.World);
                yield return null;
            }
            GetComponent<Animator>().SetBool("Run", false);
        }
       
        Debug.Log(i.Pop() + " 액션 종료!");//종료!
    }

    public int DamageCalculate()        //데미지 계산공식
    {
        float CriticalRatio = 1.0f;
        if (Random.Range(0,100) <= Luck)
        {
            CriticalRatio = 2.0f;
        }
        int damage = (int)(Attack * Random.Range(0.85f, 1.15f) * SkillMultiplier[SkillNumber] * CriticalRatio);
        if(CriticalRatio == 2.0f)
        {
            isCrit = true;
            Debug.Log(damage + "의 크리티컬 데미지를 입혔다!");
        }
        else
        {
            isCrit = false;
            Debug.Log(damage + "의 데미지를 입혔다!");
        }
        
        return damage;
    }
}