using UnityEngine;
using UnityEngine.InputSystem;

public class TowerSelector : MonoBehaviour
{
    public static TowerSelector Instance;
    public Tower selectedTower { get; private set; }
    public TowerRangeDisplay rangeDisplay { get; private set; }
    [SerializeField] private LayerMask towerLayer;

    void Awake() => Instance = this;

    void Start()
    {
        InputManager.Instance.OnClick += HandleClick;
    }

    void OnDestroy()
    {
        InputManager.Instance.OnClick -= HandleClick;
    }

    void HandleClick()
    {
        if (InputManager.Instance.IsPointerOverUI()) return;
        if (PlacementSystem.Instance.isActive) return;
        if (rangeDisplay != null)
        {
            rangeDisplay.SetVisible(false);
            rangeDisplay = null;
        }

        Vector2 screenPos = InputManager.Instance.GetPointerScreenPosition();
        Ray ray = Camera.main.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, towerLayer))
        {
            Tower tower = hit.collider.gameObject.GetComponentInParent<Tower>();
            rangeDisplay = hit.collider.gameObject.GetComponentInParent<TowerRangeDisplay>();
            if (tower != null)
            {
                if (tower == selectedTower)
                    Deselect();
                else
                    Select(tower);
                return;
            }
        }
        Deselect();
    }

    public void Select(Tower tower)
    {
        selectedTower = tower;
        rangeDisplay.SetVisible(true);
        TowerUI.Instance.Show(tower);
    }

    public void Deselect()
    {
        if (selectedTower == null) return;
        selectedTower = null;
        TowerUI.Instance.Hide();
    }
}
