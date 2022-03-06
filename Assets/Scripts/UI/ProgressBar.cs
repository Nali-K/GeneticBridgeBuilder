using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    // Start is called before the first frame update
    public float progress=0.0f;
    public RectTransform background;
    public RectTransform bar;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        var barOffsetMax = bar.offsetMax;
        barOffsetMax.x = -background.rect.width * (1-Mathf.Clamp(progress,0f,1f));
        bar.offsetMax = barOffsetMax;

    }
}
