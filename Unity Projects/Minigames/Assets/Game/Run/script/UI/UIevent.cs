using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UIevent : MonoBehaviour {
    private bool pauseOn = false;
    private GameObject normalPanel;
    private GameObject pausePanel;
    
    private void Awake()
    {
        normalPanel = GameObject.Find("Canvas").transform.Find("NormalUI").gameObject;
        pausePanel = GameObject.Find("Canvas").transform.Find("PauseUI").gameObject;
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKey(KeyCode.Home))
        {
            ActivePauseBt();
        }
    }

    public void ActivePauseBt()
    {//일시 정지 버튼을 눌렀을 때 처리
       if(!pauseOn)
        {
            Time.timeScale = 0;//시간 흐름 비율 0으로
            pausePanel.SetActive(true);
            normalPanel.SetActive(false);
        }
        else
        {//일시정지 중이면 해제.
            Time.timeScale = 1.0f;//시간 흐름 비율 1로.
            pausePanel.SetActive(false);
            normalPanel.SetActive(true);
        }
        pauseOn = !pauseOn;//불값 반전.
    }

    public void RetryBt()
    {
        Debug.Log("게임 재시작.");
        Time.timeScale = 1.0f;//시간 초기상태로 돌려줌.
        SceneManager.LoadScene("Run");//1번씬(현재씬) 다시 로드
    }
    public void QuitBt()
    {
        Debug.Log("게임 종료.");
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu");//게임 종료.
    }
}
