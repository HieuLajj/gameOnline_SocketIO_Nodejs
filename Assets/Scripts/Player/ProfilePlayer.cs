using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfilePlayer : Singleton<ProfilePlayer>
{
    // Start is called before the first frame update
    public string id;
    public string name;
    public string email;
    public int win;
    public int lose;
    public void ChangeProfile(string _id, string _name, string _email, int _win, int _lose){
        id = _id;
        name = _name;
        email = _email;
        win = _win;
        lose = _lose;
    }
}
