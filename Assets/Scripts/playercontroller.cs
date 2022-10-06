using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class playercontroller : MonoBehaviour
{
    public GameObject weaponGun;
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
    public Gun[] allGuns;
    public Gun gun;
    public int selectedGun;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform2 = GetComponent<Transform>();
        oldPosition = transform2.position;
        currentPosition = transform2.position;
        gun.SwitchGunWeapon(selectedGun);
    }

    void Update()
    {
        Check();
        //SwitchWeapon();
        if(isLocalPlayer){
            SwitchWeapon();
            currentPosition = transform2.position;
            CheckInput();
            if(currentPosition != oldPosition){
                NetworkManager.instance.GetComponent<NetworkManager>().ComandMove(transform2.position);
            }
            if(Input.GetKeyDown(KeyCode.Space)){
                CmdFire();
                NetworkManager.instance.GetComponent<NetworkManager>().CommandShoot();
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
    }

    private void SwitchWeapon(){
        if(Input.GetAxisRaw("Mouse ScrollWheel") > 0f){
            ++selectedGun;
            if(selectedGun >= allGuns.Length){
                selectedGun = 0;
            }
            SwitchGun();
        }else if( Input.GetAxisRaw("Mouse ScrollWheel") < 0f){
            --selectedGun;
            if(selectedGun <0){
                selectedGun = allGuns.Length -1;
            }
            SwitchGun();
        }
    }
    private void SwitchGun(){
        //Debug.Log(selectedGun);
        // foreach (Gun gun in allGuns){
        //     gun.gameObject.SetActive(false);
        // }
        // allGuns[selectedGun].gameObject.SetActive(true);
        gun.SwitchGunWeapon(selectedGun);
        NetworkManager.instance.GetComponent<NetworkManager>().ComandSelectedGuns(selectedGun);
    }
    public void ChangeWeapon(int selectedGun2){
        gun.SwitchGunWeapon(selectedGun2);
        // if(!isLocalPlayer){
        //     foreach (Gun gun in allGuns){
        //         gun.gameObject.SetActive(false);
        //     }
        //     allGuns[selectedGun2].gameObject.SetActive(true);
        // }
    }
    

    public void CmdFire(){
       var bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation) as GameObject;
       Bullet b = bullet.GetComponent<Bullet>();
       b.a = selectedGun;
       b.playerFrom = gameObject;
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
