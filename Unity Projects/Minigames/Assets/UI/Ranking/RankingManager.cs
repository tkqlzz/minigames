using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class RankingManager : MonoBehaviour {
    public TMP_Text myScore;
    public TMP_Text myRank;

    public TMP_Text[] id;
    public TMP_Text[] score;

    public Canvas popupCanvas;
    public Text popupText;


    // Use this for initialization
    void Start () {
        Screen.SetResolution(800, 1280, true);
        popupCanvas.enabled = false;
        Player player = Player.getInstance();
        myScore.text = player.total_score.ToString();

        WWWManager wwwManager = WWWManager.getInstance();
        string result = wwwManager.GetRank(player.user_id, "total");

        if (result == "")
        {
            popupCanvas.enabled = true;
            popupText.text = "서버접속실패!";
            return;
        }

        Result res = JsonUtility.FromJson<Result>(result);
        if (res.error == HTTPErrorCode.SUCCESS)
        {
            if (res.data != "")
                myRank.text = (Int32.Parse(res.data) + 1).ToString();
        }

        // 랭킹 1-10 GET
        result = wwwManager.GetAllRank("total");

        if (result == "")
        {
            popupCanvas.enabled = true;
            popupText.text = "서버접속실패!";
            return;
        }

        res = JsonUtility.FromJson<Result>(result);

        if (res.error == HTTPErrorCode.SUCCESS)
        {
            Rank.list = JsonHelper.FromJson<Rank>(res.data);

            for (int i = 0; i < Rank.list.Length; i++)
            {
                id[Rank.list[i].rank - 1].text = Rank.list[i].userId;
                score[Rank.list[i].rank - 1].text = Rank.list[i].score.ToString();
            }
        }
        
            
        
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (popupCanvas.enabled)
            {
                popupCanvas.enabled = false;
            }
            else
            {
                ChangeSceneMenu();
            }
        }
    }

    public void ChangeSceneMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ClosePopup()
    {
        popupCanvas.enabled = false;
    }
}
