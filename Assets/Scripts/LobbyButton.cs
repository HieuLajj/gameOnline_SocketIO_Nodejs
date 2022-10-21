using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyButton : MonoBehaviour
{
    [SerializeField]
    private Text LobbyId;
    
    public Lobby lobby;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(delegate(){
            LobbyUIManager.instance.Ingame(lobby.id);
        });
    }

    public void Hienthi(){
        LobbyId.text = lobby.id;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
