using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchingPanel : MonoBehaviour {
    public RectTransform matchingPanel;
    Vector3 v3;
    // Use this for initialization
    void Start () {
        StartCoroutine(ScaleLoop());

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator ScaleLoop()
    {
        v3 = matchingPanel.localScale;
        while (!GameManagerAlkkagi.isStarted)
        {
            while (v3.x < 1.15)
            {
                v3.x += 0.0015f;
                v3.y += 0.0015f;
                //v3.z += 0.005f;
                matchingPanel.localScale = v3;
                yield return new WaitForSeconds(0.01f);
            }
            while (v3.x > 0.95)
            {
                v3.x -= 0.0015f;
                v3.y -= 0.0015f;
                //v3.z -= 0.005f;
                matchingPanel.localScale = v3;
                yield return new WaitForSeconds(0.01f);
            }



        }
        
    }
}
