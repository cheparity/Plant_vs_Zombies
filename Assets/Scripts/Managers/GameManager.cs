using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**************************************
 * GameManager用来管理游戏配置
 * ***********************************/
public class GameManager : MonoBehaviour {
	public static GameManager Instance;//单例 
	public GameConf GameConf { get; private set; }

	private void Awake() {
		GameConf = Resources.Load<GameConf>("GameConf"); //加载游戏配置
		Instance = this;
	}


}
