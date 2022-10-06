using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
}
public interface IGun{
    Sprite ShowSprite();
}
public class Sungtruong : MonoBehaviour, IGun {
    public Sprite spriteGun;
    public Sprite ShowSprite(){
        spriteGun = Resources.LoadAll<Sprite>("Load/Gun/Spritesheet_alt")[20];
        return spriteGun;
    }
}

public class ShotGun : MonoBehaviour, IGun {
    public Sprite spriteGun;
    public Sprite ShowSprite(){
        spriteGun = Resources.LoadAll<Sprite>("Load/Gun/Spritesheet_alt")[17];
        return spriteGun;
    }
}
