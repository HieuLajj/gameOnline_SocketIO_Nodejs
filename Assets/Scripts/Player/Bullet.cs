using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int a;
    public SpriteRenderer spriteRenderer;
    public IBullet bulletSprite;
    public void Bung(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        switch(a){
            case 0:
                bulletSprite = new BlueBullet();
                break;
            case 1:
                bulletSprite = new RedBullet();
                break;
            default:
                bulletSprite = new RedBullet();
                break;
        }  
        spriteRenderer.sprite = bulletSprite.ShowSprite();
    }
    private void Start() {
        Bung();
    }
    public GameObject playerFrom;
    private void OnCollisionEnter2D(Collision2D other) {
        var hit = other.gameObject;
        if(hit.name != playerFrom.name){
            Debug.Log(hit.name);
            var health = hit.GetComponent<Health>();
            if(health != null){
                health.TakeDamage(playerFrom,10);
            }
            Destroy(gameObject);
        }
    }
}
