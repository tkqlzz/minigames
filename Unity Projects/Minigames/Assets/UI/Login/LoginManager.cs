using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour {
    public Canvas popup;
    public Canvas exitPopup;
    public Text popupTxt;
    public Text id;
    public Text pw;

	// Use this for initialization
	void Start () {
        Screen.SetResolution(800, 1280, true);
        popup.enabled = false;
        exitPopup.enabled = false;

        Player.instance = null;
        UserScore.list = null;
        UserItem.list = null;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (popup.enabled)
            {
                ClosePopup();
            }
            else if (!exitPopup.enabled)
            {
                exitPopup.enabled = true;
            }
            
        }
    }

    public void Login()
    {
        popup.enabled = true;
        if (id.text == "")
        {
            popupTxt.text = "아이디를 입력해주십시오.";
            return;
        }
        if (pw.text == "")
        {
            popupTxt.text = "비밀번호를 입력해주십시오.";
            return;
        }

        popupTxt.text = "로그인중..";
        WWWManager wwwManager = WWWManager.getInstance();

        int errorCode = wwwManager.DoLogin(id.text, pw.text);
        if (errorCode == HTTPErrorCode.CONNECTION_ERROR)
        {
            popupTxt.text = "서버접속실패!";
        }
        else if (errorCode == HTTPErrorCode.NULL_ERROR)
        {
            popupTxt.text = "아이디 또는 비밀번호가 틀렸습니다.";
        }
        else if (errorCode == HTTPErrorCode.SUCCESS)
        {
            popupTxt.text = "로그인 성공\n로딩중 ...";
            wwwManager.GetUserScore(id.text);
            wwwManager.GetUserItem(id.text);
            SceneManager.LoadScene("Menu");
        }
    }

    public void ClosePopup()
    {
        popup.enabled = false;
    }

    public void ChangeSceneSignUp()
    {
        SceneManager.LoadScene("SignUp");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void CloseExitPopup()
    {
        exitPopup.enabled = false;
    }

}
