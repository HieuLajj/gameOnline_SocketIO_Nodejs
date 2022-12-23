using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionLobby : MonoBehaviour
{
    // Start is called before the first frame update
    public RectTransform MainRT;
    public RectTransform thisRT;
    private float _height;
    void Start()
    {
        _height = (float)MainRT.rect.height/2;
        thisRT.anchoredPosition = new Vector2(0,_height);            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
