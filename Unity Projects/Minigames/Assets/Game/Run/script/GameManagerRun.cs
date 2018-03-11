using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManagerRun : MonoBehaviour {
    public Canvas resultCanvas;
    public static bool isEnded;
    public TMP_Text scoreText;
    public TMP_Text starPointText;
    public static bool isUseScoreBooster;

    // Use this for initialization
    void Start () {
        isEnded = false;
        resultCanvas.enabled = false;
        isUseScoreBooster = false;
        Screen.SetResolution(1280, 800, true);
        Screen.orientation = ScreenOrientation.Landscape;
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
            resultCanvas.enabled = true;
            scoreText.text = ScoreManager.scorePoint.ToString();
            starPointText.text = ScoreManager.starPoint.ToString();
            WWWManager wwwManager = WWWManager.getInstance();
            if (ScoreManager.maxScore < ScoreManager.scorePoint)
                wwwManager.SendScore("Run", ScoreManager.scorePoint);
            wwwManager.AddStarPoint(ScoreManager.starPoint);

        }
    }

    public static void EndGame()
    {
        isEnded = true;
        Time.timeScale = 0;
    }

    public void ChangeSceneMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
}
