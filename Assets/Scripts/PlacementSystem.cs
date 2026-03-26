using System;
using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlacementSystem : MonoBehaviour
{


    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;


    [SerializeField]
    private ObjectDatabase database;


    [SerializeField]
    private GameObject gridVisualization;

    private GridData floorData, furnitureData;

    [SerializeField]
    private ObjectPlacer objectPlacer;

    [SerializeField]
    private SoundFeedBack soundFeedBack;



    GridOrientation m_Orientation;

    [SerializeField]
    private PreviewSystem preview;

    private Vector3Int lastPosition = Vector3Int.zero;

    IBuildingState buildingState;

    private void Start()
    {
        StopPlacement();
        floorData = new GridData();
        furnitureData = new GridData();


    }

    public void StartPlacement(int ID)
    {

        StopPlacement(); // para evitar que se puedan colocar objetos al mismo tiempo
        gridVisualization.SetActive(true); //activa visualizacion

        buildingState = new PlacementState(ID,
                                           grid,
                                           preview,
                                           database,
                                           m_Orientation,
                                           floorData,
                                           furnitureData,
                                           objectPlacer, 
                                           soundFeedBack); //crea un nuevo estado de colocacion con el ID del objeto
        //(buildingState as PlacementState).OnPlacementEvent += ScoreManager.FindAnyObjectByType<ScoreManager>().AddPoint; //evento de colocar objeto para sumar puntos
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;

    }

    public void StartRemoving()
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState = new RemovingState(grid, preview, m_Orientation, floorData, furnitureData, objectPlacer); //crea un nuevo estado de eliminacion
        //(buildingState as RemovingState).OnRemovalEvent += ScoreManager.FindAnyObjectByType<ScoreManager>().SubstractPoint; //evento de eliminar objeto para restar puntos
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            //  evitar poner objetos debajo de la UI
            return;
        }

        //el mouse del juego como un objeto junto con su pos que hará que se instancie el nuevo objeto
        Vector3 mouseposition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mouseposition);


        buildingState.OnAction(gridPosition); // colocar el objeto en la posicion de la grid
    }

    //private bool CheckPlacementValidity(Vector3Int gridPosition, int selectObjectIndex)
    //{
    //    GridData selectedData = database.objectsData[selectObjectIndex].ID == 0 ?
    //        floorData : furnitureData; // si el id es 0, se coloca en el piso y sino en los muebles

    //    return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectObjectIndex].Size); // chequeo de posiciones para colocar objetos para que no se pongan objetos encima de otros
    //}

    private void StopPlacement()
    {
        if (buildingState == null)
            return;

        gridVisualization.SetActive(false); //activa visualizacion
        buildingState.EndState();

        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        lastPosition = Vector3Int.zero;
        buildingState = null;
    }

    private void Update()
    {
        if (buildingState == null)

            return;

        //el mouse del juego como un objeto junto con su pos
        Vector3 mouseposition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mouseposition);

        //if (lastPosition != gridPosition)
        //{
            buildingState.UpdateState(gridPosition, m_Orientation); // actualizar el estado del objeto 

            lastPosition = gridPosition;
        //}

        if (Input.GetKeyDown(KeyCode.R))
        {
            //rotar el objeto con la tecla R
            m_Orientation = (GridOrientation)(((int)m_Orientation + 1) % Enum.GetNames(typeof(GridOrientation)).Length);
        }


    }
}
