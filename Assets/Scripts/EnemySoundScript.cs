using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundScript : MonoBehaviour
{
    public AudioClip[] audioClips;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource= GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(int i, float volume)
    {
        audioSource.PlayOneShot(audioClips[i]);
        audioSource.volume = volume;
    }
}
