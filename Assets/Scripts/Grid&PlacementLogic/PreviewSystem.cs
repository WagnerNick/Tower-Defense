using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField] private float previewYOffset = 0.06f;

    [SerializeField] private GameObject cellIndicator;
    private GameObject previewObject;

    [SerializeField] private Material previewMaterialsPrefab;
    private Material previewMaterialsInstance;

    private Renderer cellIndicatorRender;

    private void Start()
    {
        previewMaterialsInstance = new Material(previewMaterialsPrefab);
        cellIndicator.SetActive(false);
        cellIndicatorRender = cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void ShowPreview(GameObject prefab, Vector2Int size)
    {
        previewObject = Instantiate(prefab);
        PreparePreview(previewObject);
        PrepareCursor(size);
        cellIndicator.SetActive(true);
    }

    private void PrepareCursor(Vector2Int size)
    {
        if (size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
            cellIndicatorRender.material.mainTextureScale = size;
        }
    }

    private void PreparePreview(GameObject previewObject)
    {
        MonoBehaviour[] behaviours = previewObject.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour behaviour in behaviours)
        {
            if (behaviour is TowerRangeDisplay)
                continue;
            behaviour.enabled = false;
        }

        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            if (renderer is LineRenderer)
                continue;
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialsInstance;
            }
            renderer.materials = materials;
        }
        TowerRangeDisplay rangeDisplay = previewObject.GetComponent<TowerRangeDisplay>();
        if (rangeDisplay != null)
        {
            rangeDisplay.SetVisible(true);
        }
    }

    public void StopPreview()
    {
        cellIndicator.SetActive(false);
        if (previewObject != null)
            Destroy(previewObject);
    }

    public void UpdatePos(Vector3 pos, bool validity)
    {
        if (previewObject != null)
        {
            MovePreview(pos);
            ApplyFeedbackToPreview(validity);
        }

        MoveCursor(pos);
        ApplyFeedbackToCursor(validity);
    }

    private void ApplyFeedbackToPreview(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        c.a = 0.5f;
        previewMaterialsInstance.color = c;
    }

    private void ApplyFeedbackToCursor(bool validity)
    {
        Color c = validity ? Color.green : Color.red;
        c.a = 0.5f;
        cellIndicatorRender.material.color = c;
    }

    private void MoveCursor(Vector3 pos)
    {
        cellIndicator.transform.position = pos;
    }

    private void MovePreview(Vector3 pos)
    {
        previewObject.transform.position = new Vector3(pos.x, pos.y + previewYOffset, pos.z);
    }

    internal void ShowRemovalPreview()
    {
        cellIndicator.SetActive(true);
        PrepareCursor(Vector2Int.one);
        ApplyFeedbackToCursor(false);
    }
}
