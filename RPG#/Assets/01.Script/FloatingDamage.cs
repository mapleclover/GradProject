using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingDamage : MonoBehaviour
{
    public Animator anim;
    private Text damageText;
    
    void Start()
    {
        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, clipInfo[0].clip.length);
    }

    public void SetText(string damage)
    {
        damageText = anim.GetComponent<Text>();
        damageText.text = damage;
    }
}
