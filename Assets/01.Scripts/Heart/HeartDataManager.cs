using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartDataManager : MonoBehaviour
{
    public static HeartDataManager instance;

    public List<Color> heartColorList = new List<Color>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveColor(Color color, int heartIndex)
    {
        heartColorList[heartIndex] = color;
    }
}
