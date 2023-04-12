using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaShooter : PlantBase
{

	private float attackCD = 1.4f; //攻击cd
	private Vector3 creatBulletOffsetPosition = new Vector3(0.562f, 0.386f);

	public override float MaxHP
	{
		get
		{
			return 300; //最大生命值为300
		}
	}


	protected override void OnInitForPlace()
	{
		InvokeRepeating("Attack", 0, attackCD);
	}


	/// <summary>
	///  如果前方有僵尸,则豌豆射手攻击
	/// </summary>
	private void Attack()
	{
		//先看有没有僵尸，如果有则生成预制体bullet
		Zombie zombie = ZombieManager.Instance.GetZombieByLineMinDistance((int)currentGrid.Point.y, transform.position);
		//如果没有僵尸,退出
		if (zombie == null) 
			return;
		//看僵尸有没有在草坪上,若没有则return
		if (zombie.CurrentGrid.Position.x >= 8 && Vector2.Distance(zombie.transform.position, zombie.CurrentGrid.Position) > 1.5f)
			return;
		//如果僵尸在我的左边,退出
		if (zombie.CurrentGrid.Position.x < gameObject.transform.position.x)
			return;
		//生成子弹
		Bullet bullet = Instantiate<GameObject>(GameManager.Instance.GameConf.Bullet1, transform.position + creatBulletOffsetPosition, Quaternion.identity, transform).GetComponent<Bullet>();

	}


}
