using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicgun : MonoBehaviour
{
     [SerializeField] private AudioClip music1;
    void Start()
    {
        AudioManager.Instance.PlaySFX(music1);
    }
}
