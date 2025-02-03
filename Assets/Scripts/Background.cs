using System;

using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    [SerializeField]
    private int index_offset;

    void Start()
    {
        int bg_to_Show = (Game.round - index_offset)%3;
        anim.SetLayerWeight(bg_to_Show, 1);
    }
}