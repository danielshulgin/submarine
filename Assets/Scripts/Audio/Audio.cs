using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    AudioSource source;

    public List<AudioClip> onDestroy;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        if (onDestroy.Count > 0)
        {
            GameObject go = Instantiate(AudioManager.instance.audioPlayerPrefab, this.transform.position, Quaternion.identity);
            go.GetComponent<AudioSource>().PlayOneShot(onDestroy[Random.Range(0, onDestroy.Count - 1)]);
        }
    }

}
