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
    private void Start() {
        if(team == 0){
            spriteRenderer.color = Color.blue;
        }else if(team == 1){
            spriteRenderer.color = Color.red;
        }
    }
    
    void Update()
    {
        if(isTiming){
            timeTillKeyIsPressed += Time.deltaTime;
        }
        if( timeTillKeyIsPressed > 5f){
            Win();
        }
        
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
