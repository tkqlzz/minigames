using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManagerSnack : MonoBehaviour {
    public static int score = 0;
    public static int maxScore = 0;
    public static bool isGetScore = false;

    public static void setScore(int value)
    {
        score += value;
        isGetScore = true;
    }

    public static int getScore()
    {
        return score;
    }

}
