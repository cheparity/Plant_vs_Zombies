using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	private Rigidbody2D rigid;
	private SpriteRenderer spriteRender; //后面击碎效果的图片
	private bool isHit = false;//是否击中僵尸
	private float attackForce = 20f;

	#region unity region
	// Start is called before the first frame update
	void Start()
	{
		//初始化
		rigid = GetComponent<Rigidbody2D>(); //获得刚体组件
		rigid.AddForce(Vector2.right * 300);
		spriteRender = GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update()
	{
		if (isHit) return; //如果击中就不旋转了
		transform.Rotate(new Vector3(0, 0, -15));
	}

	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (isHit) return; //如果击中就不旋转了
		if (collision.tag == "Zombie") //如果击中了僵尸
		{
			isHit = true;
			//僵尸受伤
			Zombie zombie = collision.gameObject.GetComponent<Zombie>(); //获取僵尸的脚本
			zombie.Hurt(attackForce);


			//修改图片->速度变为0,重力变为1,往下落->一段时间后摧毁自身
			spriteRender.sprite = GameManager.Instance.GameConf.BulletHit;
			rigid.velocity = Vector2.zero;
			rigid.gravityScale = 1;
			Invoke("Destroy", 0.8f);
		}
	}
	#endregion
	
	
	
	private void Destroy()
	{
		Destroy(gameObject);
	}

}
