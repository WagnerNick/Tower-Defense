using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TowerRangeDisplay : MonoBehaviour
{
    [Header("Display")]
    [SerializeField] private int segments = 64;
    [SerializeField] private Vector3 offset;

    private LineRenderer lineRenderer;
    private Tower tower;
    private float lastRange = -1f;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        tower = GetComponentInParent<Tower>();

        lineRenderer.loop = true;
        lineRenderer.useWorldSpace = false;
        lineRenderer.positionCount = segments;
    }

    private void OnDisable()
    {
        SetVisible(false);
    }

    private void Update()
    {
        if (tower != null && !Mathf.Approximately(tower.Range, lastRange))
            DrawCircle();
    }

    public void SetVisible(bool visible)
    {
        if (lineRenderer != null)
            lineRenderer.enabled = visible;
    }

    private void DrawCircle()
    {
        float range = tower != null ? tower.Range : lastRange;
        if (range <= 0f) return;

        lastRange = range;

        for (int i = 0; i < segments; i++)
        {
            float angle = 2f * Mathf.PI * i / segments;
            float x = Mathf.Cos(angle) * range;
            float z = Mathf.Sin(angle) * range;
            lineRenderer.SetPosition(i, new Vector3(offset.x + x, offset.y, offset.z + z));
        }
    }
}
