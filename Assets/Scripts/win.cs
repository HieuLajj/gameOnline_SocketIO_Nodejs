using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class win : MonoBehaviour
{
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
    [SerializeField] private GameObject effect;
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
        effect.SetActive(false);
    }
    
    void Update()
    {
        if(isTiming){
            effect.SetActive(true);
            if(imageFill.fillAmount == 1){ Win(); return;}
            StartCoroutine(Up(0.01f));
        }else{
            effect.SetActive(false);
            if(progress>0f){
                StartCoroutine(Down(0.01f));
            }
        }
        imageFill.fillAmount = progress;
    }
    private IEnumerator Up(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        progress +=0.001f;
    }
    private IEnumerator Down(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        progress -=0.001f;
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
        NetworkManager.instance.BackLobby(team);
        Destroy(gameObject);
    }
    
}
