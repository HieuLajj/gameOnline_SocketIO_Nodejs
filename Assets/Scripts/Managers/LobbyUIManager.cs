using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyUIManager : MonoBehaviour
{
    public static LobbyUIManager instance;   
    public Button createRoomBtn;
    public TMP_InputField namePlayerText;
    public TMP_InputField lobbyText;
    public Transform panelLobby;
    public GameObject buttonTemplate;
    public GameObject LobbyUI;
    public Button backButton;
    public Button playButton;
    public Button resetBtn;
    public GameObject playerUI;

    public Button StatusButton;
    public bool statusBool = false;

    public Dropdown luachonmap;

    private void Awake() {
        if(instance == null){
            instance = this;
        }
        else if (instance != this){
            Destroy(gameObject);
        }
        HideButtonClient();
    }
    void Start()
    {
        backButton.onClick.AddListener(delegate(){
            HideButtonClient();
            NetworkManager.instance.JoinGame("0");
            NetworkManager.instance.isRoommaster=0;
            LobbyUI.SetActive(true);
            backButton.gameObject.SetActive(false);
            for(var i = NetworkManager.instance.managerPlayer.transform.childCount-1; i>=0; i--){
                Destroy(NetworkManager.instance.managerPlayer.transform.GetChild(i).gameObject);
            }
        });
        
        createRoomBtn.onClick.AddListener(()=>{
            Ingame(lobbyText.text);
        });

        resetBtn.onClick.AddListener(()=>{
            this.DeleteAllLobby();
            NetworkManager.instance.ResetRoom();
        });

        
        playButton.onClick.AddListener(delegate(){
            NetworkManager.instance.StartGame(Luachonmap());
        });

        StatusButton.onClick.AddListener(delegate(){
            int statusInt;
            statusBool = !statusBool;
            if(statusBool){
                statusInt = 2;
                
            }else{
                statusInt = 0;
            }
            NetworkManager.instance.CommandStatusChange(statusInt);
        });
    }
    public void chuphong(){
        StatusButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(true);
        luachonmap.gameObject.SetActive(true);
    }

    public void khachthamgia(){
        StatusButton.gameObject.SetActive(true);
        playButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void Ingame(string lobbyId){
        NetworkManager.instance.JoinGame(lobbyId);
        LobbyUI.SetActive(false);
        backButton.gameObject.SetActive(true);
    }
    protected void DeleteAllLobby(){
        for(var i = panelLobby.childCount-1; i>=0; i--){
                Destroy(panelLobby.GetChild(i).gameObject);
        }
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

    public string Luachonmap(){
        int m_DropdownValue = luachonmap.value;
        string hh = luachonmap.options[m_DropdownValue].text;
        return hh;
    }

    public void HideButtonClient(){
        luachonmap.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
        StatusButton.gameObject.SetActive(false);
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
