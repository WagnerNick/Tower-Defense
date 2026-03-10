using System;
using UnityEngine;

public class Dart : MonoBehaviour
{
    private Transform target;
    public float speed = 70f;

    public void Seek(Transform _target)
    {
        target = _target;
    }

    private void Update()
    {
        if (target == null)
        {
            DartPool.Instance.ReturnToPool(this);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distThisFrame, Space.World);
        transform.LookAt(target);
    }

    private void HitTarget()
    {
        DartPool.Instance.ReturnToPool(this);
        PopFx popFx = PopPool.Instance.Get();
        EnemyPool.Instance.ReturnToPool(target.GetComponentInParent<Enemy>());
        popFx.transform.SetPositionAndRotation(transform.position, popFx.transform.rotation);
        popFx.gameObject.SetActive(true);
    }
}
