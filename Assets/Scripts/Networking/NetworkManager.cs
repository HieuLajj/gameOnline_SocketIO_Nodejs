using System.Collections;
using BestHTTP.SocketIO3;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Web;
using Cinemachine;
using TMPro;

public class NetworkManager : MonoBehaviour
{
    public CinemachineVirtualCamera vCam;
    public static NetworkManager instance;
    public Canvas canvas;
    public InputField playerNameInput;
    public GameObject player;
    public SocketManager socketManager;

    public string textName;

    public string stringname;
    //public string stringphong;
    public GameObject managerPlayer;
    private void Awake() {
        
        if(instance == null){
            instance = this;
        }
        else if (instance != this){
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {  
        // socketManager = new SocketManager(new Uri("http://localhost:3000"));
        // socketManager.Socket.On("connection",()=>{
        //     Debug.Log("hahaha");
        //     Debug.Log(socketManager.Socket);
        //     Debug.Log("hihihi");
        //     Debug.Log(socketManager.Socket.Id);});        
		// socketManager.Socket.On("other player connected",(String data)=>{OnOtherPlayerConnected(data);});
        // socketManager.Socket.On("play", (String data)=>{OnPlay(data);});
		// socketManager.Socket.On("player move", (String data)=>{OnPlayerMove(data);});
        // socketManager.Socket.On("weapon rotation", (String data)=>{OnPlayerWeaponRotation(data);});
        // socketManager.Socket.On("selected gun", (String data)=>{SelectedGun(data);});
        // socketManager.Socket.On("player shoot", (String data)=> {OnPlayerShoot(data);});
        // socketManager.Socket.On("health", (String data)=>{OnHealth(data);});
		// socketManager.Socket.On("other player disconnected",(String data) =>{OnOtherPlayerDisconnect(data);});
    }

    public void StartGame(String data){
        socketManager.Socket.Emit("start game", data);
    }
    public void OnStartGame(String data){
        SceneManagementManager.Instance.LoadAddScene(data);
    }

    public void BackLobby(){
        socketManager.Socket.Emit("back lobby");
    }

    public void OnBackLobby(String data){
        SceneManagementManager.Instance.UnLoadScene(data);
    }

    public void ConnectSocket(){
        socketManager = new SocketManager(new Uri("http://localhost:3000"));
        socketManager.Socket.On("connection",()=>{
            Debug.Log("laivanhieu");
        });        
		socketManager.Socket.On("other player connected",(String data)=>{OnOtherPlayerConnected(data);});
        socketManager.Socket.On("play", (String data)=>{OnPlay(data);});
		socketManager.Socket.On("player move", (String data)=>{OnPlayerMove(data);});
        socketManager.Socket.On("weapon rotation", (String data)=>{OnPlayerWeaponRotation(data);});
        socketManager.Socket.On("selected gun", (String data)=>{SelectedGun(data);});
        socketManager.Socket.On("player shoot", (String data)=> {OnPlayerShoot(data);});
        socketManager.Socket.On("health", (String data)=>{OnHealth(data);});
		socketManager.Socket.On("other player disconnected",(String data) =>{OnOtherPlayerDisconnect(data);});
        socketManager.Socket.On("list Room",(String data)=>{ListRoom(data);});
        socketManager.Socket.On("xoa Room",(String data)=>{DeleteRoom(data);});
        socketManager.Socket.On("start game",(String data)=>{OnStartGame(data);});
        socketManager.Socket.On("back lobby",(String data)=>{OnBackLobby(data);});
        socketManager.Socket.On("change status", (String data)=>{OnStatusChange(data);});
        socketManager.Socket.On("change roommaster", (String data)=>{OnChangeRoommaster(data);});
    }
    public void OnOtherPlayerConnected(String data){
        UserJSON playerinfomation = JsonUtility.FromJson<UserJSON>(data);

        Vector3 position = new Vector3(playerinfomation.position[0], playerinfomation.position[1], playerinfomation.position[2]);
        Quaternion rotation =  Quaternion.Euler(0f,0f,0f);

        GameObject g = Instantiate(player, position, rotation, managerPlayer.transform) as GameObject;
        PlayerController pc = g.GetComponent<PlayerController>();
       
        pc.ChangeWeapon(playerinfomation.selectedGun);

        pc.Status(playerinfomation.roommaster); 

        Vector3 rotationWeVec3 = new Vector3(playerinfomation.rotationWeapon[0], playerinfomation.rotationWeapon[1], playerinfomation.rotationWeapon[2]);
        pc.weaponGun.gameObject.transform.rotation = Quaternion.Euler(rotationWeVec3);
             
        Health health = g.GetComponent<Health>();
        health.OnChangeHealth(playerinfomation.health);
    
        pc.name.text = playerinfomation.name;
        g.name = playerinfomation.name;
    }

    public void ListRoom(String data){
        Lobby lobby = JsonUtility.FromJson<Lobby>(data);
        LobbyUIManager.instance.ThemLobby(lobby);
    }
    public void DeleteRoom(String data){
        Lobby lobby = JsonUtility.FromJson<Lobby>(data);
        LobbyUIManager.instance.XoaLobby(lobby);
    }

    public void OnOtherPlayerDisconnect (String data){
        UserJSON userJSON = JsonUtility.FromJson<UserJSON>(data);
        Destroy(GameObject.Find(userJSON.name));
    }

    public void OnPlayerMove(String data){
        UserJSON userJSON = JsonUtility.FromJson<UserJSON>(data);
        Vector3 position = new Vector3(userJSON.position[0], userJSON.position[1], userJSON.position[2]);
		// if it is the current player exit
		if (userJSON.name == textName)
		{
			return;
		}
		GameObject p = GameObject.Find(userJSON.name) as GameObject;
		if (p != null)
		{
			p.transform.position = position; 
		}
    }

    public void OnHealth(String data){
        UserHealthJSON userHealthJSON = UserHealthJSON.CreateFromJSON(data);
        GameObject p = GameObject.Find(userHealthJSON.name);
		Health h = p.GetComponent<Health>();
		//h.currentHealth = userHealthJSON.health;
		h.OnChangeHealth(userHealthJSON.health);
    }

    public void OnPlayerShoot(String data){
        ShootJSON shootJSON = ShootJSON.CreateFromJSON(data);
        GameObject p = GameObject.Find(shootJSON.name);
        PlayerController pc = p.GetComponent<PlayerController>();
        pc.CmdFire();
    }
    
    public void SelectedGun(String data){
        UserJSON userJSON = JsonUtility.FromJson<UserJSON>(data);
        int selected = userJSON.selectedGun;
		if (userJSON.name == textName)
		{
			return;
		}
		GameObject p = GameObject.Find(userJSON.name) as GameObject;
		if (p != null)
		{
            PlayerController pc = p.GetComponent<PlayerController>();
            pc.selectedGun = selected;
            pc.ChangeWeapon(selected);
		}
    }

    public void OnPlayerWeaponRotation(String data){
        UserJSON userJSON = JsonUtility.FromJson<UserJSON>(data);
        Vector3 rotationWeVec3 = new Vector3(userJSON.rotationWeapon[0], userJSON.rotationWeapon[1], userJSON.rotationWeapon[2]);
        Quaternion rotationWe = Quaternion.Euler(rotationWeVec3);
		// if it is the current player exit
		if (userJSON.name == textName)
		{
			return;
		}
		GameObject p = GameObject.Find(userJSON.name) as GameObject;
		if (p != null)
		{
            PlayerController pc = p.GetComponent<PlayerController>();
            pc.weaponGun.gameObject.transform.rotation = rotationWe;
		}
    }

    public void ComandMove(Vector3 vec3){
        string data = JsonUtility.ToJson(new PositionJSON(vec3));
        socketManager.Socket.Emit("player move", data);
    }

    public void ComandRotateWeapon(Vector3 vec3){
        string data = JsonUtility.ToJson(new RotationWeaponJSON(vec3));
        socketManager.Socket.Emit("weapon rotation", data);
    }

    public void CommandShoot(){
        socketManager.Socket.Emit("player shoot");
    }

    public void ComandSelectedGuns(int selectedGun){
        string data = JsonUtility.ToJson(new SelectedGunJSON(selectedGun));
        socketManager.Socket.Emit("selected gun",data);
    }
    public void CommandHealthChange(GameObject playerFrom, GameObject playerTo, int healthChange){
        HealthChangeJSON healthChangeJSON = new HealthChangeJSON(playerTo.name, healthChange, playerFrom.name);
		socketManager.Socket.Emit("health", JsonUtility.ToJson(healthChangeJSON));
    }

    public void CommandStatusChange(int StatusUnit){
        socketManager.Socket.Emit("change status", StatusUnit);
    }
    public void OnStatusChange(String data){
        UserJSON userJSON = JsonUtility.FromJson<UserJSON>(data);
        GameObject p = GameObject.Find(userJSON.name);
        PlayerController pc = p.GetComponent<PlayerController>();
        pc.Status(userJSON.roommaster); 
    }

    public void OnChangeRoommaster(String data){
       OnStatusChange(data);
       LobbyUIManager.instance.StatusButton.gameObject.SetActive(false);
    }

    public void OnPlay(String data){
        UserJSON playerinfomation = JsonUtility.FromJson<UserJSON>(data);
        
        Vector3 position = new Vector3(playerinfomation.position[0], playerinfomation.position[1], playerinfomation.position[2]);
        Quaternion rotationWeapon = Quaternion.Euler(playerinfomation.rotationWeapon[0],playerinfomation.rotationWeapon[1],playerinfomation.rotationWeapon[2]);
        Quaternion rotation =  Quaternion.Euler(0f,0f,0f);  
        
        GameObject g = Instantiate(player, position, rotation, managerPlayer.transform) as GameObject;
        PlayerController pc = g.GetComponent<PlayerController>();

        pc.Status(playerinfomation.roommaster); 

        GameObject weapon = pc.weaponGun.gameObject;
        weapon.transform.rotation = rotationWeapon;
       
        PlayerAimWeapon aim = weapon.GetComponent<PlayerAimWeapon>();
        aim.isLocalPlayer = true;
       
        pc.name.text = playerinfomation.name;
        g.name = playerinfomation.name;
        
        textName = playerinfomation.name;
        pc.isLocalPlayer = true;
        
        vCam.Follow = g.transform;

        if(playerinfomation.roommaster == 1 && pc.isLocalPlayer == true){
            LobbyUIManager.instance.StatusButton.gameObject.SetActive(false);
        }else{
            LobbyUIManager.instance.StatusButton.gameObject.SetActive(true);
        }
    }

    public void JoinGame(String stringphong){
        StartCoroutine(ConnectToServer(stringname, stringphong));
    }
    IEnumerator ConnectToServer(String playerName, String lobbyName){
        yield return new WaitForSeconds(0.5f);
        socketManager.Socket.Emit("joinGame", lobbyName);
        if(lobbyName!="0"){
            socketManager.Socket.Emit("player connect");
            PlayerJSON playerJSON = new PlayerJSON(playerName);
            string data = JsonUtility.ToJson(playerJSON);
            socketManager.Socket.Emit("play", data);      
        }
    }

    [Serializable]
    public class PlayerJSON{
        public string name; 
        public PlayerJSON(string _name){
            name = _name;
        }
    }

    // [Serializable]
    // public class PointJSON{
    //     public float[] position;
    //     public PointJSON(SpawnPoint spawnPoint){
    //         position = new float[]{
    //             spawnPoint.transform.position.x,
    //             spawnPoint.transform.position.y,
    //             spawnPoint.transform.position.z
    //         };
    //     }
    // }

    [Serializable]
    public class PositionJSON{
        public float[] position;
        public PositionJSON(Vector3 _position){
            position =  new float[]{_position.x, _position.y, _position.z};
        }
    }

    [Serializable]
    public class RotationWeaponJSON{
        public float[] rotation;
        public RotationWeaponJSON(Vector3 _rotation){
            rotation = new float[]{_rotation.x, _rotation.y, _rotation.z};
        }
    }
    [Serializable]
    public class SelectedGunJSON{
        public int selectedGun;
        public SelectedGunJSON (int _selectedGun){
            selectedGun = _selectedGun;
        }
    }

    [Serializable]
    public class UserJSON{
        public string id;
        public string name;
        public float[] position;
        public float[] rotation;
        public float[] rotationWeapon;
        public int selectedGun;
        public int health;
        public int roommaster;
        public static UserJSON CreateFromJSON(string data){
            return JsonUtility.FromJson<UserJSON>(data);
        }
    }

    [Serializable]
    public class HealthChangeJSON{
        public string name;
        public int healthChange;
        public string from;
        public HealthChangeJSON(string _name, int _healthChange, string _from){
            name = _name;
            healthChange = _healthChange;
            from = _from;
        }
    }

    [Serializable]
    public class ShootJSON{
        public string name;
        public static ShootJSON CreateFromJSON(string data){
            return JsonUtility.FromJson<ShootJSON>(data);
        }
    }

    [Serializable]  
    public class UserHealthJSON{
        public string name;
        public int health;
        public static UserHealthJSON CreateFromJSON(string data){
            return JsonUtility.FromJson<UserHealthJSON>(data);
        }
    }
}
