using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    private Text sunTotalNumText;
    private GameObject mainPanel;
    private SetPanel setPanel;
    private GameOverPanel gameOverPanel;
    private GameVictoryPanel gameVictoryPanel;

   // private 
    #region unity
    private void Awake()
    {
        Instance = this;
        sunTotalNumText = transform.Find("MainPanel/SunTotalNumText").GetComponent<Text>();
        mainPanel = transform.Find("MainPanel").gameObject;
        setPanel = transform.Find("SetPanel").GetComponent<SetPanel>();
        gameOverPanel = transform.Find("GameOverPanel").GetComponent<GameOverPanel>();
        gameVictoryPanel = transform.Find("GameVictoryPanel").GetComponent<GameVictoryPanel>();
        setPanel.Show(false);
    }
	#endregion

	#region methods

    /// <summary>
    /// 设置主面板的显示
    /// </summary>
    /// <param name="isOnShow"></param>是否显示主面板
    public void ShowMainPanel(bool isOnShow)
    {
        mainPanel.SetActive(isOnShow);
      //  Debug.Log(isOnShow);
    }

	/// <summary>
	/// 更新阳光数量
	/// </summary>
	public void UpdateSunNum(int num)
    {
        sunTotalNumText.text = num.ToString();
    }

    /// <summary>
    /// get the position of the number-of-sun
    /// </summary>
    public Vector3 GetSunNumTextPos()
    {
        return sunTotalNumText.transform.position;
    }

    public void ShowSetPanel()
    {
        setPanel.Show(true);
    }

    public void GameOver()
    {
        gameOverPanel.Over();
    }

    public void GameVictory()
    {
        gameVictoryPanel.Over(); // 游戏胜利
    }

	#endregion
}
