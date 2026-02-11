using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class InputManager : MonoBehaviour
{
    [SerializeField]

    private Camera sceneCamera;

    private Vector3 lastPos;

    [SerializeField]

    private LayerMask placelayerMask;



    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition; // registrar el input en el espacio 3D con el mouse

        mousePos.z = sceneCamera.nearClipPlane; // ayuda a que no se puedan seleccionar objetos fuera del eje z 

        // ayuda del raytrace a identificar el input del mouse

        Ray ray = sceneCamera.ScreenPointToRay(mousePos);

        RaycastHit hit; 

        if (Physics.Raycast(ray, out hit, 100, placelayerMask))
        {
            lastPos = hit.point;
        }

        // regresara valores, incluso si no esta en nuestro plano

        return lastPos; 

    }


}


