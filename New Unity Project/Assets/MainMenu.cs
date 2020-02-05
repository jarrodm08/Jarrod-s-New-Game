using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
 
    void Start()
    {
        loadClouds();
    }

    void Update()
    {
        moveClouds();
    }

    
   



    // CLOUDS //
    private GameObject cloudA;
    private Vector2 startPos;
    private float scrollSpeed = 3f;

    private void loadClouds()
    {
        cloudA = this.transform.parent.Find("Clouds").gameObject;
        startPos = cloudA.transform.position;
    }

    private void moveClouds()
    {
        float newPos = Mathf.Repeat(Time.time * scrollSpeed,17.7f); // Number 17.7f depends on size of image, adjust to a precise number where the image repeats seemlessly
        cloudA.transform.position = startPos + Vector2.right * newPos;
    }
    // ----- //
}
