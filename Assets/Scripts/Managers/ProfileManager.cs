using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
    void Start()
    {
        playGameButton.onClick.AddListener(()=>{
            SceneManagementManager.LoadLevel(SceneList.LOBBY_SCENE);
            NetworkManager.instance.ConnectSocket();
        });
        usernameTMP.text = ProfilePlayer.Instance.nameplayer;
        winTMP.text = ProfilePlayer.Instance.win.ToString();
        loseTMP.text = ProfilePlayer.Instance.lose.ToString();
    }

    void Update()
    {
        
    }
}
