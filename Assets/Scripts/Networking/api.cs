using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class api : MonoBehaviour
{
    IEnumerator RaiseWin(){
        string uri = "http://localhost:3000/laihieu/user/increasewin";
        WWWForm form = new WWWForm();
        using(UnityWebRequest request = UnityWebRequest.Post(uri,form)){
            request.SetRequestHeader("Authorization","jwt eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiI2MzZjOWY1NmYxMjJmNTkwODJjOWIzZjAiLCJpYXQiOjE2NjgzMDU5NTAsImV4cCI6MTY5OTg2MzU1MH0.Aj8a7-X6VD39oezL1ZkE3vCXXuyxfkG3DevyYhe4NTU");
            yield return request.SendWebRequest();
        if(request.result == UnityWebRequest.Result.ProtocolError){
            ConfirmationCanvas.instance.Thongbao("Khong ket noi duoc may chu");
        }else{    
            Debug.Log("hahaha"+request.downloadHandler.text);
        }
        }    
    }
}
