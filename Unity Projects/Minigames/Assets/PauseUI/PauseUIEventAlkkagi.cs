using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUIEventAlkkagi : MonoBehaviour {
    public static bool isPauseOn;

    public Canvas pauseCanvas;

    // Use this for initialization
    void Start()
    {
        isPauseOn = false;
        pauseCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyUp(KeyCode.Escape) || Input.GetKey(KeyCode.Home)) && GameManagerAlkkagi.isStarted)
        {
            ActivePauseBt();
        }
    }

    public void ActivePauseBt()
    {//일시 정지 버튼을 눌렀을 때 처리
        isPauseOn = !isPauseOn;
        if (isPauseOn)
        {
            pauseCanvas.enabled = true;
        }
        else
        {//일시정지 중이면 해제.
            pauseCanvas.enabled = false;
        }
    }

    public void QuitBt()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Menu");//1번씬(현재씬) 다시 로드
    }
}
