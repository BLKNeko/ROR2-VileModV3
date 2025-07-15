using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSegment : MonoBehaviour
{

    public RawImage segmentImage;
    public float heat, maxSegments;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        int segments = Mathf.RoundToInt(heat * maxSegments);
        segmentImage.uvRect = new Rect(0f, 0f, segments, 1f);
    }
}
