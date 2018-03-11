using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static Animator animator;
    private float sec;
    private bool isNext;

    // Use this for initialization
    void Start()
    {
        animator = this.GetComponent<Animator>();
        StartCoroutine("ChangeIdle");
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator ChangeIdle()
    {
        animator.SetInteger("enemyState", 0);
        sec = Random.Range(0.5f, 3f);
        yield return new WaitForSeconds(sec);
        isNext = (Random.Range(0, 4) == 0) ? false : true;
        if (isNext)
            StartCoroutine("ChangePre");
        else
            StartCoroutine("ChangeIdle");
    }

    IEnumerator ChangePre() {
        animator.SetInteger("enemyState", 1);
        sec = Random.Range(0.5f, 2f);
        yield return new WaitForSeconds(sec);
        isNext = (Random.Range(0, 4) == 0) ? false : true;
        if (isNext)
            StartCoroutine("ChangeTurn");
        else
            StartCoroutine("ChangeIdle");
    }

    IEnumerator ChangeTurn() {
        
        animator.SetInteger("enemyState", 2);
        yield return new WaitForSeconds(0.2f);
        animator.SetInteger("enemyState", 3);
        sec = Random.Range(0.5f, 3f);
        yield return new WaitForSeconds(sec);

        isNext = (Random.Range(0, 3) == 0) ? false : true;
        while (animator.GetInteger("enemyState") == 4)
        {
            yield return new WaitForSeconds(4f);
            isNext = true;
        }
                
        
        if (isNext)
            StartCoroutine("ChangeIdle");
        else
            StartCoroutine("ChangePre");
    }
    
}