using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreView : MonoBehaviour {
    public TMP_Text scoreLabel;
    private bool isScaleUpDown = false;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (ScoreManagerSnack.isGetScore)
        {
            scoreLabel.text = ScoreManagerSnack.score.ToString();
            if (!isScaleUpDown & ScoreManagerSnack.isGetScore)
                StartCoroutine(ScaleUpDonw());
        }
    }
    IEnumerator ScaleUpDonw()
    {
        isScaleUpDown = true;
        ScoreManagerSnack.isGetScore = false;
        Vector3 v3 = scoreLabel.rectTransform.localScale;
        while (v3.x < 1.05)
        {
            v3.x += 0.01f;
            v3.y += 0.01f;
            scoreLabel.rectTransform.localScale = v3;
            yield return new WaitForSeconds(0.001f);
        }
        while (v3.x > 0.975)
        {
            v3.x -= 0.01f;
            v3.y -= 0.01f;
            scoreLabel.rectTransform.localScale = v3;
            yield return new WaitForSeconds(0.001f);
        }
        while (v3.x < 1)
        {
            v3.x += 0.01f;
            v3.y += 0.01f;
            scoreLabel.rectTransform.localScale = v3;
            yield return new WaitForSeconds(0.001f);
        }
        isScaleUpDown = false;
    }
}
