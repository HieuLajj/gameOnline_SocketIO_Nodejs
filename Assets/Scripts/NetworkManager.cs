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
    
    private void Awake() {
        if(instance == null){
            instance = this;
        }
        else if (instance != this){
            Destroy(gameObject);
            Debug.Log("?");
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        socketManager = new SocketManager(new Uri("http://localhost:3000"));
        socketManager.Socket.On("connection",()=>{
            Debug.Log("hahaha");
            Debug.Log(socketManager.Socket);
            Debug.Log("hihihi");
            Debug.Log(socketManager.Socket.Id);});        
        
        //socketManager.Socket.On("enemies", OnEnemies);
		socketManager.Socket.On("other player connected",(String data)=>{OnOtherPlayerConnected(data);});
        socketManager.Socket.On("play", (String data)=>{OnPlay(data);});
		socketManager.Socket.On("player move", (String data)=>{OnPlayerMove(data);});
        socketManager.Socket.On("weapon rotation",(String data)=>{OnPlayerWeaponRotation(data);});
        socketManager.Socket.On("selected gun",(String data)=>{SelectedGun(data);});
        socketManager.Socket.On("player shoot",(String data)=> {OnPlayerShoot(data);});
		// socketManager.Socket.On("player shoot", OnPlayerShoot);
		// socketManager.Socket.On("health", OnHealth);
		// socketManager.Socket.On("other player disconnected", OnOtherPlayerDisconnect);
        JoinGame();
    }


    public void OnOtherPlayerConnected(String data){
        UserJSON playerinfomation = JsonUtility.FromJson<UserJSON>(data);
        Vector3 position = new Vector3(playerinfomation.position[0], playerinfomation.position[1], playerinfomation.position[2]);
        Quaternion rotation =  Quaternion.Euler(0f,0f,0f);
        GameObject g = Instantiate(player, position, rotation) as GameObject;
        playercontroller pc = g.GetComponent<playercontroller>();
        Transform t  = g.transform.Find("Healthbar_Canvas");
        Transform t1 = t.transform.Find("PlayerName");
        TextMeshProUGUI playerName = t1.GetComponent<TextMeshProUGUI>();
        playerName.text = playerinfomation.name;
        g.name = playerinfomation.name;
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

    public void OnPlayerShoot(String data){
        ShootJSON shootJSON = ShootJSON.CreateFromJSON(data);
        GameObject p = GameObject.Find(shootJSON.name);
        playercontroller pc = p.GetComponent<playercontroller>();
        pc.CmdFire();
    }
    
    public void SelectedGun(String data){
        Debug.Log(data);
        UserJSON userJSON = JsonUtility.FromJson<UserJSON>(data);
        int selected = userJSON.selectedGun;
		if (userJSON.name == textName)
		{
			return;
		}
		GameObject p = GameObject.Find(userJSON.name) as GameObject;
		if (p != null)
		{
            playercontroller pc = p.GetComponent<playercontroller>();
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
            playercontroller pc = p.GetComponent<playercontroller>();
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


    public void OnPlay(String data){
        UserJSON playerinfomation = JsonUtility.FromJson<UserJSON>(data);
        Vector3 position = new Vector3(playerinfomation.position[0], playerinfomation.position[1], playerinfomation.position[2]);
        Quaternion rotationWeapon = Quaternion.Euler(playerinfomation.rotationWeapon[0],playerinfomation.rotationWeapon[1],playerinfomation.rotationWeapon[2]);
        Quaternion rotation =  Quaternion.Euler(0f,0f,0f);    
        GameObject g = Instantiate(player, position, rotation) as GameObject;
        playercontroller pc = g.GetComponent<playercontroller>();
        
        GameObject weapon = pc.weaponGun.gameObject;
        weapon.transform.rotation = rotationWeapon;
       
        PlayerAimWeapon aim = weapon.GetComponent<PlayerAimWeapon>();
        aim.isLocalPlayer = true;
       

        Transform t  = g.transform.Find("Healthbar_Canvas");
        Transform t1 = t.transform.Find("PlayerName");
        TextMeshProUGUI playerName = t1.GetComponent<TextMeshProUGUI>();
        playerName.text = playerinfomation.name;
        textName = playerinfomation.name;
        pc.isLocalPlayer = true;
        g.name = playerinfomation.name;
        vCam.Follow = g.transform;
        //Debug.Log("dang tao");
    }

    public void JoinGame(){
        StartCoroutine(ConnectToServer());
    }
    IEnumerator ConnectToServer(){
        yield return new WaitForSeconds(0.5f);
        socketManager.Socket.Emit("player connect");
        yield return new WaitForSeconds(1f);
        int h = UnityEngine.Random.Range(0, 100);
        string playerName = "hieulaiday"+h;
         List<SpawnPoint> playerSpawnPoints = GetComponent<PlayerSpawner>().playerSpawnPoints;
        // List<SpawnPoint> enemySpawnPoints = GetComponent<EnemySpawner>().enemySpawnPoints;
        //PlayerJSON playerJSON = new PlayerJSON(playerName, playerSpawnPoints, enemySpawnPoints);
        PlayerJSON playerJSON = new PlayerJSON(playerName, playerSpawnPoints);
        string data = JsonUtility.ToJson(playerJSON);
        socketManager.Socket.Emit("play", data);
    }

    [Serializable]
    public class PlayerJSON{
        public string name;
        public List<PointJSON> playerSpawnPoints;
        public PlayerJSON(string _name, List<SpawnPoint> _playerSpawnPoints){
            name = _name;
            playerSpawnPoints = new List<PointJSON>();
           
            foreach(SpawnPoint playerSpawnPoint in _playerSpawnPoints){
                PointJSON pointJSON = new PointJSON(playerSpawnPoint);
                playerSpawnPoints.Add(pointJSON);
            }
            
        }
    }

    [Serializable]
    public class PointJSON{
        public float[] position;
        public PointJSON(SpawnPoint spawnPoint){
            position = new float[]{
                spawnPoint.transform.position.x,
                spawnPoint.transform.position.y,
                spawnPoint.transform.position.z
            };
        }
    }

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
        public string name;
        public float[] position;
        public float[] rotation;
        public float[] rotationWeapon;
        public int selectedGun;
        public int health;
        public static UserJSON CreateFromJSON(string data){
            return JsonUtility.FromJson<UserJSON>(data);
        }
    }

    [Serializable]
    public class HealthChangeJSON{
        public string name;
        public int healthChange;
        public string from;
        public bool isEnemy;
        public HealthChangeJSON(string _name, int _healthChange, string _from, bool _isEnemy){
            name = _name;
            healthChange = _healthChange;
            from = _from;
            isEnemy = _isEnemy;
        }
    }

    [Serializable]
    public class EnemiesJSON{
        public List<UserJSON> enemies;
        public static EnemiesJSON CreateFromJSON(string data){
            return JsonUtility.FromJson<EnemiesJSON>(data);
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
