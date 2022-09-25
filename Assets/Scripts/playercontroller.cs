using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class playercontroller : MonoBehaviour
{
    private bool isFacingRightl = true;
    private float movementHorizontal;
    private float movementVertical;
    public float indexFlip;
    public bool isWalking = false;
    public Animator anim;
    public TextMeshProUGUI name;
    private float speed = 10.0f;
    public Vector2 direction;
    public Vector2 directionClient;
    private Rigidbody2D rb;

    // :)) network ?
    public GameObject bulletPrefab;
    public Transform body;
    public Transform bulletSpawn;
    public Transform centerGun;
    public bool isLocalPlayer = false;
    public Vector2 oldPosition;
    public Vector2 currentPosition;
    private int m=0;
    
    public Transform transform2;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform2 = GetComponent<Transform>();
        oldPosition = transform2.position;
        currentPosition = transform2.position;
    }

    void Update()
    {
        Check();
        if(isLocalPlayer){
            currentPosition = transform2.position;
            CheckInput();
            if(currentPosition != oldPosition){
            NetworkManager.instance.GetComponent<NetworkManager>().ComandMove(transform2.position);
        }
        }
        
        currentPosition = transform2.position;
        indexFlip = currentPosition.x - oldPosition.x;
        if(currentPosition != oldPosition){
           m=0;
            if(!isWalking){
                isWalking = true;
            }
            oldPosition = currentPosition;
        }
        else{
            m++;
            if(m==10){
                isWalking = false;
                m=0;
            }
        }
        UpdateAnimation(); 
        if(Input.GetKeyDown(KeyCode.Space)){
            CmdFire();
        }
    }

    public void CmdFire(){
       var bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation) as GameObject;
       Bullet b = bullet.GetComponent<Bullet>();
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
        direction = new Vector2(movementHorizontal,movementVertical);
        rb.velocity = direction.normalized*speed;
    }
    
    private void Check(){
        if(isFacingRightl && indexFlip < 0){
            Flip();
        }else if(!isFacingRightl && indexFlip > 0){
            Flip();
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
