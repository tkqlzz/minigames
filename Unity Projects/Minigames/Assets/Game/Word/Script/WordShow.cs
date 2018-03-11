using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordShow : MonoBehaviour {
    //private GUIStyle guiStyle = new GUIStyle();

    // Use this for initialization
    void Start () {
        //guiStyle.fontSize = 200;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnGUI()
    {
        if (TimeManagerWord.isShow)
        {
            switch (RoundManagerWord.round)
            {
                case 1:
                    GUI.Box(new Rect(100, 250, 600, 600), "test");
                    break;
                case 2:
                    GUI.Box(new Rect(100, 400, 300, 300), "test");
                    GUI.Box(new Rect(400, 400, 300, 300), "test");
                    break;
                case 3:
                    GUI.Box(new Rect(100, 500, 200, 200), "test");
                    GUI.Box(new Rect(300, 500, 200, 200), "test");
                    GUI.Box(new Rect(500, 500, 200, 200), "test");
                    break;
                case 4:
                    GUI.Box(new Rect(100, 250, 300, 300), "test");
                    GUI.Box(new Rect(400, 250, 300, 300), "test");
                    GUI.Box(new Rect(100, 550, 300, 300), "test");
                    GUI.Box(new Rect(400, 550, 300, 300), "test");
                    break;
                case 5:
                    GUI.Box(new Rect(100, 250, 300, 300), "test");
                    GUI.Box(new Rect(400, 250, 300, 300), "test");
                    GUI.Box(new Rect(100, 550, 300, 300), "test");
                    GUI.Box(new Rect(400, 550, 300, 300), "test");
                    break;
            }
        } else if (TimeManagerWord.isShow)
        {
            switch (RoundManagerWord.round)
            {
                case 1:
                    GUI.Box(new Rect(100, 250, 600, 600), "");
                    break;
                case 2:
                    GUI.Box(new Rect(100, 400, 300, 300), "");
                    GUI.Box(new Rect(400, 400, 300, 300), "");
                    break;
                case 3:
                    GUI.Box(new Rect(100, 500, 200, 200), "");
                    GUI.Box(new Rect(300, 500, 200, 200), "");
                    GUI.Box(new Rect(500, 500, 200, 200), "");
                    break;
                case 4:
                    GUI.Box(new Rect(100, 250, 300, 300), "");
                    GUI.Box(new Rect(400, 250, 300, 300), "");
                    GUI.Box(new Rect(100, 550, 300, 300), "");
                    GUI.Box(new Rect(400, 550, 300, 300), "");
                    break;
                case 5:
                    GUI.Box(new Rect(100, 250, 300, 300), "");
                    GUI.Box(new Rect(400, 250, 300, 300), "");
                    GUI.Box(new Rect(100, 550, 300, 300), "");
                    GUI.Box(new Rect(400, 550, 300, 300), "");
                    break;
            }
        }
        

    }

}
