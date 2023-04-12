using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    private Image img;
    private Image panel;
    void Awake()
    {
        img = transform.Find("Image").GetComponent<Image>();
        panel = transform.Find("Panel").GetComponent<Image>();
        img.gameObject.SetActive(false);
        panel.gameObject.SetActive(false);
        img.color = new Color(0, 0, 0, 0);
    }
    /// <summary>
    /// ????
    /// </summary>
    public void Over()
    {
        //???
        img.gameObject.SetActive(true);
        panel.gameObject.SetActive(true);
        //Panel??
        StartCoroutine(DoColorGraduallyChange());
    }

   IEnumerator DoColorGraduallyChange()
    {
        float alpha = 0;
        while (alpha < 1)
        {
            alpha += 0.02f;
            img.color=new Color(0, 0, 0,alpha);
            yield return new WaitForSeconds(0.05f);
        }

        //??2s???????
        yield return new WaitForSeconds(2f);
        BackMainScene();
    }

	private void BackMainScene()
	{
		SceneManager.LoadScene("Start");
	}
}
