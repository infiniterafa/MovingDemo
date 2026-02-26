using System;
using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject mouseIndicador;

    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;


    [SerializeField]
    private ObjectDatabase database;
    private int selectObjectIndex = -1;

    [SerializeField]
    private GameObject gridVisualization;

    private GridData floorData, furnitureData;



    private List<GameObject> placedGameObjects = new();

    GridOrientation m_Orientation;

    [SerializeField]
    private PreviewSystem preview; 

    private Vector3Int lastPosition = Vector3Int.zero;

    private void Start()
    {
        StopPlacement();
        floorData = new GridData();
        furnitureData = new GridData();

     
    }

    public void StartPlacement(int ID)
    {

        StopPlacement(); // para evitar que se puedan colocar objetos al mismo tiempo


        selectObjectIndex = database.objectsData.FindIndex(data=> data.ID == ID); // como un for loop pero busca el indice del objeto que se va a colocar en la base de datos, con su ID
       
        if (selectObjectIndex < 0)
        {
            Debug.LogError($"No ID found {ID}"); // mensaje de no id 
            return; 
        }

        gridVisualization.SetActive(true); //activa visualizacion
        preview.StartShowPlacePreview(database.objectsData[selectObjectIndex].Prefab, 
                                      database.objectsData[selectObjectIndex].Size); //muestra el preview 

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

        bool placementValidity = CheckPlacementValidity(gridPosition, selectObjectIndex);

        if (placementValidity == false)
            return;

        GameObject newObject = Instantiate(database.objectsData[selectObjectIndex].Prefab); //instancia objeto 

        //convertir posiciones a la grid
        newObject.transform.position = grid.CellToWorld(gridPosition);
        newObject.transform.rotation = Quaternion.Euler(0, (int)m_Orientation * 90, 0); //rotar el objeto dependiendo de la orientacion

        placedGameObjects.Add(newObject); //agregar a la lista de objetos que se colocaron

        GridData selectedData = database.objectsData[selectObjectIndex].ID == 0 ?
           floorData : furnitureData;

        selectedData.AddObjectAt(gridPosition, database.objectsData[selectObjectIndex].Size,
                                               database.objectsData[selectObjectIndex].ID,
                                               placedGameObjects.Count - 1);//agregar a la grid data el nuevo objeto con su posicion, tamaño, id e indice

        preview.UpdatePosition(grid.CellToWorld(gridPosition), true); //actualizar  posicion preview
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectObjectIndex)
    {
        GridData selectedData = database.objectsData[selectObjectIndex].ID == 0 ?
            floorData : furnitureData; // si el id es 0, se coloca en el piso y sino en los muebles

        return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectObjectIndex].Size); // chequeo de posiciones para colocar objetos para que no se pongan objetos encima de otros
    }

    private void StopPlacement()
    {
        selectObjectIndex = -1;

        gridVisualization.SetActive(false); //activa visualizacion
        preview.StopShowingPreviewPlacement(); 

        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        lastPosition = Vector3Int.zero;
    }

    private void Update()
    {
        if(selectObjectIndex < 0)
       
            return;
       
        if (Input.GetKeyDown(KeyCode.R))
        {
            //rotar el objeto con la tecla R
            m_Orientation = (GridOrientation)(((int)m_Orientation + 1) % Enum.GetNames(typeof(GridOrientation)).Length);
        }

        //el mouse del juego como un objeto junto con su pos
        Vector3 mouseposition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mouseposition);

        if (lastPosition != gridPosition)
        {
            //el mismo chequeo para el objeto 
            bool placementValidity = CheckPlacementValidity(gridPosition, selectObjectIndex);

            mouseIndicador.transform.position = mouseposition;
            //convertir posiciones a la grid junto conla preview

            preview.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);

            lastPosition = gridPosition;
        }

       
    }
}
