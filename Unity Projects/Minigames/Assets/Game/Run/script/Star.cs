using System.Collections;

using UnityEngine;

public class Star : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        //볼과 충돌 별 한개 획득
        if(col.CompareTag("Player"))
        {
            Debug.Log("??");
            ScoreManager.starPoint += 1;
            gameObject.SetActive(false);
        }
    }
}