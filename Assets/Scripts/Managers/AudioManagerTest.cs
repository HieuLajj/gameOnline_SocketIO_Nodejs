using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerTest : MonoBehaviour
{
    [SerializeField] private AudioClip music1;
    [SerializeField] private AudioClip music2;
    [SerializeField] private AudioClip musuc3;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.C)){
            AudioManager.Instance.PlayMusic(music1);
        }
        if(Input.GetKeyDown(KeyCode.D)){
            AudioManager.Instance.PlayMusicWithFade(music2);
        }
    }
}
