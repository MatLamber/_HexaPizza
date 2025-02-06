using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientAnimationManager : MonoBehaviour
{
    private static readonly int walk = Animator.StringToHash("Walk");
    private static readonly int idle = Animator.StringToHash("Idle");

    private Animator animator => GetComponentInChildren<Animator>();


    public void WalkAndStop()
    {
        StartCoroutine(WalkAndStopCoroutine());
    }

    public void Walk()
    {
        animator.SetTrigger(walk);
    }

    public void EnableUpperBodyLayer()
    {
        animator.SetLayerWeight(1, 1);
    }

    IEnumerator WalkAndStopCoroutine()
    {
        animator.SetTrigger(walk);
        yield return new WaitForSeconds(0.3f);
        animator.SetTrigger(idle);
    }
}
