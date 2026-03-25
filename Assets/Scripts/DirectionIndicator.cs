using UnityEngine;

public class DirectionIndicator : MonoBehaviour
{

    [SerializeField] private Transform target; 

    Player player; 

    void Update()
    {
        var targetPosition = player.transform.forward;
        targetPosition.y = 0;
    }
}
