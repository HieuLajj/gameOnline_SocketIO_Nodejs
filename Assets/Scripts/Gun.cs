using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public IGun gunSprite;
    public SpriteRenderer spriteRenderer;
    public void SwitchGunWeapon(int h){
        spriteRenderer = GetComponent<SpriteRenderer>();
        switch(h){
            case 0:
                gunSprite = new Sungtruong();
                break;
            case 1:
                gunSprite = new ShotGun();
                break;
            default:
                gunSprite = new Sungtruong();
                break;
        }
        spriteRenderer.sprite = gunSprite.ShowSprite();
    }
    public bool isAutomatic;
    public float timeBetweenShots = .1f, heatPerShot = 1f;
}
