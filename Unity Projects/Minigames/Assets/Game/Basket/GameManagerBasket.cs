using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManagerBasket : MonoBehaviour {

    public Canvas resultCanvas;
    public TMP_Text scoreText;
    public TMP_Text starPointText;
    public static bool isUseScoreBooster;

    public static bool isEnded;

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
		if(isEnded)
        {
            isEnded = false;
            resultCanvas.enabled = true;

            scoreText.text = ScoreManagerBasket.score.ToString();
            starPointText.text = ScoreManagerBasket.starPoint.ToString();

            WWWManager wwwManager = WWWManager.getInstance();
            if (ScoreManagerBasket.maxScore < ScoreManagerBasket.score)
                wwwManager.SendScore("Basket", ScoreManagerBasket.score);
            wwwManager.AddStarPoint(ScoreManagerBasket.starPoint);
        }
	}

    public static void EndGame()
    {
        isEnded = true;
        Time.timeScale = 0f;
    }

    public void ChangeSceneMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
