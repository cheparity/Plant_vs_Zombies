using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : MonoBehaviour
{
	private List<Zombie> zombies = new List<Zombie>();
	private int currentOrderNum = 0;
	private float minX = 10f;
	private float maxX = 12f;

	public static ZombieManager Instance;

	public int CurrentOrderNum
	{
		get => currentOrderNum;            
		set
		{
			currentOrderNum = value;
			if (currentOrderNum > 50) currentOrderNum = 0;
		}
	}

	
	#region unity Rigion
	private void Awake()
	{
		Instance = this;
	}                                  

	/// <summary>
	/// 创建僵尸
	/// </summary>
	public void UpdateZombie(int zombieCount)
	{
		for (int i = 0; i < zombieCount; i++) //创建zombieCount个僵尸
			CreateZombie();				
	}

	#endregion



	/// <summary>
	/// 创建僵尸
	/// </summary>
	/// <param name="lineNum"></param>出现的行数
	private void CreateZombie()
	{
		Zombie zombie = Instantiate<GameObject>(
			GameManager.Instance.GameConf.Zombie,
			new Vector3(GetXaxisRandomly(), 0, 0),
			Quaternion.identity, transform).GetComponent<Zombie>();  // 三个参数你可以这么 展开

		AddZombie(zombie); //加入数组
		zombie.Init(GetYNumRandomly(), CurrentOrderNum++);//僵尸初始化		  

	}

	/// <summary>
	/// 清除所有僵尸
	/// </summary>
	public void ClearZombie()
	{
		while (zombies.Count > 0)
		{
			zombies[0].Dead();
		}
	}

	/// <summary>
	/// 将所有僵尸设为失活（默认）状态
	/// </summary>
	public void InactivateZombies()
	{
		while (zombies.Count > 0)
		{
			zombies[0].State=ZombieState.Idel;
		}
	}

	/// <summary>
	/// 将所有僵尸状态设为走路状态
	/// </summary>
	public void ActivateZombies()
	{
		while (zombies.Count > 0)
		{
			zombies[0].State = ZombieState.Walk;
		}
	}

	/// <summary>
	/// 获取随机的x轴坐标,注意返回值是小数
	/// </summary>
	/// <returns></returns>
	private float GetXaxisRandomly()
	{
		return Random.Range(minX, maxX);
	}
	/// <summary>
	/// 获取随机的y轴坐标,注意返回值是整数
	/// </summary>
	/// <returns></returns>
	private int GetYNumRandomly()
	{
		return Random.Range(0, 5);
	}

	/// <summary>
	/// 获取距离目标坐标pos最小的同行僵尸
	/// </summary>
	/// <param name="line"></param> 行数
	/// <param name="pos"></param> 目标点坐标
	/// <returns></returns>
	public Zombie GetZombieByLineMinDistance(int line, Vector3 pos)
	{
		Zombie zombie = null;
		float minDistance = 10000f;
		for (int i = 0; i < zombies.Count; i++)
		{
			if (zombies[i].CurrentGrid.Point.y == line && Vector2.Distance(pos, zombies[i].transform.position) < minDistance)
			{
				minDistance = Vector2.Distance(pos, zombies[i].transform.position);
				zombie = zombies[i];
			}
		}
		if (zombie == null) return null;
		else return zombie;

	}
	public void AddZombie(Zombie zombie)
	{
		zombies.Add(zombie);
	}
	public void RemoveZombie(Zombie zombie)
	{
		zombies.Remove(zombie);
	}
	public int ZombieCount()
	{
		return zombies.Count;
	}
}
