using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour {
    public Canvas selectPopup;
    public Canvas ItemPopup;
    public Canvas exitPopup;
    public Toggle[] itemToggle;
    private string game;

    public TMP_Text myTotalScore;

    public TMP_Text[] scoreText;
    public TMP_Text[] itemText;

    public Image[] gameIconImages;
    public Button[] gameIconButtons;
    public Sprite lockSprite;
    public Text[] lockText;


    // Use this for initialization
    void Start () {
        Screen.SetResolution(800, 1280, true);
        Screen.orientation = ScreenOrientation.Portrait;

        selectPopup.enabled = false;
        ItemPopup.enabled = false;
        exitPopup.enabled = false;

        lockText[1].text = "";
        lockText[0].text = "";

        myTotalScore.text = Player.instance.total_score.ToString();

        for (int i =0; i < UserScore.list.Length; i++)
        {
            if (UserScore.list[i].game_name == "Snack")
            {
                scoreText[0].text = UserScore.list[i].score.ToString();
                ScoreManagerSnack.maxScore = UserScore.list[i].score;
            }
            else if (UserScore.list[i].game_name == "Basket")
            {
                scoreText[1].text = UserScore.list[i].score.ToString();
                ScoreManagerBasket.maxScore = UserScore.list[i].score;
            }
            else if (UserScore.list[i].game_name == "Run")
            {
                scoreText[2].text = UserScore.list[i].score.ToString();
                ScoreManager.maxScore = UserScore.list[i].score;
            }
            else if (UserScore.list[i].game_name == "Word")
            {
                scoreText[3].text = UserScore.list[i].score.ToString();
            }
            else if (UserScore.list[i].game_name == "Alkkagi")
            {
                scoreText[4].text = UserScore.list[i].score.ToString();
            }
        }

        for (int i = 0; i < UserItem.list.Length; i++)
        {
            itemText[UserItem.list[i].item_id - 1].text = UserItem.list[i].point.ToString(); 
        }

        for (int i = 0; i < itemText.Length; i++)
        {
            if (itemText[i].text == "-" || itemText[i].text == "0")
                itemToggle[i].interactable = false;
        }

        if(Player.instance.total_score < 1000)
        {
            gameIconImages[4].sprite = lockSprite;
            gameIconImages[4].color = Color.black;
            gameIconButtons[4].interactable = false;
            lockText[1].text = "1000";
        }
        if (Player.instance.total_score < 500)
        {
            gameIconImages[2].sprite = lockSprite;
            gameIconImages[2].color = Color.black;
            gameIconButtons[2].interactable = false;
            lockText[0].text = "500";
        }


    }
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (ItemPopup.enabled)
            {
                ItemPopup.enabled = false;
            }
            else if(selectPopup.enabled)
            {
                selectPopup.enabled = false;
            }
            else if(!exitPopup.enabled) 
            {
                exitPopup.enabled = true;
            }
        }
    }

    public void ChangeSceneShop()
    {
        SceneManager.LoadScene("Shop");
    }
    public void ChangeSceneRanking()
    {
        SceneManager.LoadScene("Ranking");
    }
    public void ChangeSceneGame()
    {
        for (int i=0; i<itemToggle.Length; i++)
        {
            if (itemToggle[i].isOn)
            {

                int errorCode = WWWManager.getInstance().DoItemUse(Player.instance.user_id, i+1);
                if (errorCode == HTTPErrorCode.SUCCESS)
                {
                    ItemUse.isUse[i] = true;
                } else
                {
                    ItemUse.isUse[i] = false;
                }
            }
            else
            {
                ItemUse.isUse[i] = false;
            }
        }

        SceneManager.LoadScene(game);
    }

    public void ChangeSceneLogin()
    {
        SceneManager.LoadScene("Login");
    }

    public void OpenSelectPopup(string game)
    {
        this.game = game;
        selectPopup.enabled = true;
    }
    public void CloseSelectPopup()
    {
        selectPopup.enabled = false;
    }
    
    public void OpenItemPopup(string item)
    {
        ItemPopup.enabled = true;
    }
    public void CloseItemPopup()
    {
        ItemPopup.enabled = false;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void CloseExitPopup()
    {
        exitPopup.enabled = false;
    }

    public void ToggleItem(int item_id)
    {
        if (itemToggle[item_id].isOn)
        {
            itemText[item_id].text = (System.Int32.Parse(itemText[item_id].text) - 1).ToString();
        }
        else
        {
            itemText[item_id].text = (System.Int32.Parse(itemText[item_id].text) + 1).ToString();
        }
    }
}
