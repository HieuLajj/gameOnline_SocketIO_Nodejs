using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfilePlayer : MonoBehaviour
{
    public static ProfilePlayer Instance;
    private void Awake() {
        if(Instance == null){
            Instance = this;
        }
        else if (Instance != this){
            Destroy(gameObject);
        }
    }
    public string id;
    public string nameplayer;
    public string email;
    public int win;
    public int lose;
    public int team;
    public string token;
    public void ChangeProfile(string _id, string _name, string _email, int _win, int _lose, string _token){
        id = _id;
        nameplayer = _name;
        email = _email;
        win = _win;
        lose = _lose;
        token = _token;
    }
    public void TangWin(){
        this.win +=1;
    }
    public void TangLose(){
        this.lose +=1;
    }
}
