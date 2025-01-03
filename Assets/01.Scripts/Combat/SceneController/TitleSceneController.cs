using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneController : MonoBehaviour
{
    private void Start()
    {
        SoundManager.Instance.PlayBGM("TitleScene", 0.5f);
    } 
 
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
