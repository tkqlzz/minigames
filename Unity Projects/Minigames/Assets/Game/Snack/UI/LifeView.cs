using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeView : MonoBehaviour {

    public Texture lifeBrokenImage;
    RawImage[] lifeImages;
    // Use this for initialization
    void Start () {
        lifeImages = new RawImage[3];
        lifeImages[0] = transform.Find("LifeIcon1").GetComponent<RawImage>();
        lifeImages[1] = transform.Find("LifeIcon2").GetComponent<RawImage>();
        lifeImages[2] = transform.Find("LifeIcon3").GetComponent<RawImage>();
    }
	
	void Update () {
        switch (LifeManager.life)
        {
            case 0:
                lifeImages[0].texture = lifeBrokenImage;
                break;
            case 1:
                lifeImages[1].texture = lifeBrokenImage;
                break;
            case 2:
                lifeImages[2].texture = lifeBrokenImage;
                break;
        }
    }
}
