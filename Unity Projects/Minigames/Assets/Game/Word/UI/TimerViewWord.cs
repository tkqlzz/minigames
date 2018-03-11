using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerViewWord : MonoBehaviour {

    Slider timerSlider;
    // Use this for initialization
    void Start()
    {
        timerSlider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        timerSlider.value = TimeManagerWord.time;
        timerSlider.maxValue = TimeManagerWord.maxtime;
    }

}
