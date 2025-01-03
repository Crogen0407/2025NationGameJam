using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class HeartList
{
    public GameObject heartPice;
    public Color piceColor;
}

public class HeartManager : MonoBehaviour
{
    [field: SerializeField] private Camera mainCamera;

    [field: SerializeField] List<HeartList> heartPice = new List<HeartList>();

    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; 
        }
    }

    private void Start()
    {
        if(HeartDataManager.instance != null)
        {
            for(int i = 0; i < heartPice.Count; i++)
            {
                PaintColor(heartPice[i].heartPice, HeartDataManager.instance.heartColorList[i]);
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SelectHeartPice();
        }
    }

    private void SelectHeartPice()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("HeartPice"))
            {
                foreach (var heart in heartPice)
                {
                    if(heart.heartPice == hit.collider.gameObject)
                    {
                         PaintColor(hit.collider.gameObject, Color.red);
                         heart.piceColor = Color.red;
                    }
                }
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
