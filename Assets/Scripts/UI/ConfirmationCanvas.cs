using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConfirmationCanvas : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textMeshPro;
    public GameObject canvasMain;
    public static ConfirmationCanvas instance;
    private void Awake() {
        canvasMain.SetActive(false);
        if(instance == null){
            instance = this;
        }
        else if (instance != this){
            Destroy(gameObject);
        }
    }
    public void Thongbao(string tinthongbao){
        this.canvasMain.SetActive(true);
        textMeshPro.text = tinthongbao;
    }
}
