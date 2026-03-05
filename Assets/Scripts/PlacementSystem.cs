using System;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private ObjectDatabaseSO database;
    [SerializeField] private GameObject gridVisual;
    [SerializeField] private PreviewSystem preview;
    [SerializeField] private ObjectPlacer objectPlacer;

    private Vector3Int lastDetectedPos = Vector3Int.zero;
    private GridData powerData, towerData;

    IBuildingState buildingState;

    private void Start()
    {
        StopPlacement();
        gridVisual.SetActive(false);
        powerData = new();
        towerData = new();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        gridVisual.SetActive(true);
        buildingState = new PlacementState(ID, grid, preview, database, powerData, towerData, objectPlacer);
        InputManager.Instance.OnClick += PlaceObject;
        InputManager.Instance.OnCancel += StopPlacement;
    }

    public void StartRemoval()
    {
        StopPlacement();
        gridVisual.SetActive(true);
        buildingState = new RemovalState(grid, preview, powerData, towerData, objectPlacer);
        InputManager.Instance.OnClick += PlaceObject;
        InputManager.Instance.OnCancel += StopPlacement;
    }

    private void PlaceObject()
    {
        if (InputManager.Instance.IsPointerOverUI())
            return;
        Vector3 mousePos = InputManager.Instance.GetMapPos();
        Vector3Int gridPos = grid.WorldToCell(mousePos);

        buildingState.OnAction(gridPos);
    }

    private void StopPlacement()
    {
        if (buildingState == null)
            return;
        gridVisual.SetActive(false);
        buildingState.EndState();
        InputManager.Instance.OnClick -= PlaceObject;
        InputManager.Instance.OnCancel -= StopPlacement;
        lastDetectedPos = Vector3Int.zero;
        buildingState = null;
    }

    private void Update()
    {
        if (buildingState == null)
            return;
        Vector3 mousePos = InputManager.Instance.GetMapPos();
        Vector3Int gridPos = grid.WorldToCell(mousePos);

        if (lastDetectedPos != gridPos)
        {
            buildingState.UpdateState(gridPos);
            lastDetectedPos = gridPos;
        }
    }
}
