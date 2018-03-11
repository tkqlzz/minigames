using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TurnManagerAlkkagi : MonoBehaviour {
    public Text turnInfo;
    public static bool isTurn;
    public static int color;

    // Use this for initialization
    void Start () {
        isTurn = true;
        color = -1;
    }
	
	// Update is called once per frame
	void Update () {
        if (color == 0)
        {
            turnInfo.color = Color.black;
            turnInfo.text = MatchingInfo.blackId + " Turn";
        }
        else if (color == 1)
        {
            turnInfo.color = Color.white;
            turnInfo.text = MatchingInfo.whiteId + " Turn";
        }
    }

}
