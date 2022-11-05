using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public IGun gunPlayer;
    public SpriteRenderer spriteRenderer;
    public void SwitchGunWeapon(int h){
        spriteRenderer = GetComponent<SpriteRenderer>();
        switch(h){
            case 0:
                gunPlayer = new Sungtruong();
                break;
            case 1:
                gunPlayer = new ShotGun();
                break;
            case 2:
                gunPlayer = new Shot1();
                break;
            case 3:
                gunPlayer = new Shot2();
                break;
            case 4:
                gunPlayer = new Shot3();
                break;
            default:
                gunPlayer = new Sungtruong();
                break;
        }
        spriteRenderer.sprite = gunPlayer.ShowSprite();
    }
    public bool isAutomatic;
    public float timeBetweenShots = .1f, heatPerShot = 1f;
}
