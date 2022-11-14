using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private float movementHorizontal;
    private float movementVertical;
    protected float indexFlip;
    public bool isWalking = false;

    public new TextMeshProUGUI name;
    public TextMeshProUGUI status;
    
    public Vector2 directionClient;
    private Rigidbody2D rb;

    public bool isLocalPlayer = false;
    public bool setAvtivePlayer = true;
    public Vector2 oldPosition;
    public Vector2 currentPosition;
    private int m=0;

    private Transform transformPlayer;
    public int roommaster;

    [SerializeField]
    private SpriteRenderer bodyPlayer;
    public int team;

    [SerializeField]
    private PlayerMovement playerMovement;
    
    public PlayerAnimation playerAnimation;
    public PlayerActivity playerActivity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponent<PlayerAnimation>();
        transformPlayer = GetComponent<Transform>();
        oldPosition = transformPlayer.position;
        currentPosition = transformPlayer.position;
    }

    void Update()
    {
        if (!setAvtivePlayer) return;
        playerMovement.Check(indexFlip);
        //SwitchWeapon();
        if(isLocalPlayer){
            //playerActivity.SwitchWeapon();
            currentPosition = transformPlayer.position;
            CheckInput();
            if(currentPosition != oldPosition){
                NetworkManager.instance.GetComponent<NetworkManager>().ComandMove(transformPlayer.position);
            }
            if(Input.GetMouseButtonDown(0) && NetworkManager.instance.statusRoom == "Game"){
                playerActivity.Shoot(this.team);
                NetworkManager.instance.GetComponent<NetworkManager>().CommandShoot();
            }

            if(Input.GetKeyDown(KeyCode.L)){
                NetworkManager.instance.ComandSelectedGuns(0);
            }
        }
        currentPosition = transformPlayer.position;
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
        playerAnimation.UpdateAnimation(isWalking); 
    }

    private void FixedUpdate() {
        playerMovement.Movement(rb,movementHorizontal,movementVertical);
    }
    private void CheckInput(){
        movementHorizontal = Input.GetAxisRaw("Horizontal");
        movementVertical = Input.GetAxisRaw("Vertical");
    }
    // doi phong
    public void ChangeTeam(int team){
        this.team = team;
        if(isLocalPlayer){
            ProfilePlayer.Instance.team = team;
        }
        if (team == 0){
            this.bodyPlayer.color = Color.blue; 
        }else if(team == 1){
            this.bodyPlayer.color = Color.red; 
        }
    }
    // chuyen doi cac trang thai cua play trong networkmanager
    public void Status(int roommaster){
        this.roommaster = roommaster;
        status.text = StatusUnit(roommaster);
    }

    private string StatusUnit(int status){
        string statusString;
        if(status == 1){
            statusString = "chu phong";
        }
        else if (status == 2){
            statusString = "san sang";
        }
        else{
            statusString = "chua san sang";
        }
        return statusString;
    }
}
