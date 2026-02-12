using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField]

    private Camera sceneCamera;

    private Vector3 lastPos;

    [SerializeField]

    private LayerMask placelayerMask;

    public event Action OnClicked, OnExit;

    private void Update()
    {
        //invocando action del boton derecho del mouse  
        if(Input.GetMouseButtonDown(0))
            OnClicked?.Invoke();
        if(Input.GetKeyDown(KeyCode.Escape))
            OnExit?.Invoke();
    }

    public bool IsPointerOverUI()
         => EventSystem.current.IsPointerOverGameObject(); // ayuda a que no se puedan seleccionar objetos debajo de la UI


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


