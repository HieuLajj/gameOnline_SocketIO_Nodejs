using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Project.Managers {
    public class MenuManager : MonoBehaviour {

        [SerializeField]
        private Button queueButton;
        public TMP_InputField namePlayerText;
        public TMP_InputField lobbyText;
        
        public void Start() {

            queueButton.onClick.AddListener(()=>{
                SceneManagementManager.Instance.LoadLevel(SceneList.LOBBY_SCENE);
                NetworkManager.instance.stringname = namePlayerText.text;
                NetworkManager.instance.ConnectSocket();
            });
            
        }
    }
}