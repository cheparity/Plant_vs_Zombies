using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
	private int sunNum; //阳光数量
	public static PlayerManager Instance; //单例
	private UnityAction SunNumUpdateAction; //阳光数量更新时的事件

	public int SunNum
	{
		get => sunNum;
		set
		{
			sunNum = value; //设置太阳数量
			UIManager.Instance.UpdateSunNum(sunNum); //更新ui组件
			SunNumUpdateAction();
		}
	}

	private void Awake()
	{
		Instance = this;
		SunNumUpdateAction = new UnityAction(NewAct); //纯纯为了初始化
	}

	/// <summary>
	/// 这个函数啥也不干
	/// </summary>
	private void NewAct(){}
	private void Start()
	{
		SunNum = 50;
	}

	/// <summary>
	/// 添加阳光数量更新时的事件监听
	/// </summary>
	public void AddSunNumUpdateActionListener(UnityAction action)
	{
		SunNumUpdateAction += action;
	}

}
