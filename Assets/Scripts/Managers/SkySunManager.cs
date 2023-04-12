using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkySunManager : MonoBehaviour
{
    public static SkySunManager Instance;
    //创建阳光的坐标
    private const float creatSunPositionY = 6;//生成时y恒为6
    //下落的最终位置的最大和最小值
    private float downSunPositionY_Min = -3.91f;
    private float downSunPositionY_Max = 2.5f;
    //x的位置从创建就不变
    private float creatMaxPositionX_Max = 5.7f;
    private float creatMinPositionX_Min = -5.7f;

    private void Awake()
    {
        Instance = this;
    }


    /// <summary>
    /// 从天空生成阳光
    /// </summary>
    void CreatSun()
    {
        //生成阳光
        Sun sun = Instantiate<GameObject>(GameManager.Instance.GameConf.Sun, Vector3.zero, Quaternion.identity, transform).GetComponent<Sun>();
        //阳光的起点y坐标，终点y坐标
        float creatY = creatSunPositionY;
        float downY = Random.Range(downSunPositionY_Min, downSunPositionY_Max);
        //阳光的起点和终点x坐标保持一致
        float creatX = Random.Range(creatMinPositionX_Min, creatMaxPositionX_Max);
        sun.InitForSky(creatX, creatY, downY);//这个组件在sun身上
    }


	public void StartCreatingSun(float delay)
	{
        InvokeRepeating("CreatSun", delay, 3); ;//调用CreatSun函数
	}
    public void StopCreatingSun()
    {
        CancelInvoke();
    }

}
