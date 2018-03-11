using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour {
    public static int life;

    void Start()
    {
        life = 3;
    }

    public static void setScore(int value)
    {
        life += value;
    }

    public static int getScore()
    {
        return life;
    }

}
