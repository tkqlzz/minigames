using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManagerSnack : MonoBehaviour {
    static bool isEnded;
    public TMP_Text maxScore;
    public Canvas popup;
    public TMP_Text score;
    public TMP_Text starpoint;
    public static bool isScoreBooster;

    // Use this for initialization
    void Start () {
        isEnded = false;
        popup.enabled = false;
        ScoreManagerSnack.score = 0;

        Screen.SetResolution(1280, 800, true);
        Screen.orientation = ScreenOrientation.Landscape;
        maxScore.text = ScoreManagerSnack.maxScore.ToString();


        isScoreBooster = false;
        if (ItemUse.isUse[0])
        {
            isScoreBooster = true;
            ItemUse.isUse[0] = false;
        }
        
    }
	// Update is called once per frame
	void Update () {
		if (isEnded)
        {
            isEnded = false;
            popup.enabled = true;
            score.text = ScoreManagerSnack.score.ToString();
            starpoint.text = (ScoreManagerSnack.score / 30).ToString();

            WWWManager wwwManager = WWWManager.getInstance();
            if (ScoreManagerSnack.maxScore < ScoreManagerSnack.score)
                wwwManager.SendScore("Snack", ScoreManagerSnack.score);
            wwwManager.AddStarPoint(ScoreManagerSnack.score / 30);

        }
	}

    public static void EndGame()
    {
        Time.timeScale = 0f;
        isEnded = true;
    }

    public void ChangeSceneMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
