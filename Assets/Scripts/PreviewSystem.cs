using System;
using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField]
    private float previewYOffset = 0.05f;

    [SerializeField]
    private Material previewMaterialPrefab;
    private Material previewMaterialInstance;

    [SerializeField]
    private GameObject previewObject;
    [SerializeField]
    private GameObject cellIndicator;

    private Renderer cellIndicatorRenderer;

    private void Start()
    {
        previewMaterialInstance = new Material(previewMaterialPrefab);
        cellIndicator.SetActive(false);
        //cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartShowPlacePreview(GameObject prefab, Vector2Int size)
    {
        previewObject = Instantiate(prefab);
        PreparePreview(previewObject);
        PrepareCursor(size);
        cellIndicator.SetActive(true);
        cellIndicatorRenderer = previewObject.GetComponentInChildren<Renderer>();
    }

    private void PrepareCursor(Vector2Int size) //toma el nuevo tamaño para la celda
    {
        if (size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
            //cellIndicatorRenderer.sharedMaterial.mainTextureScale = size; 
        }
    }

    private void PreparePreview(GameObject previewGameObject) // cambia material
    {
        Renderer[] renderers = previewGameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialInstance;
            }
            renderer.materials = materials;
        }
    }

    public void StopShowingPreview()  //quita el objeto que se esta haciendo el preview
    {
        cellIndicator.SetActive(false);
        if (previewObject != null)
            Destroy(previewObject);
    }

    public void UpdatePosition(Vector3 position, bool validity)
    {
        if (previewObject != null)
        {
            MovePreview(position);
            ApplyFeedBackToPreview(validity);
        }

        MoveCursor(position);

        ApplyFeedBackToCursor(validity);
    }
    // cambiar el color, posicion y mouse de la preview
    private void ApplyFeedBackToPreview(bool validity)
    {
        Color c = validity ? Color.white : Color.red;

        c.a = 0.6f;
        previewMaterialInstance.color = c;
    }

    // cambiar el color, posicion y mouse de la preview
    private void ApplyFeedBackToCursor(bool validity)
    {
        if (cellIndicatorRenderer == null) return;
        Color c = validity ? Color.white : Color.red;
        cellIndicatorRenderer.sharedMaterial.color = c;
        c.a = 0.6f;

    }

    private void MoveCursor(Vector3 position)
    {
        cellIndicator.transform.position = position;
    }

    private void MovePreview(Vector3 position)
    {
        previewObject.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z);
    }

    internal void StartShowingRemovePreview()
    {
        cellIndicator.SetActive(true);
        PrepareCursor(Vector2Int.one);
        ApplyFeedBackToCursor(false);
    }

    internal void UpdateRoration(GridOrientation o)
    {
        previewObject.transform.rotation = Quaternion.Euler(0, (int)o * 90, 0);
    }
}
