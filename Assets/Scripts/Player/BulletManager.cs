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
public class BlueBullet2 : IBullet
{
    public Sprite spriteBullet;
    public Sprite ShowSprite()
    {
       spriteBullet = Resources.Load<Sprite>("Load/Bullet/11");
       return spriteBullet;
    }
} 
public class RedBullet2 : IBullet
{
    public Sprite spriteBullet;
    public Sprite ShowSprite(){
        spriteBullet = Resources.Load<Sprite>("Load/Bullet/14");
        return spriteBullet;
    }
}
public class RedBullet3 : IBullet
{
    public Sprite spriteBullet;
    public Sprite ShowSprite(){
        spriteBullet = Resources.Load<Sprite>("Load/Bullet/33");
        return spriteBullet;
    }
}

