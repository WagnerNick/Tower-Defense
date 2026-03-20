using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PopFx : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        StartCoroutine(WaitForAnimation());
        SFXManager.Play("Pop");
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
