using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletManager : MonoBehaviour
{
}
public interface IBullet{
    Sprite ShowSprite();
}
public class BlueBullet : IBullet
{
    public Sprite spriteBullet;
    public Sprite ShowSprite()
    {
       spriteBullet = Resources.Load<Sprite>("Load/Bullet/01");
       return spriteBullet;
    }
}

public class RedBullet : IBullet
{
    public Sprite spriteBullet;
    public Sprite ShowSprite(){
        spriteBullet = Resources.Load<Sprite>("Load/Bullet/02");
        return spriteBullet;
    }
}

