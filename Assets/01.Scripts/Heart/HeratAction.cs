using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartAction : MonoBehaviour
{
    [field: SerializeField] private Camera mainCamera;

    [field: SerializeField] List<GameObject> heartPice = new List<GameObject>();

    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; 
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SelectHeartPice();
        }
    }

    private void Start()
    {
        StartCoroutine(HeartActionCoroutine());
    }

    private void SelectHeartPice()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("HeartPice"))
            {
                PaintColor(hit.collider.gameObject, Color.red);
            }
        }
    }

    private void PaintColor(GameObject pice, Color color)
    {
        pice.GetComponent<SpriteRenderer>().color = color;
    }

    IEnumerator HeartActionCoroutine()
    {
        yield return null;
        while (mainCamera.orthographic)
        {
            mainCamera.orthographicSize = Mathf.Lerp(0.5f, 5f, 2);
            yield return null;
        }
    }
}
