using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏配置,不继承任何父类,不受外界影响,可以存放一些固定的游戏数据(如预制体)
/// </summary>
[CreateAssetMenu(fileName = "GameConf", menuName = "GameConf")]
public class GameConf : ScriptableObject
{
	[Header("植物")]
	[Tooltip("阳光")]
	public GameObject Sun;
	[Tooltip("太阳花")]
	public GameObject SunFlower;
	[Tooltip("豌豆射手")]
	public GameObject PeaShooter;
	[Tooltip("坚果")]
	public GameObject WallNut;
	[Tooltip("地刺")]
	public GameObject Spikeweed;
	[Tooltip("樱桃")]
	public GameObject Cherry;

	[Header("子弹")]
	[Tooltip("子弹击中效果")]
	public Sprite BulletHit;
	[Tooltip("豌豆子弹")]
	public GameObject Bullet1;

	[Header("僵尸")]
	[Tooltip("僵尸头")]
	public GameObject Zombie_head;
	[Tooltip("僵尸死尸")]
	public GameObject Zombie_DeadBody;
	[Tooltip("普通僵尸")]
	public GameObject Zombie;

}
