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
    public bool menuOpen = false;

    private InputAction menuAction;
    private InputAction clickAction;
    private InputAction cancelAction;

    public event Action OnClick, OnCancel, OnMenu;

    private Vector3 lastPos;
    private Vector2 lastPointPos;

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
        GetPointerScreenPosition();
        HandleTouchClick();
        if (menuAction.WasPressedThisFrame())
            OnMenu?.Invoke();
        if (cancelAction.WasPressedThisFrame())
            OnCancel?.Invoke();
    }

    private void HandleTouchClick()
    {
        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;

            if (touch.press.wasReleasedThisFrame)
                OnClick?.Invoke();
        }
        else
        {
            if (clickAction.WasPressedThisFrame())
                OnClick?.Invoke();
        }
    }

    public bool IsPointerOverUI()
    {
        if (EventSystem.current == null)
            return false;

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            return EventSystem.current.IsPointerOverGameObject(Touchscreen.current.primaryTouch.touchId.ReadValue());
        }

        return EventSystem.current.IsPointerOverGameObject();

    }

    public Vector2 GetPointerScreenPosition()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.position.ReadValue() != Vector2.zero)
        {
            lastPointPos = Touchscreen.current.primaryTouch.position.ReadValue();
        }
        else if (Mouse.current != null)
        {
            lastPointPos = Mouse.current.position.ReadValue();
        }
        return lastPointPos;
    }

    public Vector3 GetMapPos()
    {
        Vector2 screenPos = Vector2.zero;

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            screenPos = Touchscreen.current.primaryTouch.position.ReadValue();
        }
        else if (Mouse.current != null)
        {
            screenPos = Mouse.current.position.ReadValue();
        }

        Ray ray = sceneCamera.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, 100, placementLayer))
        {
            lastPos = hit.point;
        }

        return lastPos;
    }
}
