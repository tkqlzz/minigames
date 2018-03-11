using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUIEvent : MonoBehaviour {
    public static bool isPauseOn;

    public Canvas pauseCanvas;
    public string gameName;

    // Use this for initialization
    void Start () {
        isPauseOn = false;
        pauseCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKey(KeyCode.Home))
        {
            ActivePauseBt();
        }
    }

    public void ActivePauseBt()
    {//일시 정지 버튼을 눌렀을 때 처리
        isPauseOn = !isPauseOn;
        if (isPauseOn)
        {
            Time.timeScale = 0;//시간 흐름 비율 0으로
            pauseCanvas.enabled = true;
        }
        else
        {//일시정지 중이면 해제.
            Time.timeScale = 1.0f;//시간 흐름 비율 1로.
            pauseCanvas.enabled = false;
        }
    }

    public void RetryBt()
    {
        Time.timeScale = 1.0f;//시간 초기상태로 돌려줌.
        SceneManager.LoadScene(gameName);//1번씬(현재씬) 다시 로드
    }
    public void QuitBt()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu");//1번씬(현재씬) 다시 로드
    }
}
