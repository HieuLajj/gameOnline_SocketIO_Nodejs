using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get{
            if(instance == null){
                instance = FindObjectOfType<AudioManager>();
                if(instance == null){
                    instance = new GameObject("Spawned AudioManager", typeof(AudioManager)).GetComponent<AudioManager>();
                }
            }
            return instance;
        }
        private set{
            instance = value;
        }
    }
    
    private bool firstMusicSourceIsPlaying;

    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    private AudioSource musicSource2;
    [SerializeField]
    private AudioSource sfxSource;
    public float volumeFloat;

    private void Awake() {
        musicSource = this.gameObject.AddComponent<AudioSource>();
        musicSource2 = this.gameObject.AddComponent<AudioSource>();
        sfxSource = this.gameObject.AddComponent<AudioSource>();
    
        musicSource.loop = true;
        musicSource2.loop = true;
    }

    public void PlayMusic(AudioClip musicClip){
        AudioSource activeSource = (firstMusicSourceIsPlaying) ? musicSource : musicSource2;
        activeSource.clip = musicClip;
        activeSource.volume = volumeFloat;
        activeSource.Play();
    }

    public void Pause(){
        musicSource.Pause();
        musicSource2.Pause();
    }

    public void ChangeVolume(float volume){
        volumeFloat = volume;
        musicSource.volume = volumeFloat;
        musicSource2.volume = volumeFloat;
        sfxSource.volume = volumeFloat;
    }

    public void PlaySFX(AudioClip clip){
        sfxSource.PlayOneShot(clip);      
    }

    public void PlaySFX(AudioClip clip, float volume){
        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayMusicWithFade(AudioClip newClip, float transitionTime = 1.0f){
        AudioSource activeSource = (firstMusicSourceIsPlaying) ? musicSource : musicSource2;
        StartCoroutine(UpdateMusicWithFade(activeSource, newClip, transitionTime));
    }

    private IEnumerator UpdateMusicWithFade(AudioSource activeSource, AudioClip newClip, float transitionTime){
        if(!activeSource.isPlaying){
            activeSource.Play();
        }

        float t = 0.0f;
        for(t = 0; t < transitionTime; t+= Time.deltaTime){
            activeSource.volume = (1 - (t / transitionTime));
            yield return null;
        }
        activeSource.Stop();
        activeSource.clip = newClip;
        activeSource.Play();

        // Fade in
        for( t = 0; t < transitionTime; t+= Time.deltaTime){
            float h = t/transitionTime;
            if(h<volumeFloat){
                activeSource.volume = (t/transitionTime);
            }
            yield return null;
        }
    }

}
