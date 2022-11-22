using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Music Manager class adapted from: https://stackoverflow.com/questions/27911324/play-continuous-music-when-swapping-between-multiple-scene-in-unity3d
*/

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public AudioClip menuMusic;
    public AudioClip gameMusic;

    private AudioSource audioSource;

    private static MusicManager _instance;

    public static MusicManager Instance {
        get {
            if (_instance == null) {
                _instance = GameObject.FindObjectOfType<MusicManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    void Awake() {
        if(_instance == null) {
            _instance = this;
            DontDestroyOnLoad(this);

            audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.volume = 0.5f;
        } else {
            if(this != _instance)
                Destroy(this.gameObject);
        }
    }

    public void PlayMenuMusic() {
        audioSource.Stop();
        audioSource.clip = menuMusic;
        audioSource.Play();
    }

    public void PlayGameMusic() {
        audioSource.Stop();
        audioSource.clip = gameMusic;
        audioSource.Play();
    }
}
