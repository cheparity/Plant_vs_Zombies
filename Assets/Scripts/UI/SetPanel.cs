using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetPanel : MonoBehaviour
{

    public void Show(bool isOnShow)
    {
        gameObject.SetActive(isOnShow);
        //????????????
        if (isOnShow)
        {
            Time.timeScale = 0; //pause
        }
        else
        {
            Time.timeScale = 1.0f;
        }
        
    }
    public void BackMainScene()
    {
        SceneManager.LoadScene("Start"); 
    }
    /// <summary>
    /// ??????
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
}
