using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public static AudioSource source;

    public GameObject audioPlayerPrefab;

    private void Awake()
    {
        instance = this;
        source = GetComponent<AudioSource>();
    }
}
