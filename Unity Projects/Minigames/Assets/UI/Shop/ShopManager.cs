using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ShopManager : MonoBehaviour {
    public TMP_Text myStarPoint;
    public TMP_Text myScoreBoosterPoint;
    public Canvas popupCanvas;
    public Text popupText;

    private int[] itemCost = { 0, 10 };
    private int[] itemCount = { 0, 0 };

    private bool isBuy;

    // Use this for initialization
    void Start () {
        Screen.SetResolution(800, 1280, true);
        isBuy = false;
        popupCanvas.enabled = false;
        myStarPoint.text = Player.getInstance().star_point.ToString();
        for(int i=0; i < UserItem.list.Length; i++)
        {
            if (UserItem.list[i].item_id == 1)
                itemCount[UserItem.list[i].item_id] = UserItem.list[i].point;
                myScoreBoosterPoint.text = itemCount[UserItem.list[i].item_id].ToString();
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

    public void BuyItem(int itemId)
    {
        if (!isBuy)
        {
            isBuy = true;
            if (Player.instance.star_point < itemCost[itemId])
            {
                popupCanvas.enabled = true;
                popupText.text = "별개수가 부족합니다";
            }
            else if (Player.instance.star_point >= itemCost[itemId])
            {
                int errorCode = WWWManager.getInstance().DoBuyItem(Player.instance.user_id, itemId, itemCount[itemId] + 1, itemCost[itemId]);

                if (errorCode == HTTPErrorCode.CONNECTION_ERROR)
                {
                    popupCanvas.enabled = true;
                    popupText.text = "서버접속실패!";
                }
                else if (errorCode == HTTPErrorCode.SUCCESS)
                {
                    itemCount[itemId] += 1;

                    myScoreBoosterPoint.text = itemCount[itemId].ToString();
                    myStarPoint.text = Player.getInstance().star_point.ToString();
                }
                else
                {
                    popupCanvas.enabled = true;
                    popupText.text = "구매실패!\nErrorCode : " + errorCode.ToString();
                }

            }
            isBuy = false;
        }
    }

    public void ClosePopup()
    {
        popupCanvas.enabled = false;
    }
}
