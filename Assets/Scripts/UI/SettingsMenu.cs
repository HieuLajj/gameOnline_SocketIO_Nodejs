using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SettingsMenu : MonoBehaviour
{
    [Header ("space between menu items")]
    [SerializeField] Vector2 spacing;
    Button mainButton;
    SettingsMenuItem[] menuItems;
    bool isExpanded = false;
    Vector2 mainButtonPosition;
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Button backBtn;
    [SerializeField] private Button volumnBtn;

    int itemsCount;
    void Start()
    {
        sliderMusic.gameObject.SetActive(false);
        itemsCount = transform.childCount - 1;
        menuItems = new SettingsMenuItem[itemsCount];
        for(int i=0; i<itemsCount; i++){
            menuItems [i] = transform.GetChild(i+1).GetComponent<SettingsMenuItem>();
        }
        mainButton = transform.GetChild(0).GetComponent<Button>();
        
        mainButton.onClick.AddListener(ToggleMenu);
        backBtn.onClick.AddListener(BackFunction);
        volumnBtn.onClick.AddListener(VolumnFunction);

        mainButton.transform.SetAsLastSibling();

        mainButtonPosition = mainButton.transform.position;
        ResetPositions();
        LoadValues();
        VolumnSlider(sliderMusic.value);
    }

    void VolumnFunction(){
        Debug.Log("OK");
        //  StartCoroutine(PostDataSignIn());
    }
    void ResetPositions(){
        for(int i =0; i<itemsCount; i++){
            menuItems[i].trans.position = mainButtonPosition;
        }
    }
    
    void ToggleMenu(){
        isExpanded = !isExpanded;
        if (isExpanded){
            for(int i = 0; i < itemsCount; i++){
                menuItems[i].trans.position = mainButtonPosition + spacing * (i+1);
                sliderMusic.gameObject.SetActive(true);
            }
        }else{
            for(int i = 0; i < itemsCount; i++){
                menuItems[i].trans.position = mainButtonPosition;
                sliderMusic.gameObject.SetActive(false);
            }
        }
    }

    void BackFunction(){
        int indexScene = SceneManager.GetActiveScene().buildIndex;

        if(LobbyUIManager.instance){
            if(LobbyUIManager.instance.LobbyUI.activeSelf == false && indexScene == 2
                && NetworkManager.instance.statusRoom == "Lobby"
            ){
                LobbyUIManager.instance?.TroveLobby();
                return;
            }else if(LobbyUIManager.instance.LobbyUI?.activeSelf == true && indexScene ==2){
                if(indexScene <= 0)return;
                NetworkManager.instance.Disconnect();
                SceneManager.LoadScene(indexScene-1, LoadSceneMode.Single);
                return;
            }
        }
        if(indexScene == 2){
            Debug.Log("Dang trong tran khong the thoat");
            return;
        }
        if(indexScene <= 0)return;
        if(indexScene == 1){
            // for(int i=0; i< Object.FindObjectsOfType<DontDestroy>().Length; i++){
            //         Destroy(Object.FindObjectsOfType<DontDestroy>()[i]);
            // }
            SceneManager.LoadScene(indexScene-1, LoadSceneMode.Single);
            return;
        }
        SceneManager.LoadScene(indexScene-1, LoadSceneMode.Single);
    }
    

    private void OnDestroy() {
        mainButton?.onClick.RemoveListener(ToggleMenu);
    }

    public void VolumnSlider(float volume){
        AudioManager.Instance.ChangeVolume(volume);
        SaveVolumeButton();
    }

    void SaveVolumeButton(){
        float volumeValue = sliderMusic.value;
        PlayerPrefs.SetFloat("VolumeValue", volumeValue);
    }

    void LoadValues(){
        float volumeValue = PlayerPrefs.GetFloat("VolumeValue");
        sliderMusic.value = volumeValue;
    }

}

