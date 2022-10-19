using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyUIManager : MonoBehaviour
{
    public static LobbyUIManager instance;
    public List<Lobby> listRoom = new List<Lobby>();    
    public Button playBtn;
    public TMP_InputField namePlayerText;
    public TMP_InputField lobbyText;
    public Transform panelLobby;
    public GameObject buttonTemplate;
    public GameObject LobbyUI;
    public GameObject backButton;

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
            Debug.Log("dang an nhe con");
        });
        playBtn.interactable = false;
        playBtn.onClick.AddListener(delegate(){Ingame(lobbyText.text);});  
    }

    // Update is called once per frame
    public void Ingame(string lobbyId){
        NetworkManager.instance.stringphong = lobbyId;
        NetworkManager.instance.JoinGame();
        LobbyUI.SetActive(false);
        backButton.SetActive(true);
    }
    void Update()
    {
        if(NetworkManager.instance.state == NetworkManager.State.Ready){
            playBtn.interactable = true;
        }else{
            playBtn.interactable = false;
        }
        
    }
    void HienthiLobby(){
        for(var i = panelLobby.childCount-1; i>=0; i--){
            Destroy(panelLobby.GetChild(i));
        }
        int N = listRoom.Count;
        GameObject g;
        for(int i = 0; i< N ; i++ ){
            g = Instantiate(buttonTemplate, panelLobby);
            g.transform.GetChild(0).GetComponent<Text>().text = "Game"+listRoom[i].id; 
        }
    }
    public void XoaLobby(Lobby lobby){
        for(var i = panelLobby.childCount-1; i>=0; i--){
            if(panelLobby.GetChild(i).transform.GetChild(0).GetComponent<Text>().text == lobby.id){
                Destroy(panelLobby.GetChild(i).gameObject);
            }
        }
    }

    public void ThemLobby(Lobby lobby){
        //listRoom.Add(lobby);
        GameObject g = Instantiate(buttonTemplate, panelLobby);
        g.transform.GetChild(0).GetComponent<Text>().text = lobby.id; 
        g.GetComponent<Button>().onClick.AddListener(delegate(){
            LobbyClicked(lobby.id);
        });
    }
    void LobbyClicked(string lobbyId){
        Ingame(lobbyId);
        //Debug.Log(lobbyId);
    }
}

public struct Lobby{
    public string id;
    public string connections;
    public string settings;
    public string lobbyState;
    public static Lobby CreateFromJSON(string data){
        return JsonUtility.FromJson<Lobby>(data);
    }
}
