using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
}
public interface IGun{
    Sprite ShowSprite();
}
public class Sungtruong : IGun{
    public GameObject g;
    public Sprite spriteGun;
    public Sprite ShowSprite(){
        spriteGun = Resources.LoadAll<Sprite>("Load/Gun/Spritesheet_alt")[17];
        return spriteGun;
    }
}

public class ShotGun : IGun {
    public Sprite spriteGun;
    public Sprite ShowSprite(){
        spriteGun = Resources.LoadAll<Sprite>("Load/Gun/Spritesheet_alt")[20];
        return spriteGun;
    }
}
public class Shot1 : IGun {
    public Sprite spriteGun;
    public Sprite ShowSprite(){
        spriteGun = Resources.LoadAll<Sprite>("Load/Gun/Spritesheet_alt")[21];
        return spriteGun;
    }
}
public class Shot2 : IGun {
    public Sprite spriteGun;
    public Sprite ShowSprite(){
        spriteGun = Resources.LoadAll<Sprite>("Load/Gun/Spritesheet_alt")[23];
        return spriteGun;
    }
}

public class Shot3 : IGun {
    public Sprite spriteGun;
    public Sprite ShowSprite(){
        spriteGun = Resources.LoadAll<Sprite>("Load/Gun/Spritesheet_alt")[24];
        return spriteGun;
    }
}
