using UnityEngine;
using System.Collections; 
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using System;
using UnityEngine.Events;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1; //indice, -1 si no hay ninguno seleccionado
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    ScoreManager scoreManager; 

    GridOrientation m_Orientation;
    ObjectDatabase database;
    GridData floorData;
    GridData furnitureData;
    ObjectPlacer objectPlacer;
    SoundFeedBack soundFeedBack;

    public event System.Action OnPlacementEvent;
    public PlacementState(int iD,
                          Grid grid,
                          PreviewSystem previewSystem,
                          ObjectDatabase database,
                          GridOrientation orientation,
                          GridData floorData,
                          GridData furnitureData,
                          ObjectPlacer objectPlacer,
                          SoundFeedBack soundFeedBack)
    {
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        m_Orientation = (GridOrientation)orientation;
        this.database = database;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;
        this.soundFeedBack = soundFeedBack;

        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID); // como un for loop pero busca el indice del objeto que se va a colocar en la base de datos, con su ID

        if (selectedObjectIndex > -1)
        {
            previewSystem.StartShowPlacePreview(database.objectsData[selectedObjectIndex].Prefab,
                                          database.objectsData[selectedObjectIndex].Size); //muestra el preview 
        }
        else
            throw new System.Exception($"No object found with ID {iD}");
        this.soundFeedBack = soundFeedBack;
    }

    public void EndState()
    {
       
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        Debug.Log("Placing object at: " + gridPosition);
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        if (placementValidity == false)
        {
            soundFeedBack.PlaySound(SoundType.wrongPlacement);
            return;

        }

        soundFeedBack.PlaySound(SoundType.Place);
        int index = objectPlacer.PlaceObject((database.objectsData[selectedObjectIndex].Prefab), grid.CellToWorld(gridPosition), m_Orientation);

        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ?
           floorData : furnitureData;

        selectedData.AddObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size,
                                               database.objectsData[selectedObjectIndex].ID,
                                               index);//agregar a la grid data el nuevo objeto con su posicion, tamaño, id e indice

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false); //actualizar  posicion preview
        OnPlacementEvent?.Invoke(); //evento de colocar objeto
    }


    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectObjectIndex)
    {
        GridData selectedData = database.objectsData[selectObjectIndex].ID == 0 ?
            floorData : furnitureData; // si el id es 0, se coloca en el piso y sino en los muebles

        return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectObjectIndex].Size); // chequeo de posiciones para colocar objetos para que no se pongan objetos encima de otros
    }

    public void UpdateState(Vector3Int gridPosition, GridOrientation O)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);


        //convertir posiciones a la grid junto conla preview
        m_Orientation = O;

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
        previewSystem.UpdateRoration(O);

    }
}
