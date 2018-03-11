using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManagerAlkkagi : MonoBehaviour {
    public static float time;
    public Slider timerSlider;
    public PhotonView photonView;

    // Use this for initialization
    void Start()
    {
        StartCoroutine("TimerStart");
        photonView = PhotonView.Get(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator TimerStart()
    {
        time = 15;
        while (true) {
            while (time > 0)
            {
                yield return new WaitForSeconds(0.1f);
                if (TurnManagerAlkkagi.isTurn && GameManagerAlkkagi.isStarted)
                    time -= 0.1f;
                timerSlider.value = time;
            }
            if (time <= 0)
            {
                photonView.RPC("EndTurn", PhotonTargets.All, (TurnManagerAlkkagi.color + 1) % 2);
                yield return new WaitForSeconds(1);
            }
        }
    }
    
    [PunRPC]
    void EndTurn(int color)
    {
        TurnManagerAlkkagi.color = color;
        time = 15;
    }
}
