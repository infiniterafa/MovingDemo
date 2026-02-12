using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed = 5.0f;
    private float horizontalInput;
    private float forwardInput;
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
        transform.Translate(Vector3.right * Time.deltaTime * speed * horizontalInput);
        transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput);
        
    }
}
