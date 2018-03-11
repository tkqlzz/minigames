using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    bool jumpOn = false;
    float jumpPower = 1.3f;
    float tempJump;
    Transform playerTf;
    Vector3 tempVec;

    Animator playerAni;

    void Awake()
    {
        playerTf = transform;
        playerAni = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
       if(!jumpOn)
        {
            if ((Input.GetMouseButtonDown(0)))//버튼을 누름.
            {
               if(EventSystem.current.IsPointerOverGameObject() ==false)
                {
                    StartCoroutine("CheckButtonDownSec");
                }
            }
            else if((Input.GetMouseButtonUp(0)))//버튼에서 땜.
            {
               if(EventSystem.current.IsPointerOverGameObject() == false)
                {//UI이 위가 아니면
                    if(checkTime > 0)
                    {//점프 수치가 있어야 실행함.
                        StartCoroutine("JumpAction");
                    }
                }
                else
                {
                    //위면 점프 실행 x +시간 측정 끔.
                    if(checkTime > 0)
                    {
                        StopCoroutine("CheckButtonDownSec");
                        checkTime = 0;
                    }
                }
            }
        }
    }


    float checkTime;
    IEnumerator CheckButtonDownSec()
    {//버튼 누르고있는 시간 측정을 위한 코루틴.
        checkTime = 0;
        while (!jumpOn)
        {
            yield return new WaitForSeconds(0.03f);//시간 측정은 0.04초에 한번씩
            checkTime += 0.03f;
        }

    }
    IEnumerator JumpAction()
    {//점프 처리는 코루팅으로.
     //누르는 시간에 따라 혹은 드래그에 따라 점프 파워 달리함

        jumpOn = true;
        tempVec = playerTf.position;

        if(checkTime > 0.08f)//차지 가능한 최대 시간.
        {
            if(checkTime > 0.25f)
            {
                playerAni.SetTrigger("jump1");//트리거 켜줌 -> 애니메이션 시작.
            }
            else
            {
                playerAni.SetTrigger("jump2");//높은 점프용.
            }

            yield return new WaitForSeconds(0.08f);//애니메이션 처리 끝나고 점프하도록
        }
       
        tempJump = jumpPower * checkTime;//기존 점프파워보다 상승시킨 후 checktime 적용


        
        tempVec.y += tempJump;
        playerTf.position = tempVec;

        while (tempVec.y > (-4.56))
        {
            yield return new WaitForSeconds(0.03f);
            tempJump -= 0.1f;
            tempVec.y += tempJump;
            playerTf.position = tempVec;
        }

        tempVec.y = (float)-4.56;
        playerTf.position = tempVec;
        jumpOn = false;

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "라이언(Clone)")
        {

            HPViewRun.hp -= 10;

        }
    }

}

