using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialStage : MonoBehaviour
{
    public GameObject[] tutorial;
    public GameObject Clear;
    public GameObject Enemy;
    public GameObject Front;
    public GameObject Back;
    public GameObject CheckPoint;
    private AudioManager SFXm;

    private int mission = 0;
    private int checkTime = 0;

    void Start()
    {
        tutorial[0].SetActive(true);
        CheckPoint.transform.position = Front.transform.position;
        CheckPoint.SetActive(true);
        SFXm = FindObjectOfType<AudioManager>();
    }

    private void FixedUpdate()
    {
        if(mission == 2 && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
        {
            checkTime++;
            if (checkTime > 30)
            {
                StartCoroutine(ClearSign(mission));
                checkTime = -3000;
                mission = 3;
            }
        }
        else
        {
            checkTime = 0;
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("CheckPoint"))
        {
            if (tutorial[mission].activeSelf)
            {
                StartCoroutine(ClearSign(mission));
            }
        }
    }

    IEnumerator ClearSign(int i)
    {
        SFXm.Play("Clear");
        CheckPoint.SetActive(false);
        Clear.SetActive(true);
        Clear.GetComponent<Text>().fontSize = 150;
        while(Clear.GetComponent<Text>().fontSize > 50)
        {
            Clear.GetComponent<Text>().fontSize -= 5;
            yield return null;
        }

        yield return new WaitForSeconds(2.0f);
        if(mission < 3)
        {
            mission++;
        }
        else
        {
            mission = 3;
        }

        Clear.SetActive(false);
        tutorial[i].SetActive(false);
        tutorial[i + 1].SetActive(true);

        if(i == 0)
        {
            CheckPoint.transform.position = Back.transform.position;
            CheckPoint.SetActive(true);
        }
        else if (i==1)
        {

        }
        else if(i == 2)
        {
            Vector3 relativePos = this.transform.position - Front.transform.position;
            Enemy.transform.rotation = Quaternion.LookRotation(relativePos);
            Enemy.transform.position = Front.transform.position;

            Enemy.SetActive(true);
        }
    }
}
