using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class playercontroller : MonoBehaviour
{
    private bool isFacingRightl = true;
    private float movementHorizontal;
    private float movementVertical;
    private bool isWalking = false;
    public Animator anim;
    public TextMeshProUGUI name;
    private float speed = 10.0f;
    private Rigidbody2D rb;

    // :)) network ?
    public GameObject bulletPrefab;
    public Transform body;
    public Transform bulletSpawn;
    public Transform centerGun;
    public bool isLocalPlayer = false;
    Vector2 oldPosition;
    Vector2 currentPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        oldPosition = transform.position;
        currentPosition = transform.position;
    }

    void Update()
    {
        if(isLocalPlayer){
            CheckInput();
            Check();
        }
        UpdateAnimation();

        if(!isLocalPlayer){
            return;
        }    
        currentPosition = transform.position;

        if(currentPosition != oldPosition){
            NetworkManager.instance.GetComponent<NetworkManager>().ComandMove(transform.position);
            oldPosition = currentPosition;
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            CmdFire();
        }
    }

    public void CmdFire(){
       var bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation) as GameObject;
       Bullet b = bullet.GetComponent<Bullet>();
       //b.playerFrom = this.gameObject
       b.GetComponent<Rigidbody2D>().velocity = (bulletSpawn.position - centerGun.position) * 6;
       Destroy(bullet,2.0f);
    }
    private void FixedUpdate() {
        Movement();
    }
    private void CheckInput(){
        movementHorizontal = Input.GetAxisRaw("Horizontal");
        movementVertical = Input.GetAxisRaw("Vertical");

    }
    private void Movement(){
        rb.velocity = new Vector2(movementHorizontal,movementVertical).normalized*speed;
    }
    
    private void Check(){
        if(isFacingRightl && movementHorizontal < 0){
            Flip();
        }else if(!isFacingRightl && movementHorizontal > 0){
            Flip();
        }
        if(rb.velocity.x!=0 || rb.velocity.y!=0){
            isWalking = true;
        }else{
            isWalking = false;
        }
    }
    private void Flip(){
        isFacingRightl =! isFacingRightl;
        body.Rotate(0.0f,180.0f,0.0f);
    }
    private void UpdateAnimation(){
        anim.SetBool("isWalking",isWalking);
    }
}
