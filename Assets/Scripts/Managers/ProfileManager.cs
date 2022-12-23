using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json.Linq;

public class ProfileManager : MonoBehaviour
{
    [SerializeField]
    private Button playGameButton;
    [SerializeField]
    public TMP_Text usernameTMP;
    [SerializeField]
    public TMP_Text winTMP;
    [SerializeField]
    public TMP_Text loseTMP;
    public List<OtherPlayer> OtherPlayersList;
    public GameObject PrefabRankItem;
    public Transform PanelRank;
    private bool checkRank = false;
    public GameObject RankCanvas;
    void Start()
    {
        playGameButton.onClick.AddListener(()=>{
            SceneManagementManager.LoadLevel(SceneList.LOBBY_SCENE);
            NetworkManager.instance.ConnectSocket();
        });
        usernameTMP.text = ProfilePlayer.Instance.nameplayer;
        winTMP.text = ProfilePlayer.Instance.win.ToString();
        loseTMP.text = ProfilePlayer.Instance.lose.ToString();
        AudioManager.Instance.Pause();
        StartCoroutine(Bangxephang());
    }
    IEnumerator Bangxephang(){
            string uri = "http://192.168.1.234:3000/laihieu/user/getall";
            using(UnityWebRequest request = UnityWebRequest.Get(uri)){
                request.SetRequestHeader("Authorization",$"jwt {ProfilePlayer.Instance.token}");
                yield return request.SendWebRequest();
                if(request.result == UnityWebRequest.Result.ProtocolError){
                    ConfirmationCanvas.instance.Thongbao("Khong ket noi duoc may chu");
                }else{
                    try{
                        string jsonResponse = request.downloadHandler.text;
                        JArray jArray = JArray.Parse(jsonResponse);
                        foreach (JObject jObject in jArray)
                        {
                            //OtherPlayer otherPlayer = new OtherPlayer((string)jObject["name"],(string)jObject["win"],(string)jObject["lose"]);
                            //Debug.Log($"hahaha{(string)jObject["name"]} -> {(string)jObject["win"]} -> {(string)jObject["lose"]}");
                            GameObject gameObject = Instantiate(PrefabRankItem, PanelRank);
                            RankItem rankItem = gameObject.GetComponent<RankItem>();
                            rankItem.SetItem((string)jObject["name"],(string)jObject["lose"],(string)jObject["win"]);
                            //OtherPlayersList.Add(otherPlayer);
                        } 
                        Debug.Log("THnagnakjfhew==>"+ OtherPlayersList[1].name);     
                    }
                    catch{  
                        Debug.Log("co loi xay ra khong ket noi duoc voi may chu");
                    }
                }
            }
    }
    public void DisplayRank(){
        checkRank = !checkRank;
        if(checkRank){
            RankCanvas.SetActive(true);
        }else{
            RankCanvas.SetActive(false);
        }
    }
    [System.Serializable]
    public class OtherPlayer{
        public string name;
        public string win;
        public string lose;
        public OtherPlayer(string _name, string _win, string _lose){
            name = _name;
            win = _win;
            lose = _lose;
    }
}
}
