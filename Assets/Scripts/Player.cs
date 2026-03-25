using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed = 5.0f;
    private float horizontalInput;
    private float forwardInput;

    GameObject grabbedObject;

    Transform CurrentSelectedGameObject;
    private IEnumerable<GameObject> hitObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //player input 
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        //forward
        if (horizontalInput + forwardInput != 0)
        {
            transform.Translate(-transform.right * Time.deltaTime * speed * horizontalInput);
            transform.Translate(transform.forward * Time.deltaTime * speed * forwardInput);
            transform.LookAt(transform.position + (new Vector3(horizontalInput, 0, forwardInput).normalized * Time.deltaTime), Vector3.up); //gira el player hacia la direccion que se mueve
        }

        if (!Input.GetKeyDown(KeyCode.E)) return;
        if (grabbedObject != null)
        {
            grabbedObject?.transform.SetParent(null); // si el objeto agarrado no es null, lo suelta
            grabbedObject = null;
        }
        else
        {
            var ray = new Ray(transform.position + transform.forward + transform.up, -transform.up);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (!hit.collider.CompareTag("Furniture")) return;
                Debug.Log("Hit furniture: " + hit.collider.gameObject.name);
                grabbedObject = hit.collider.gameObject;
                hit.collider.transform.SetParent(transform); // hace que el objeto golpeado sea hijo del player, para que se mueva con el player
            }
        }
    }
}
