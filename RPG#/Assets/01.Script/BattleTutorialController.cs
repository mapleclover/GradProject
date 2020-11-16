using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTutorialController : MonoBehaviour
{
    public int TutorialStep;
    public GameObject Image;
    public GameObject Image2;
    public GameObject[] Tutorials;
    public GameObject[] TutorialsText;
    public GameObject ActionButton;

    private void Start()
    {
        TutorialStep = 0;

        ActionButton.SetActive(false);
    }
    
    public void Proceed(int i)
    {
        if (i == 4)
        {
            Image.SetActive(false);
            Image2.SetActive(true);
            return;
        }
        else if (i == TutorialStep + 1)
        {
            TutorialStep = i;
            Tutorials[i - 1].SetActive(false);
            TutorialsText[i - 1].SetActive(false);
            Tutorials[i].SetActive(true);
            TutorialsText[i].SetActive(true);

            if (TutorialStep == 3)
            {
                ActionButton.SetActive(true);
            }
        }
    }
}
