using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    protected Vector2 direction;
    private float speed = 10.0f;
    protected bool isFacingRightl = true;
    [SerializeField]
    private Transform body;

    //movement
    public void Movement(Rigidbody2D rb ,float movementHorizontal, float movementVertical){      
        direction = new Vector2(movementHorizontal,movementVertical);
        rb.velocity = direction.normalized*speed;
    }

    public void Check(float indexFlip){
        if(isFacingRightl && indexFlip < 0){
            Flip();
        }else if(!isFacingRightl && indexFlip > 0){
            Flip();
        }
    }
    protected void Flip(){
        isFacingRightl =! isFacingRightl;
        body.Rotate(0.0f,180.0f,0.0f);
    }

}
