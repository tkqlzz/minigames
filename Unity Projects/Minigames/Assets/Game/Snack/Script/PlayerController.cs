using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static Animator animator;

    // Use this for initialization
    void Start()
    {
        animator = this.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !PauseUIEvent.isPauseOn)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Player_idle") && animator.GetInteger("playerState") == 0)
            {
                animator.Play("Player_eating");
                if (GameManagerSnack.isScoreBooster)
                    ScoreManagerSnack.setScore(15);
                else
                    ScoreManagerSnack.setScore(10);

                if (EnemyController.animator.GetCurrentAnimatorStateInfo(0).IsName("Enemy_turn") && EnemyController.animator.GetInteger("enemyState") == 3)
                {
                    StartCoroutine("Catch");
                }
            }
        }   
    }

    IEnumerator Catch()
    {
        Handheld.Vibrate();
        animator.SetInteger("playerState", 1);
        EnemyController.animator.SetInteger("enemyState", 4);
        LifeManager.setScore(-1);

        if (LifeManager.life == 0)
        {
            GameManagerSnack.EndGame();
        }

        yield return new WaitForSeconds(4f);

        animator.SetInteger("playerState", 0);
        EnemyController.animator.SetInteger("enemyState", 0);
    }
}
