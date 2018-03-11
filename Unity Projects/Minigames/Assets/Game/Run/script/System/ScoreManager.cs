using System.Collections;
using UnityEngine.UI;//UI 사용하려면 추가함
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public static int scorePoint;
    public static int maxScore;
    public static int starPoint;

    private Text scoreTx;
    public Text starTx;

    void Awake()
    {
        scorePoint = 0;
        scoreTx = GameObject.Find("NormalUI").transform.Find("scoreTx").GetComponent<Text>();
        scoreTx.text = "SCORE : 0";
        StartCoroutine("PlusScoreRoutine");
    }
    public void PlusScore(int plusPoint)
    {
        scorePoint += plusPoint;//받아온 만큼 추가.
        /*if (plusIng)
        {
            StopCoroutine("PlusValue");
        }
        StartCoroutine("PlusValue");*/

        scoreTx.text = "SCORE : " + scorePoint.ToString();//text 바꿔줌
    }

    IEnumerator PlusScoreRoutine()
    {
        while (true)
        {
            if(GameManagerRun.isUseScoreBooster)
                yield return new WaitForSeconds(0.07f);//1초 간격으로 스코어 점수 줌.
            else
                yield return new WaitForSeconds(0.1f);//1초 간격으로 스코어 점수 줌.
            PlusScore(1);
            starTx.text = "StarPoint : " + starPoint.ToString();
        }
    }
    bool plusIng = false;
    float tempPer;
    IEnumerator PlusValue()//점수 증가시 호출
    {
        plusIng = true;
        tempPer = 0;
        int pastScorePoint=0;
        while (tempPer < 1f)
        {
           
            tempPer += 0.1f;
            if (pastScorePoint < scorePoint)
            {//카운트 형식으로 상승하기 위하여 Lerp사용
                pastScorePoint = (int)Mathf.Lerp(pastScorePoint, scorePoint, tempPer);
                scoreTx.text = "SCORE : " + pastScorePoint.ToString("N0");
            }
            yield return new WaitForSeconds(0.033f);
        }
        pastScorePoint = scorePoint;
        scoreTx.text = "SCORE : " + scorePoint.ToString("N0");//text 최종 수치로 바꿔줌

        plusIng = false;
    }


    
}


