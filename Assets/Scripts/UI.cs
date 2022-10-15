using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    // Start is called before the first frame update
    public Button playBtn;
    public TMP_InputField namePlayerText;

    void Start()
    {
        playBtn.onClick.AddListener(Ingame);
        
    }

    // Update is called once per frame
    public void Ingame(){
        //Debug.Log(namePlayerText.text);
        NetworkManager.instance.JoinGame(namePlayerText.text);
        gameObject.SetActive(false);
    }
    void Update()
    {
        
    }
}
