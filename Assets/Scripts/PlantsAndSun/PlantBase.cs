using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 植物基类
/// </summary>
public abstract class PlantBase : MonoBehaviour {
	protected Animator animator; //private子类不能调用,所以用protected
	protected SpriteRenderer spriteRenderer;
	/// <summary>
	/// 当前植物所在的网格
	/// </summary>
	protected Grid currentGrid;
	protected float hp; //生命值

	public float HP { get => hp; }
	public abstract float MaxHP { get;  }

	/// <summary>
	/// 查找组件
	/// </summary>
	protected void FindConponents() {
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	/// <summary>
	/// 创建时(放置之前)的初始化
	/// </summary>
	public void InitForCreate(bool inGrid) {
		hp = MaxHP;
		FindConponents();
		//动画调成0
		animator.speed = 0; //停止动画
		if (inGrid) //如果是网格中的植物
		{
			spriteRenderer.sortingOrder = -1;
			spriteRenderer.color = new Color(1, 1, 1, 0.6f);
		}
		else {
			spriteRenderer.sortingOrder = 1;
		}
	}

	/// <summary>
	/// 放置之后的初始化
	/// </summary>
	public void InitForPlace(Grid grid) {
		currentGrid = grid;
		currentGrid.CurrentPlantBase = this; //让当前网格的种植状态为自身
		transform.position = grid.Position;
		animator.speed = 1;
		spriteRenderer.sortingOrder = 0;//将sortingOrder调整成0
		OnInitForPlace();
	}

	/// <summary>
	/// (需要重写)InitForPlace()的补充方法
	/// </summary>
	protected virtual void OnInitForPlace() { }

	/// <summary>
	/// 受到攻击的方法
	/// </summary>
	/// <param name="hurtValue"></param>
	public void Hurt(float hurtValue) {
		hp -= hurtValue;
		if (hp <= 0) {
			Dead();//死亡
		}
	}

	public void Dead() {
		if (currentGrid != null)
		{
			currentGrid.CurrentPlantBase = null;
			currentGrid = null;
		}
		Destroy(gameObject);
	}
}
