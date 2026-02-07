using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject mouseIndicador, cellIndicator;

    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid; 

    private void Update()
    {
        //el mouse del juego como un objeto junto con su pos
        Vector3 mouseposition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mouseposition);
        mouseIndicador.transform.position = mouseposition;
        //convertir posiciones a la grid
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }
}
