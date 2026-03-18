using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TowerSelector : MonoBehaviour
{
    public static TowerSelector instance;
    public Tower selectedTower;
    [SerializeField] private LayerMask towerLayer;
    [SerializeField] private Camera sceneCamera;

    void Awake() => instance = this;

    void OnEnable()
    {
        InputManager.Instance.OnClick += HandleClick;
    }

    void OnDisable()
    {
        InputManager.Instance.OnClick -= HandleClick;
    }

    void HandleClick()
    {
        //if (PlacementSystem.Instance.IsPlacing) return;
        if (InputManager.Instance.IsPointerOverUI()) return;

        Ray ray = sceneCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, towerLayer))
        {
            Tower tower = hit.collider.gameObject.GetComponentInParent<Tower>();
            if (tower != null)
            {
                if (tower == selectedTower)
                    Deselect();
                else
                    SelectTower(tower);
                return;
            }
        }
        Deselect();
    }

    public void SelectTower(Tower tower)
    {
        //if (selectedTower != null)
        //    selectedTower.SetSelected(false);
        selectedTower = tower;
        //tower.SetSelected(true);
        TowerUI.instance.Show(tower);
    }

    public void Deselect()
    {
        if (selectedTower != null)
        {
            //selectedTower.SetSelected(false);
            selectedTower = null;
        }
        TowerUI.instance.Hide();
    }
}
