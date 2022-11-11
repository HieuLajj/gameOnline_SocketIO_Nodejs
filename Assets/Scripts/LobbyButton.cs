using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyButton : MonoBehaviour
{
    [SerializeField]
    private Text LobbyId;
    private Button button;
    
    public Lobby lobby;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(delegate(){
            LobbyUIManager.instance.Ingame(lobby.id);
        });
        button = gameObject.GetComponent<Button>();
        ChangeStatus();

    }

    public void Hienthi(){
        LobbyId.text = lobby.id;
    }
    public void ChangeStatus(){
        if(lobby.currentState == "Game"){
            LobbyId.color = Color.red;
            button.interactable = false;
        }else if(lobby.currentState == "Lobby"){
            LobbyId.color = Color.green;
            button.interactable = true;
        }
    }
}
