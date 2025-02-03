using UnityEngine;

//Animator helper class
public static class Animator_Ext
{
    //Check if any animation is playing
    public static bool IsPlaying(this Animator animator, int layerIndex)
    {
        return animator.GetCurrentAnimatorStateInfo(layerIndex).length >
               animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime;
    }

    public static bool IsPlayingLoop(this Animator animator, int layerIndex)
    {
        return (animator.IsPlaying(layerIndex) ||
                animator.GetCurrentAnimatorStateInfo(layerIndex).length <=
                animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime) &&
                animator.GetCurrentAnimatorStateInfo(layerIndex).loop;
    }
    
    //Check if animation with stateName is playing
    public static bool IsPlaying(this Animator animator, int layerIndex, string stateName)
    {
        return animator.IsPlaying(layerIndex) &&
               animator.GetCurrentAnimatorStateInfo(layerIndex).IsName
                    (animator.GetLayerName(layerIndex) + "." + stateName);
    }

    public static bool IsPlayingLoop(this Animator animator, int layerIndex, string stateName)
    {
        return animator.IsPlayingLoop(layerIndex) &&
               animator.GetCurrentAnimatorStateInfo(layerIndex).IsName
                    (animator.GetLayerName(layerIndex) + "." + stateName);
    }
}