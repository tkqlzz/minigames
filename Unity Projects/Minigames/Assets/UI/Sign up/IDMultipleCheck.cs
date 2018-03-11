using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IDMultipleCheck : MonoBehaviour {
    public Text checkText;
    public static bool isSuccess;

    // Use this for initialization
    void Start () {
        isSuccess = false;
    }
	
	// Update is called once per frame
	void Update () {
	}

    public void IdMultipleCheck(Text id) {
        if (id.text == "")
        {
            checkText.color = Color.red;
            checkText.text = "ID를 입력해주십시오.";
            return;
        }

        int errorCode = WWWManager.getInstance().IdMultipleCheck(id.text);

    
        isSuccess = false;
        if (errorCode == HTTPErrorCode.SUCCESS)
        {
            checkText.color = Color.green;
            checkText.text = "사용가능한 ID 입니다.";
            isSuccess = true;
        }
        else if (errorCode == HTTPErrorCode.NULL_ERROR)
        {
            checkText.color = Color.red;
            checkText.text = "중복되는 ID입니다.";
        }
        else if (errorCode == HTTPErrorCode.CONNECTION_ERROR)
        {
            checkText.color = Color.red;
            checkText.text = "서버접속실패!.";
        }
        else
        {
            checkText.color = Color.red;
            checkText.text = "ErrorCode : " + errorCode.ToString();
        }
        
            
    }

    public void CheckReset()
    {
        checkText.text = "";
        isSuccess = false;
    }
}
