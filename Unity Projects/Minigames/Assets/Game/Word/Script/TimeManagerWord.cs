using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManagerWord : MonoBehaviour {
    public static int time;
    public static int maxtime;
    public static bool isShow;

    // Use this for initialization
    void Start()
    {
        StartCoroutine("TimerStart");
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator TimerStart()
    {
        isShow = true;
        switch (RoundManagerWord.round)
        {
            case 1:
                time = 10;
                break;
            case 2:
                time = 10;
                break;
            case 3:
                time = 15;
                break;
            case 4:
                time = 20;
                break;
            case 5:
                time = 20;
                break;
        }
        maxtime = time;

        while (time > 0)
        {
            yield return new WaitForSeconds(1);
            time--;
        }

        isShow = false;
        switch (RoundManagerWord.round)
        {
            case 1:
                time = 30;
                break;
            case 2:
                time = 30;
                break;
            case 3:
                time = 35;
                break;
            case 4:
                time = 40;
                break;
            case 5:
                time = 40;
                break;
        }
        maxtime = time;

        //GameManager.EndGame();

    }
}
