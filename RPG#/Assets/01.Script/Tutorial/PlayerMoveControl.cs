using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMoveControl : MonoBehaviour

{
    [SerializeField]
    private Transform tr;
    private Animator anim;
    private FadeManager fm;
    private AudioManager SFXm;

    public float moveSpeed = 10.0f;
    public float rotSpeed = 100.0f;

    public int MaxHp = 100;
    public int CurrnetHp = 100;

    private int hashRun;

    public bool canMove = true;

    public string nextStage;

    void Start()
    {
        SFXm = FindObjectOfType<AudioManager>();
        fm = FindObjectOfType<FadeManager>();
        tr = this.gameObject.GetComponent<Transform>();
        anim = GetComponent<Animator>();
        hashRun = Animator.StringToHash("Run");
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
        {
            float v = Input.GetAxis("Vertical"); //-1.0f ~ 0.0f ~ 1.0f
            float h = Input.GetAxis("Horizontal");
            //float r = Input.GetAxis("Mouse X");

            if (v <= -0.1f)
                moveSpeed = 5.0f;
            else
                moveSpeed = 10.0f;

            Vector3 dir = (Vector3.forward * v); // + (Vector3.right * h);
            tr.Translate(dir.normalized * Time.deltaTime * moveSpeed);
            tr.Rotate(Vector3.up * Time.deltaTime * h * rotSpeed);


            /* 
            Vector3.forward = Vector3(0, 0, 1)
            Vector3.up      = Vector3(0, 1, 0)
            Vector3.right   = Vector3(1, 0, 0)

            Vector3.one  = Vector3(1, 1, 1)
            Vector3.zero = Vector3(0, 0, 0)
            */

            if (v != 0.0f)          //전진
            {
                anim.SetBool(hashRun, true);
            }
            else                    //idle
            {
                anim.SetBool(hashRun, false);
            }
        }        
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Tutorial"))
        {
            StartCoroutine(LoadNextScene(2));
        }
        else if(coll.CompareTag("Stage1"))
        {
            StartCoroutine(LoadNextScene(6));
        }
    }

    public void StartLoadNextScene()
    {
        StartCoroutine(LoadNextScene(2));
    }

    IEnumerator LoadNextScene(int i)
    {
        canMove = false;
        SFXm.Play("Decision");
        fm.FadeOut();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(i);
    }
}
