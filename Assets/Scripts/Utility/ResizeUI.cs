using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeUI : MonoBehaviour
{
    // Start is called before the first frame update
    public RectTransform thisRT;
    public RectTransform MainRT;
    void Start()
    {
        thisRT.sizeDelta = new Vector2(MainRT.rect.width*3/4,MainRT.rect.height*3/4);            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
