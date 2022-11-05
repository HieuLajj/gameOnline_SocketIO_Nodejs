using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGod : MonoBehaviour
{
     private bool isTiming = false;
    public float timeTillKeyIsPressed = 0;
    [SerializeField]
    private int team = 0;
    // Start is called before the first frame update
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
    void Update()
    {
        if(isTiming){
            timeTillKeyIsPressed += Time.deltaTime;
        }
        if( timeTillKeyIsPressed > 3f){
            //NetworkManager.instance.ComandChangeTeam();
            isTiming = false;
        }
        
    }

}
