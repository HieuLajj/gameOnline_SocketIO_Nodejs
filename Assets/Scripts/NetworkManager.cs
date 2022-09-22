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
		// socketManager.Socket.On("play", OnPlay);
		socketManager.Socket.On("player move", (String data)=>{OnPlayerMove(data);});
		// socketManager.Socket.On("player shoot", OnPlayerShoot);
		// socketManager.Socket.On("health", OnHealth);
		// socketManager.Socket.On("other player disconnected", OnOtherPlayerDisconnect);
        JoinGame();
    }


    public void OnOtherPlayerConnected(String data){
        Debug.Log(data);
        Debug.Log("---------------------");
        PlayerInfomationJson playerinfomation = JsonUtility.FromJson<PlayerInfomationJson>(data);
        Vector3 position = new Vector3(playerinfomation.position[0], playerinfomation.position[1], playerinfomation.position[2]);
        Quaternion rotation =  Quaternion.Euler(0f,0f,0f);
        GameObject g = Instantiate(player, position, rotation) as GameObject;
        playercontroller pc = g.GetComponent<playercontroller>();
        Transform t  = g.transform.Find("Healthbar_Canvas");
        Transform t1 = t.transform.Find("PlayerName");
        TextMeshProUGUI playerName = t1.GetComponent<TextMeshProUGUI>();
        playerName.text = playerinfomation.name;
        g.name = playerinfomation.name;
        Debug.Log(position);
        Debug.Log(rotation);
    }

    public void OnPlayerMove(String data){
        Debug.Log("danga chay"+data);
        UserJSON userJSON = JsonUtility.FromJson<UserJSON>(data);
        Vector3 position = new Vector3(userJSON.position[0], userJSON.position[1], userJSON.position[2]);
        Debug.Log(position);
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

    public void ComandMove(Vector3 vec3){
        string data = JsonUtility.ToJson(new PositionJSON(vec3));
        socketManager.Socket.Emit("player move", data);
    }

    public void OnPlay(String data){
        PlayerInfomationJson playerinfomation = JsonUtility.FromJson<PlayerInfomationJson>(data);
        Vector3 position = new Vector3(playerinfomation.position[0], playerinfomation.position[1], playerinfomation.position[2]);
        Quaternion rotation =  Quaternion.Euler(0f,0f,0f);    
        GameObject g = Instantiate(player, position, rotation) as GameObject;
        playercontroller pc = g.GetComponent<playercontroller>();
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
        List<SpawnPoint> enemySpawnPoints = GetComponent<EnemySpawner>().enemySpawnPoints;
        PlayerJSON playerJSON = new PlayerJSON(playerName, playerSpawnPoints, enemySpawnPoints);
        string data = JsonUtility.ToJson(playerJSON);
        Debug.Log("?");
        socketManager.Socket.Emit("play", data);
    }

    [Serializable]
    public class PlayerJSON{
        public int i=0;
        public int j=0;
        public string name;
        public List<PointJSON> playerSpawnPoints;
        public List<PointJSON> enemySpawnPoints;
        public PlayerJSON(string _name, List<SpawnPoint> _playerSpawnPoints, List<SpawnPoint> _enemySpawnPoints ){
            playerSpawnPoints = new List<PointJSON>();
            enemySpawnPoints = new List<PointJSON>();
            name = _name;
            foreach(SpawnPoint playerSpawnPoint in _playerSpawnPoints){
                PointJSON pointJSON = new PointJSON(playerSpawnPoint);
                playerSpawnPoints.Add(pointJSON);
            }
            foreach(SpawnPoint enemySpawnPoint in _enemySpawnPoints){
                PointJSON pointJSON = new PointJSON(enemySpawnPoint);
                enemySpawnPoints.Add(pointJSON);
            }
        }
    }

    [Serializable]
    public class PlayerInfomationJson{
        public string name;
        public  float[] position;
        public int health;

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
    public class UserJSON{
        public string name;
        public float[] position;
        public float[] rotation;
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
