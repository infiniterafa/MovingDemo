using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField]
    private float previewYOffset = 0.05f;

    [SerializeField]
    Material previewMaterialPrefab;
    Material previewMaterialInstance; 

    [SerializeField]
    private GameObject previewGameObject;
    private GameObject cellIndicator; 
}
