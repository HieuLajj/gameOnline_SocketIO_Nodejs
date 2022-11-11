using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class win : MonoBehaviour
{
    //public int flag = 0;
    private bool isTiming = false;
    public float timeTillKeyIsPressed = 0;
    [SerializeField]
    private int team;
    public SpriteRenderer spriteRenderer;
    [SerializeField]
    private Image image;
    [SerializeField]
    private Image imageFill;
    private float progress = 0f;
    private void Start() {
        if(team == 0){
            imageFill.color = Color.red;
            image.color = Color.blue;
            spriteRenderer.color = Color.blue;
        }else if(team == 1){
            imageFill.color = Color.blue;
            image.color = Color.red;
            spriteRenderer.color = Color.red;
        }
        imageFill.fillAmount = 0;
    }
    
    void Update()
    {
        if(isTiming){
            if(imageFill.fillAmount == 1){ Win(); return;}
            StartCoroutine(Up(0.01f));
        }else{
            if(progress>0f){
                StartCoroutine(Down(0.01f));
            }
        }
        imageFill.fillAmount = progress;
    }
    private IEnumerator Up(float waitTime)
    {
        Debug.Log("co chay");
            yield return new WaitForSeconds(waitTime);
            progress +=0.01f;
    }
    private IEnumerator Down(float waitTime)
    {
        Debug.Log("co chay");
            yield return new WaitForSeconds(waitTime);
            progress -=0.01f;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            int teamOther = other.gameObject.GetComponent<PlayerController>().team;
            if(teamOther != this.team){
                isTiming = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        isTiming = false;
        timeTillKeyIsPressed = 0;
    }
    private void Win(){
        Debug.Log("Ban da chien thang");
        NetworkManager.instance.BackLobby();
        Destroy(gameObject);
    }
    
}
