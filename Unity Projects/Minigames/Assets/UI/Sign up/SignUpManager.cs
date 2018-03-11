using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SignUpManager : MonoBehaviour {
    public Canvas popup;
    public Text popupTxt;
    public Text id;
    public Text pwd;
    public Text nameTxt;
    public Text email;

    private bool isSignUp; // 가입버튼 중복클릭방지
    private bool isSuccess; // 가입 성공여부

    // Use this for initialization
    void Start () {
        Screen.SetResolution(800, 1280, true);
        isSignUp = false;
        isSuccess = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (popup.enabled)
            {
                popup.enabled = false;
            }
            else
            {
                ChangeLoginScene();
            }
        }
    }

    public void ChangeLoginScene()
    {
        SceneManager.LoadScene("Login");
    }

    public void ChangeMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }

    public void SignUp()
    {
        if(!isSignUp)
        {
            isSignUp = true;
            popup.enabled = true;
            if (!IDMultipleCheck.isSuccess)
            {
                popupTxt.text = "중복체크를 하십시오";
                return;
            }
            if (!PWDCheck.isSuccess)
            {
                popupTxt.text = "패스워드가 일치하지않습니다.";
                return;
            }
            if (nameTxt.text == "")
            {
                popupTxt.text = "이름을 입력해주십시오";
                return;
            }
            if (email.text == "")
            {
                popupTxt.text = "이메일을 입력해주십시오";
                return;
            }

            popupTxt.text = "등록중..";
            int errorCode = WWWManager.getInstance().DoSignUp(id.text, pwd.text, nameTxt.text, email.text);

            if (errorCode == HTTPErrorCode.CONNECTION_ERROR)
            {
                popup.enabled = true;
                popupTxt.text = "서버접속실패!";
            }
            else if (errorCode == HTTPErrorCode.SUCCESS)
            {
                isSuccess = true;
                popup.enabled = true;
                popupTxt.text = "등록성공";
            }
            else
            {
                popup.enabled = true;
                popupTxt.text = "등록실패.\nErrorCode : " + errorCode.ToString();
            }
        }
    }

    public void ClosePopup()
    {
        popup.enabled = false;
        isSignUp = false;
        if (isSuccess)
        {
            ChangeMenuScene();
        }
    }

}
