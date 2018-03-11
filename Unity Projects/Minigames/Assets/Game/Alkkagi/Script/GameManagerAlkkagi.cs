using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class GameManagerAlkkagi : MonoBehaviour {
    const int black = 0;
    const int white = 1;

    public Canvas matchingPopup;
    public Canvas endGamePopup;

    public TMP_Text resultText;
    public TMP_Text scoreText;
    public TMP_Text starPointText;

    public static bool isStarted;
    public static bool isEnded;
    public static int playerColor;

    private static int winColor;

    private bool isUseScoreBooster;

    // Use this for initialization
    void Start () {
        Physics.gravity = new Vector3(0, 0, 9.8f);

        isStarted = false;
        isEnded = false;
        playerColor = -1;
        winColor = -1;

        MatchingInfo.playerUserId = "";
        MatchingInfo.otherUserId = "";
        MatchingInfo.blackId = "";
        MatchingInfo.whiteId = "";
        MatchingInfo.playerScore = 0;
        MatchingInfo.otherScore = 0;

        Screen.SetResolution(800, 1280, true);
        Screen.orientation = ScreenOrientation.Portrait;
        endGamePopup.enabled = false;
        isUseScoreBooster = false;
        if (ItemUse.isUse[0])
        {
            ItemUse.isUse[0] = false;
            isUseScoreBooster = true;
        }

    }
	
	// Update is called once per frame
	void Update () {
        if (isEnded)
        {
            isEnded = false;
            endGamePopup.enabled = true;
            if (winColor == playerColor)
            {
                resultText.text = "WIN";

                int score = MatchingInfo.playerScore + 100;
                if (isUseScoreBooster)
                    score += 50;

                scoreText.text = (score).ToString();
                starPointText.text = "10";

                WWWManager wwwmanager = WWWManager.getInstance();
                
                wwwmanager.SendScore("Alkkagi", score);
                wwwmanager.SendScore("Alkkagi", Mathf.Max(0, MatchingInfo.otherScore - 50), MatchingInfo.otherUserId, false);

                wwwmanager.AddStarPoint(10);

            }
            else
            {
                int score = Mathf.Max(0, MatchingInfo.playerScore - 50);
                resultText.text = "LOSE";
                scoreText.text = score.ToString();
                starPointText.text = "0";

                // refresh
                bool hasGameScore = false;
                int beforeScore = 0;
                foreach (UserScore us in UserScore.list)
                {
                    if (us.game_name == "Alkkagi")
                    {
                        beforeScore = us.score;
                        us.score = score;
                        hasGameScore = true;
                        break;
                    }
                }

                Player.instance.total_score += (score - beforeScore);
                if (!hasGameScore)
                {
                    Array.Resize<UserScore>(ref UserScore.list, UserScore.list.Length + 1);
                    UserScore.list[UserScore.list.Length - 1] = new UserScore();
                    UserScore.list[UserScore.list.Length - 1].game_name = "Alkkagi";
                    UserScore.list[UserScore.list.Length - 1].score = score;
                }
            }
        }
    }

    public static void EndGame(int color)
    {
        winColor = color;

        isStarted = false;
        isEnded = true;
        PhotonNetwork.Disconnect();
    }

    public void ChangeSceneMenu()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Menu");
    }
}
