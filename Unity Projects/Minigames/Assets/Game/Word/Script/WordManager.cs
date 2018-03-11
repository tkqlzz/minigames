using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordManager : MonoBehaviour {
    public static string[] list;
    // Use this for initialization
    void Start () {
        TextAsset text = (TextAsset)Resources.Load("File/hangul") as TextAsset;
        list = text.text.Split('\n');
        
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
