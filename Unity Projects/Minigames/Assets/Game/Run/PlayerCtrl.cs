using UnityEngine;
using System.Collections;

public class PlayerCtrl : MonoBehaviour
{
	float speed = 2f;//속도

	void Update()//메프레임 실행
	{
		if (Input.GetKey(KeyCode.LeftArrow))//왼쪽을 눌렀다면
		{
			transform.Translate(Vector2.left * speed * Time.deltaTime);//왼쪽으로 speed만큼 이동
		}
		if (Input.GetKey(KeyCode.RightArrow))//오른쪽을 눌렀다면
		{
			transform.Translate(Vector2.right * speed * Time.deltaTime);//오른쪽으로 speed만큼 이동
        }
	}
}