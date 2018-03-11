using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManagerAlkkagi : MonoBehaviour {
    public static int blackScore;
    public static int whiteScore;
    public PhotonView photonView;

    // Use this for initialization
    void Start () {
        blackScore = 0;
        whiteScore = 0;
        photonView = PhotonView.Get(this);
    }
	
	// Update is called once per frame
	void Update () {
        if (GameManagerAlkkagi.isStarted)
        {
            if (blackScore >= 5)
            {
                photonView.RPC("EndGame", PhotonTargets.All, 0);
            }
            if (whiteScore >= 5)
            {
                photonView.RPC("EndGame", PhotonTargets.All, 1);
            }
        }
    }

    [PunRPC]
    void EndGame(int color)
    {
        GameManagerAlkkagi.EndGame(color);
    }
}
