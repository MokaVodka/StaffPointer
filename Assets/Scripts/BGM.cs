using System;

using UnityEngine;

public class BGM : MonoBehaviour
{
    [SerializeField]
    private AudioSource aud_src;

    [SerializeField]
    private AudioClip[] bgms;

    [SerializeField]
    private int index_offset;

    void Start()
    {
        int clip_to_play = (Game.round - index_offset)%bgms.Length;
        aud_src.clip = bgms[clip_to_play];
        aud_src.Play();
    }
}