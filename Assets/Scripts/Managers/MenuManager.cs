using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

    public class MenuManager : MonoBehaviour {

        [SerializeField]
        private Button queueButton;
        public TMP_InputField emailText;
        public TMP_InputField passwordText;
        [SerializeField]
        private Button chuyendoiBtn;
        [SerializeField]
        private Text chuyendoiText;
        private bool flagChuyendoi = false;
        [SerializeField]
        private GameObject LoginCanvas;
        [SerializeField]
        private GameObject SignupCanvas;
        
        public void Start() {
            chuyendoi();

            queueButton.onClick.AddListener(()=>{
                // NetworkManager.instance.stringname = namePlayerText.text;
                // NetworkManager.instance.ConnectSocket();
                StartCoroutine(PostDataSignIn());
            });
            chuyendoiBtn.onClick.AddListener(()=>{
                flagChuyendoi = !flagChuyendoi;
                chuyendoi();
            });
            
        }
        void chuyendoi(){
            if(flagChuyendoi == true){
                chuyendoiText.text = "dang nhap";
                LoginCanvas.SetActive(false);
                SignupCanvas.SetActive(true);
            }else{
                chuyendoiText.text = "dang ki tai khoan";
                LoginCanvas.SetActive(true);
                SignupCanvas.SetActive(false);
            }
        }

        IEnumerator PostDataSignIn(){
            string uri = "http://localhost:3000/laihieu/user/sign_in";
            WWWForm form = new WWWForm();
            form.AddField("email",emailText.text);
            form.AddField("password",passwordText.text);
            using(UnityWebRequest request = UnityWebRequest.Post(uri,form)){
                yield return request.SendWebRequest();
                if(request.isNetworkError || request.isHttpError){
                    Debug.Log("Co loi xay ra");
                }else{
                    Debug.Log("ok"+request.downloadHandler.text);
                    ProfileJSON profilejson = JsonUtility.FromJson<ProfileJSON>(request.downloadHandler.text);
                    if(profilejson.name != null){
                        ProfilePlayer.Instance.ChangeProfile(profilejson.id, profilejson.name, profilejson.email, profilejson.win, profilejson.lose);
                        SceneManagementManager.Instance.LoadLevel(SceneList.PROFILE_SCENE);
                    }
                }
            }
        }
        [Serializable]
        public class ProfileJSON{
            public string id;
            public string name;
            public string email;
            public int win;
            public int lose;
            public static ProfileJSON CreateFromJSON(string data){
                return JsonUtility.FromJson<ProfileJSON>(data);
            }
        }
    }
