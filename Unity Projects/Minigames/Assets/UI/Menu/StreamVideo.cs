using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class StreamVideo : MonoBehaviour {

    public RawImage image;
    public VideoPlayer[] videoPlayer;

    int i;

    public void Play(int i)
    {
        //Application.runInBackground = true;
        this.i = i;
        StartCoroutine(PlayVideo());
    }

    IEnumerator PlayVideo()
    {
        if (!videoPlayer[i].isPrepared)
            videoPlayer[i].Prepare();
        while (!videoPlayer[i].isPrepared)
        {
            yield return new WaitForSeconds(0.1f);
        }
        
        videoPlayer[i].Play();
        image.enabled = true;
        image.texture = videoPlayer[i].texture;
        videoPlayer[i].Play();
    }

    public void Stop()
    {
        image.enabled = false;
        videoPlayer[i].Pause();
        videoPlayer[i].frame = 0;
    }
}
