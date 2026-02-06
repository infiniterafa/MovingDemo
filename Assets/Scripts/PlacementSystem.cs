using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject mouseIndicador;

    [SerializeField]
    private InputManager inputManager;

    private void Update()
    {
        //el mouse del juego como un objeto junto con su pos
        Vector3 mouseposition = inputManager.GetSelectedMapPosition();
        mouseIndicador.transform.position = mouseposition;
    }
}
