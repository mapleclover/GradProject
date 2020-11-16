using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeInOut : MonoBehaviour
{
    public Image Fade;
    public float Fades = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(SceneFadeOut());
        if (Fades >= 1.0f)
        {
            StopCoroutine(SceneFadeOut());
        }
    }
    IEnumerator SceneFadeOut()
    {
        while (Fades <= 1.0f)
        {
            Fades += 0.05f;
            Fade.color = new Color(0, 0, 0, Fades);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
