using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionResetBtn : MonoBehaviour
{
    // Start is called before the first frame update
    public RectTransform MainRT;
    public RectTransform thisRT;
    private float _width;
    private float _height;
    void Start()
    {
        _width = (float)MainRT.rect.width/2;
        _height = (float)-MainRT.rect.height/2;
        thisRT.anchoredPosition = new Vector2(_width,_height);            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
