using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ZombieState
{
    Idel, //默认状态
    Walk,
    Eat,
    Dead
}


public class Zombie : MonoBehaviour
{
	private ZombieState state;   //僵尸状态
	private Animator animator;
    private Grid currentGrid; //当前网格
    private float speed=6;
    private bool isEating; //在攻击状态中
    private Vector3 offSetVector=new Vector3(0,1); //位置调整向量
    private float aggressivity=100; //攻击力
    private string walkingAnimitionName; //行走动画的名称
    private string attackingAnimitionName; //攻击动画的名称
    private float hp=185;
    private bool lostHead=false ;// 初始时头还没掉
    private bool lostBody = false;//初始时身子还在
    private SpriteRenderer spriteRenderer; 


    /// <summary>
    /// 修改状态会直接改变动画
    /// </summary>
	public ZombieState State { get => state;
        set
        {
            state = value;
            StateToAnimition();   
        }
    }

	public Grid CurrentGrid { get => currentGrid;  }
	public float Hp { get => hp; set
        {
            hp = value;
            if (hp <= 90 &&lostHead==false)//如果血量小于90并且没有失去头
            {
				//头掉(只执行一次),需要一个bool来判定头已经掉了没
				lostHead = true;

				walkingAnimitionName = "Zombie_LostHead"; //切换动画
                attackingAnimitionName = "Zombie_LostHeadAttack";

                //创建一个头
               GameObject.Instantiate<GameObject>(GameManager.Instance.GameConf.Zombie_head, animator.transform.position+new Vector3(0.5f,0,0), Quaternion.identity, null);
                
                //状态转换为动画
                StateToAnimition();
				
			}
			if (hp <= 0 && lostBody == false)//僵尸死亡
			{
				State = ZombieState.Dead;
				lostBody = true;
				Instantiate<GameObject>(GameManager.Instance.GameConf.Zombie_DeadBody, animator.transform.position + new Vector3(0.5f, 0, 0), Quaternion.identity, null);
			}
		}
    
    }

	// Update is called once per frame
	void Update()
	{
		StateToLogic(state);
	}



	/// <summary>
	/// 图层顺序排序(保证僵尸不会互相遮挡)
	/// </summary>
	/// <param name="orderNum"></param> 同行的顺序数
	private void LayerOrderSort(int orderNum)
    {
        int line = (int)CurrentGrid.Point.y;
        // spriteRenderer.sortingOrder=
        int startNum=400-line*100 ;//起始数字,第0行最大为400,第4行最小为0
        spriteRenderer.sortingOrder = startNum + orderNum;
    }

	/// <summary>
	/// 僵尸初始化
	/// </summary>
	/// <param name="lineNum"></param>生成的行数
	/// <param name="orderNum"></param>同行僵尸的顺序数
	public void Init(int lineNum,int orderNum)
    {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

		FindComponents();
        InitAnimationName();
		GetGridByVerticalNum(lineNum); //网格的初始化在awake里,所以这个方法放在start
        LayerOrderSort(orderNum);
	}

    /// <summary>
    /// 用来查找和初始化各种组件
    /// </summary>
	private void FindComponents()
	{
		animator = GetComponentInChildren<Animator>();

	}

	/// <summary>
	/// 用来初始化动画名称
	/// </summary>
	private void InitAnimationName()
    {
        int randomlyWalk = Random.Range(1, 4); //有1，2，3 三种可能
        switch (randomlyWalk)
        {
            case 1:
                walkingAnimitionName = "Zombie_walk 1";
				break;
            case 2:
				walkingAnimitionName = "Zombie_walk 2";
				break;
            case 3:
				walkingAnimitionName = "Zombie_walk 3";
				break;
		}
        attackingAnimitionName = "Zombie_Attack";
	}

  
    /// <summary>
    /// 检测状态,适配动画
    /// </summary>
    private void StateToAnimition()
    {
		switch (state)
		{
			case ZombieState.Idel:
				animator.Play(walkingAnimitionName, 0, 0);
				animator.speed = 0;
				break;
			case ZombieState.Walk:
				animator.Play(walkingAnimitionName,0,animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
				animator.speed = 1;
				break;
			case ZombieState.Eat:
				animator.Play(attackingAnimitionName);
				animator.Play(attackingAnimitionName, 0, animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
				animator.speed = 1;
				break;
			case ZombieState.Dead:
				break;
		}
	}
    

    /// <summary>
    /// 检测状态,适配逻辑
    /// </summary>
    private void StateToLogic(ZombieState state)
    {
		switch (state)
		{
			case ZombieState.Idel: //默认走路(没啥用状态)
                State = ZombieState.Walk;
				break;
			case ZombieState.Walk:
                Move();
				break;
			case ZombieState.Eat:
                if (isEating) break; //只让他进入一次Eat函数
                Eat(currentGrid.CurrentPlantBase);
				break;
			case ZombieState.Dead:
                Dead();
				break;
		}
	}
    

    /// <summary>
    /// 僵尸移动,一直向左走,遇见植物会攻击
    /// </summary>
    private void Move()
    {
        //如果当前网格为空,则返回;
        if (currentGrid == null)  return;
        
        currentGrid = GridManager.Instance.GetGridByWorldPosition(transform.position - offSetVector); 
	
        //如果当前网格有植物,并且距离植物很近
        if (currentGrid.IsPlanted && currentGrid.CurrentPlantBase.transform.position.x < transform.position.x && transform.position.x - currentGrid.CurrentPlantBase.transform.position.x < 0.3f)
        {
            //切换为吃的状态
            State = ZombieState.Eat;
            return;
        }

        //如果超过最左边的网格
        else if (currentGrid.Point.x == 0 && currentGrid.Position.x - transform.position.x > 0.6f)
        {
            //走向终点——房子
            Vector2 sefPos = transform.position;
            Vector2 housePos = new Vector2(-7.2f, -1.29f);
            Vector2 direction = (housePos - sefPos).normalized*3f;
            transform.Translate(direction*Time.deltaTime/speed);
            //如果距离房子很近，则游戏结束
            if (Vector2.Distance(housePos, sefPos) < 0.05f)
            {
                //游戏结束
                LevelManager.Instance.GameOver();
            }

            return;
        }

		transform.Translate(new Vector2(-1.33f, 0) * Time.deltaTime / speed);
	}


    private void Eat(PlantBase plant)
    {
        isEating = true;
        StartCoroutine(DoHurtToPlant(plant));
    }


    /// <summary>
    /// 获取一个网格,决定僵尸在第几排出现
    /// </summary>
    /// <param name="ver"></param>
    private void GetGridByVerticalNum(int ver)
    {
        //思路:获取网格->对齐y轴
        currentGrid=GridManager.Instance.GetGridByVerticalNum(ver);
        transform.position = new Vector3(transform.position.x, currentGrid.Position.y+1);//对齐y轴
    }

    /// <summary>
    /// 伤害植物
    /// </summary>
    /// <param name="plant"></param>
    /// <returns></returns>
    IEnumerator DoHurtToPlant(PlantBase plant) 
    {
        float interval=0.2f;
         while(plant.HP>0) 
        {
            plant.Hurt(aggressivity*interval); //每0.2s的伤害
            yield return new WaitForSeconds(interval);
		}
        //攻击结束,切换回走路状态
        isEating=false;
        State = ZombieState.Walk;
    }


    /// <summary>
    /// 自身受伤
    /// </summary>
    public void Hurt(float attackForce)
    {
        Hp -= attackForce;
    }

    /// <summary>
    /// 死亡
    /// </summary>
    public void Dead()
    {
        ZombieManager.Instance.RemoveZombie(this); //移除
        Destroy(gameObject);
    }
}
