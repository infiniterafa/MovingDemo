using UnityEngine;

public class FurnitureBehaviour : MonoBehaviour
{
    //[SerializeField] private float vibrationAmount = 1f;
    bool isPlaceHolder = false;

    public void IsPlaceHolder(bool isIt)
    {
        isPlaceHolder = isIt;
        FindAnyObjectByType<ScoreManager>().SubstractPoint();
    }

    private void Awake()
    {
        if (isPlaceHolder)return;
        FindAnyObjectByType<ScoreManager>().AddPoint();
    }


    private void OnDestroy()
    {
        if (isPlaceHolder)return;
        FindAnyObjectByType<ScoreManager>().SubstractPoint();
    }
}
