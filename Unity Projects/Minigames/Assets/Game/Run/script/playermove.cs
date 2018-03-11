using UnityEngine;
using System.Collections;


public class playermove : MonoBehaviour
{
    public float speed = 0.5f;// 속도값
    private Transform playerTf;//참조를 위해
    private Vector3 playerPos;//플레이어 위치값

    void Awake()
    {
        playerTf = transform;
    }
    void Start()
    {
        playerPos = playerTf.position;
    }
    void FixedUpdate()
    {
        playerPos.x += speed;
        playerTf.position = playerPos;
    }
}
