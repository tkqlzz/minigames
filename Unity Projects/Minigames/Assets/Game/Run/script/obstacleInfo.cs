using System.Collections;
using UnityEngine;

public class obstacleInfo : MonoBehaviour
{
    public int highLevel = 0;//높이 레벨.
    float height;//높이

    Transform obsTf;
    private void Awake()
    {
        obsTf = transform;
    }

    public void SetObstacle(int lv)//함수 호출하여 정보 셋팅함
    {
        height = 0.015f + 0.015f * lv;//기본 0.015f +레벨당 0.015씩 증가
        Vector3 tempVec = new Vector3(1f, height, 1f);
        obsTf.localScale = tempVec;
        
    }

    

}
