using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 关卡状态
/// </summary>
public enum LevelState
{
	Start, //开始
	Fighting, //战斗中
	Victory,
	Over //结束
}

public class LevelManager : MonoBehaviour
{
	#region variable
	public static LevelManager Instance;
	private LevelState currentLevelState;
	private float timer;

	public LevelState CurrentLevelState
	{
		get => currentLevelState;
		set
		{
			currentLevelState = value;
			switch (currentLevelState)
			{
				case LevelState.Start:
					UIManager.Instance.ShowMainPanel(false);//隐藏UI组件
					ZombieManager.Instance.UpdateZombie(10); //创建10个僵尸
					CameraManager.Instance.MoveCameraForStart();//摄像机移动到右侧,延时移动回左侧
					SkySunManager.Instance.StartCreatingSun(6);//让阳关延时开始创建
					Invoke("TransformLevelStateToFighting", 6f); //改变关卡状态
					Invoke("ClearZombie", 4f); //清理僵尸
					break;
				case LevelState.Fighting:
					UIManager.Instance.ShowMainPanel(true); //显示UI
					InvokeRepeating("UpdateZombie", 10, 10);  //重新刷新僵尸
					break;
				case LevelState.Over:
					CancelInvoke("UpdateZombie");
					break;
				case LevelState.Victory:
					
					break;
			}
		}
	}

	#endregion

	#region unity
	private void Awake()
	{
		Instance = this;
		
	}
	private void Start()
	{
		CurrentLevelState=LevelState.Start; //状态设置为开始游戏
	}
	private void Update()
	{
		timer += Time.deltaTime;
		if (timer > 120 && CurrentLevelState!=LevelState.Victory ) //时间大于120s且游戏状态不为胜利
		{
			CancelInvoke("UpdateZombie");
			GameVictory(); //游戏胜利
		}
	}

	//public void FSM()
	//{
	//	switch (currentLevelState)
	//	{
	//		case LevelState.Start:
	//			break;
	//		case LevelState.Fighting:
	//			break;
	//		case LevelState.Over:
	//			break;
	//	}
	//}

	#endregion

	/// <summary>
	/// 延时刷新僵尸
	/// </summary>
	/// <param name="delay"></param>
	/// <param name="zombieNum"></param>
	private void UpdateZombie()
	{
		//StartCoroutine(DoUpdateZombie(delay,zombieCount));
		ZombieManager.Instance.UpdateZombie((int)(timer/10)); //姑且刷一个僵尸
	}
	

	//IEnumerator DoUpdateZombie(float delay, int zombieCount)
	//{
	//	 yield return new WaitForSeconds(delay);
	//	ZombieManager.Instance.UpdateZombie(zombieCount); //刷新zombieCount个僵尸
	//}

	public void GameOver()
	{
		UIManager.Instance.GameOver();//UI的gameover
		SkySunManager.Instance.StopCreatingSun(); //不要掉阳光了
		ZombieManager.Instance.ClearZombie();//清理所有僵尸
		CurrentLevelState = LevelState.Over; //修改关卡状态为游戏结束
		//如果后面有时间再做一下音乐模块!!!!!!!!!
	}

	public void GameVictory()
	{
		if (ZombieManager.Instance.ZombieCount() == 0) //如果zombie没有了
		{
			UIManager.Instance.GameVictory(); //UI为胜利状态
			SkySunManager.Instance.StopCreatingSun(); //不要掉阳光了
			CurrentLevelState = LevelState.Victory; //停止刷僵尸
		}

	}

	#region 没啥用,单纯为了用Invoke
	void TransformLevelStateToFighting()
	{
		CurrentLevelState = LevelState.Fighting;//切换到战斗状态
	}

	void ClearZombie()
	{
		ZombieManager.Instance.ClearZombie();
	}
	#endregion
}
