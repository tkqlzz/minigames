using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManagerSnack : MonoBehaviour {
    public static float time;
    
    // Use this for initialization
    void Start () {
        StartCoroutine("TimerStart");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator TimerStart()
    {
        time = 60;
        while (time > 0)
        {
            yield return new WaitForSeconds(0.1f);
            time -= 0.1f;
        }
        GameManagerSnack.EndGame();
            
    }

}
