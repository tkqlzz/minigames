using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManagerWord : MonoBehaviour {

    public static int round = 1;

    public static void setScore(int value)
    {
        round += value;
    }

    public static int getScore()
    {
        return round;
    }
}
