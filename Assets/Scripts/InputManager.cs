using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [SerializeField] private LayerMask placementLayer;
    [SerializeField] private Camera sceneCamera;
    public static PlayerInput input;

    private InputAction menuAction;
    private InputAction clickAction;
    private InputAction cancelAction;

    public event Action OnClick, OnCancel, OnMenu;

    private Vector3 lastPos;

    void Awake()
    {
        Instance = this;
        input = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        clickAction = input.actions["Click"];
        cancelAction = input.actions["Cancel"];
        menuAction = input.actions["Menu"];
    }

    private void Update()
    {
        if (menuAction.WasPressedThisFrame())
            OnMenu?.Invoke();
        if (clickAction.WasPressedThisFrame())
            OnClick?.Invoke();
        if (cancelAction.WasPressedThisFrame())
            OnCancel?.Invoke();
    }

    public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();

    public Vector3 GetMapPos()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, placementLayer))
        {
            lastPos = hit.point;
        }
        return lastPos;
    }
}
