using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine.Networking;


public class LobbyUIManager : MonoBehaviour
{
    public static LobbyUIManager instance;   
    public Button createRoomBtn;
    //public TMP_InputField namePlayerText;
    public TMP_InputField lobbyText;
    public Transform panelLobby;
    public GameObject buttonTemplate;
    public GameObject LobbyUI;
    //public Button backButton;
    public Button playButton;
    public Button resetBtn;
    public Button changeTeamBtn;
    public GameObject playerUI;

    public Button StatusButton;
    public bool statusBool = false;

    public Dropdown luachonmap;

    private void Awake() {
        // string jsonResponse = "[{\"name\": \"JD Bots\",\"email\": \"info@jd-bots.com\"},{\"name\": \"Dewiride Creations\",\"email\": \"info@dewiride.com\"}]";
        // //string feoppoop ='[{"_id":"636c9f56f122f59082c9b3f0","email":"laihieu@gmail.com","name":"nguyenthach","password":"$2b$08$19JTSPRlHYaIB2xAha3FweNkPu9T5OIx06/9foJumwu5Z7GB8oiYa","win":33,"lose":6,"__v":0},{"_id":"636e6be06f428828586411b2","email":"daiktb2000@gmail.com","name":"hieulajj","password":"$2b$08$xT0NyuxrxIO/8mrywxQ1.uPFY8sOAdMjps/LUILGLtH0cRFujFaRG","win":9,"lose":13,"__v":0},{"_id":"63738899717f64cd9bc4a019","email":"laihieu999@gmail.com","name":"laivantien","password":"$2b$08$Ma9YWivGnm3IHJl29zhLYuXa4vFuACcydhxBFXvKTnwCnCuPuaiUa","win":2,"lose":0,"__v":0},{"_id":"637388fa717f64cd9bc4a021","email":"laihieu777@gmail.com","name":"tranvietdung","password":"$2b$08$1fqoa3fif/9NFsOLl6jHOukCrbGQdfqvymSU4dk5hjc3LFjflhKba","win":0,"lose":2,"__v":0}]';
        // JArray jArray = JArray.Parse(jsonResponse);
        // foreach (JObject jObject in jArray)
        // {
        //     Debug.Log("--------------------------------------------------------");
        //     Debug.Log($"hahahahaahahahhaha{(string)jObject["name"]} -> {(string)jObject["email"]}");
        // }
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
        AudioManager.Instance.Pause();

        createRoomBtn.onClick.AddListener(()=>{
            Ingame(lobbyText.text);
        });

        resetBtn.onClick.AddListener(()=>{
            this.DeleteAllLobby();
            NetworkManager.instance.ResetRoom();
        });

        changeTeamBtn.onClick.AddListener(()=>{
            NetworkManager.instance.ComandChangeTeam();
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
        //backButton.gameObject.SetActive(true);
        changeTeamBtn.gameObject.SetActive(true);
        
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
    public void LobbyStatus(Lobby lobby){

        for(var i = panelLobby.childCount-1; i>=0; i--){
            LobbyButton lobbyButton = panelLobby.GetChild(i).GetComponent<LobbyButton>();
            if(lobbyButton.lobby.id == lobby.id){
                lobbyButton.lobby = lobby;
                lobbyButton.ChangeStatus();
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
        changeTeamBtn.gameObject.SetActive(false);
        //backButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
        StatusButton.gameObject.SetActive(false);
    }
    public void TroveLobby(){
        HideButtonClient();
        NetworkManager.instance.JoinGame("0");
        NetworkManager.instance.isRoommaster=0;
        LobbyUI.SetActive(true);
        //backButton.gameObject.SetActive(false);
        for(var i = NetworkManager.instance.managerPlayer.transform.childCount-1; i>=0; i--){
            Destroy(NetworkManager.instance.managerPlayer.transform.GetChild(i).gameObject);
        }
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
