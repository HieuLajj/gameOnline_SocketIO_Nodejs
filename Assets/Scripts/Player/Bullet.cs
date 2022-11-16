using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    public int teamPlayerFrom;
    [SerializeField]
    private int movespeed = 1;
    private Vector3 direction = Vector3.right;
    public int a;
    public SpriteRenderer spriteRenderer;
    public IBullet bulletSprite;
    public GameObject playerFrom;
    public UnityEvent OnHit = new UnityEvent();
    public UnityEvent OnHit2 = new UnityEvent();
    public void Bung(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        switch(a){
            case 0:
                bulletSprite = new BlueBullet();
                break;
            case 1:
                bulletSprite = new RedBullet();
                break;
            case 2:
                bulletSprite = new BlueBullet2();
                break;
            case 3:
                bulletSprite = new RedBullet2();
                break;
            case 4:
                bulletSprite = new RedBullet3();
                break;
            default:
                bulletSprite = new RedBullet();
                break;
        }  
        spriteRenderer.sprite = bulletSprite.ShowSprite();
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.tag=="Static"){
            OnHit?.Invoke();
            // Destroy(this.gameObject);
            this.gameObject.SetActive(false);
        }
        if (other.tag != "Player") return; 
        var hit = other.gameObject;
        PlayerController playerHit = hit.GetComponent<PlayerController>();
        if(hit.name != playerFrom.name && playerHit.team != teamPlayerFrom){
            //Debug.Log(hit.name);
            var health = hit.GetComponent<Health>();
            if(health != null){
                health.TakeDamage(playerFrom,10);
            }
            OnHit2?.Invoke();
            this.gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }

    private void Start() {
        Bung();
    }
    
    private void Update() {
        transform.Translate(this.direction * this.movespeed * Time.deltaTime); 
    }
}
