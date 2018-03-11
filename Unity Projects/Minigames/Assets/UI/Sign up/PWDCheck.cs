using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PWDCheck : MonoBehaviour {
    public Text pwd1;
    public Text pwd2;
    public Text pwdChecktext;
    public static bool isSuccess = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    public void Compare() {
        StartCoroutine("DelayCompare");
    }

    IEnumerator DelayCompare()
    {
        yield return new WaitForSeconds(0.1f);
        if (pwd1.text != pwd2.text)
        {
            pwdChecktext.color = Color.red;
            pwdChecktext.text = "비밀번호가 다릅니다.";
            isSuccess = false;
        }
        else if (pwd1.text == pwd2.text)
        {
            if (pwd1.text == "")
            {
                pwdChecktext.text = "";
                isSuccess = false;
            }
            else
            {
                pwdChecktext.color = Color.green;
                pwdChecktext.text = "일치합니다.";
                isSuccess = true;
            }
        }
    }

   
}
