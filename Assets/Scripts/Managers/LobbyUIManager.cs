using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyUIManager : MonoBehaviour
{
    public static LobbyUIManager instance;   
    public Button playBtn;
    public TMP_InputField namePlayerText;
    public TMP_InputField lobbyText;
    public Transform panelLobby;
    public GameObject buttonTemplate;
    public GameObject LobbyUI;
    public GameObject backButton;
    public GameObject playButton;

    private void Awake() {
        if(instance == null){
            instance = this;
        }
        else if (instance != this){
            Destroy(gameObject);
        }
    }
    void Start()
    {
        backButton.SetActive(false);
        backButton.GetComponent<Button>().onClick.AddListener(delegate(){
            NetworkManager.instance.JoinGame("0");
            LobbyUI.SetActive(true);
            backButton.SetActive(false);
            for(var i = NetworkManager.instance.managerPlayer.transform.childCount-1; i>=0; i--){
                Destroy(NetworkManager.instance.managerPlayer.transform.GetChild(i).gameObject);
            }
        });
        //playBtn.interactable = false;
        playBtn.onClick.AddListener(delegate(){Ingame(lobbyText.text);});  
    
        playButton.SetActive(false);
        playButton.GetComponent<Button>().onClick.AddListener(delegate(){
            NetworkManager.instance.StartGame("phong1");
        });
    }

    // Update is called once per frame
    public void Ingame(string lobbyId){
        NetworkManager.instance.JoinGame(lobbyId);
        LobbyUI.SetActive(false);
        backButton.SetActive(true);
        playButton.SetActive(true);
    }
    public void XoaLobby(Lobby lobby){
        for(var i = panelLobby.childCount-1; i>=0; i--){
            if(panelLobby.GetChild(i).GetComponent<LobbyButton>().lobby.id == lobby.id){
                Destroy(panelLobby.GetChild(i).gameObject);
            }
        }
    }

    public void ThemLobby(Lobby lobby){
        GameObject g = Instantiate(buttonTemplate, panelLobby);
        LobbyButton h = g.GetComponent<LobbyButton>();
        h.lobby = lobby;
        h.Hienthi();
    }
}

public struct Lobby{
    public string id;
    public string currentState;
    public string roommaster;
    public static Lobby CreateFromJSON(string data){
        return JsonUtility.FromJson<Lobby>(data);
    }
}
