using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] clips;

    private void Awake()
    {
        audioSource.clip = clips[0];
    }
}
