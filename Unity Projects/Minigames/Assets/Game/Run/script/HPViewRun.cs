using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPViewRun : MonoBehaviour {
    public Slider hpSlider;
    public static float hp;

    // Use this for initialization
    void Start () {
        hp = 80;
        StartCoroutine(DecreaseHp());
	}
	
    IEnumerator DecreaseHp()
    {
        while(hp > 0)
        {
            yield return new WaitForSeconds(0.1f);
            hp -= 0.1f;
            hpSlider.value = hp;
        }
        if (hp < 0)
            GameManagerRun.EndGame();
    }
}
