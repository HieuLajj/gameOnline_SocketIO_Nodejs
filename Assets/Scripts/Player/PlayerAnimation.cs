using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    public void UpdateAnimation(bool isWalking){
        anim.SetBool("isWalking",isWalking);
    }
}
