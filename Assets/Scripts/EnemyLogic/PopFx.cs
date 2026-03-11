using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PopFx : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        StartCoroutine(WaitForAnimation());
    }

    private IEnumerator WaitForAnimation()
    {
        yield return null;

        float animLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animLength);

        PopPool.Instance.ReturnToPool(this);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
