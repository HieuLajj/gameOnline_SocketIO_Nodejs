using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

    public class MenuManager : MonoBehaviour {

        [SerializeField]
        private Button signInBtn;
        [SerializeField]
        private Button signUpBtn;
        public TMP_InputField emailText;
        public TMP_InputField passwordText;
        [SerializeField]
        private Button chuyendoiBtn;
        [SerializeField]
        private TextMeshProUGUI chuyendoiText;
        private bool flagChuyendoi = false;
        [SerializeField]
        private GameObject LoginCanvas;
        [SerializeField]
        private GameObject SignupCanvas;

        [SerializeField] private TMP_InputField nameIFSignUp;
        [SerializeField] private TMP_InputField passwordIFSignUp;
        [SerializeField] private TMP_InputField emailIFSignUp;
        
        public void Start() {
            chuyendoi();
            signInBtn.onClick.AddListener(()=>{
                // NetworkManager.instance.stringname = namePlayerText.text;
                // NetworkManager.instance.ConnectSocket();
                StartCoroutine(PostDataSignIn());
            });
            signUpBtn.onClick.AddListener(()=>{
                StartCoroutine(PostSignup());
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
                if(request.result == UnityWebRequest.Result.ProtocolError){
                    ConfirmationCanvas.instance.Thongbao("Khong ket noi duoc may chu");
                }else{
                    try{
                        ProfileJSON profilejson = JsonUtility.FromJson<ProfileJSON>(request.downloadHandler.text);
                        if(profilejson.name != null){
                            ProfilePlayer.Instance.ChangeProfile(profilejson.id, profilejson.name, profilejson.email, profilejson.win, profilejson.lose, profilejson.token);
                            SceneManagementManager.LoadLevel(SceneList.PROFILE_SCENE);
                            ConfirmationCanvas.instance.Thongbao("Dang nhap thanh cong");
                        }else{
                            ConfirmationCanvas.instance.Thongbao("Dang nhap that bai");
                        }
                    }
                    catch{
                        ConfirmationCanvas.instance.Thongbao("Co loi xay ra");
                        //Debug.Log("co loi xay ra khong ket noi duoc voi may chu");
                    }
                }
            }
        }

        IEnumerator PostSignup(){
            string uri = "http://localhost:3000/laihieu/user/add_user";
            WWWForm form = new WWWForm();
            form.AddField("name",nameIFSignUp.text);
            form.AddField("email",emailIFSignUp.text);
            form.AddField("password",passwordIFSignUp.text);
            using(UnityWebRequest request = UnityWebRequest.Post(uri,form)){
                yield return request.SendWebRequest();
                if(request.result == UnityWebRequest.Result.ProtocolError){
                    ConfirmationCanvas.instance.Thongbao("Dang ki that bai");
                }else{
                    try{
                        ConfirmationCanvas.instance.Thongbao("Dang ki thanh cong");
                    }catch{
                        ConfirmationCanvas.instance.Thongbao("Co loi xay ra");
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
            public string token;
            public static ProfileJSON CreateFromJSON(string data){
                return JsonUtility.FromJson<ProfileJSON>(data);
            }
        }
    }
