using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 卡片的状态
/// </summary>
public enum CardState
{
	CanPlace,
	NoCD,
	NoSun,
	NoBoth
};

public class UIPlantCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	private Image maskImg;//用于masking的img组件
	private Image image; //自身的img组件
	public float CDTime;//CD时间
	private float currentTimeForCD;//cd时间的计算
	private bool cdAllows; //是否可以放置植物
	private bool wantPlace;//是否需要放置植物
	private PlantBase plant; //用来创建的植物                                                                                                                                                                                                                                          
	private PlantBase plantInGrid;//网格中的植物，是透明的
	public PlantTpye cardPlantType;//当前卡片植物类型
	private CardState cardState = CardState.NoBoth;//卡片状态

	public int sunConsumptionNum;//需要多少阳光  
	private Text sunConsumptionText;//阳光消耗数量的文本




	/// <summary>
	/// CD(注意是CD)是否允许放置植物
	/// </summary>
	public bool CdAllows
	{
		get => cdAllows;
		set
		{
			cdAllows = value;
			CheckState();
		}
	}
	/// <summary>
	/// 是否想要放置植物
	/// </summary>
	public bool WantPlace
	{
		get => wantPlace;
		set
		{
			wantPlace = value;
			if (wantPlace) //如果想要放置植物，则从植物管理器拿到类型并实例化
			{
				GameObject prefab = PlantManager.Instance.GetPlantForType(cardPlantType); //通过卡片类型拿到预制体
				//Debug.Log(prefab);
				plant = GameObject.Instantiate<GameObject>(prefab, Vector3.zero, Quaternion.identity, PlantManager.Instance.transform).GetComponent<PlantBase>();//实例化
																																								 //Debug.Log(plant);
				plant.InitForCreate(false);
			}
			else//如果不想放置植物，则销毁植物，此时plant为空
			{
				if (plant != null)
				{
					Destroy(plant.gameObject);//销毁植物
					plant = null;
				}

			}
		}
	}

	public CardState CardState
	{
		get => cardState;
		set
		{
			//如果要修改的值和当前值一样
			if (cardState == value)
			{
				return;
			}
			switch (value)
			{
				case CardState.CanPlace:
					maskImg.fillAmount = 0;
					image.color = Color.white;
					break;
				case CardState.NoCD:
					image.color = Color.white;
					if (CardState == CardState.NoBoth) return;
					EnterCD(); //如果没有cd，进入cd
					break;
				case CardState.NoSun:
					maskImg.fillAmount = 0;
					image.color = new Color(0.75f, 0.75f, 0.75f);
					break;
				case CardState.NoBoth:
					image.color = Color.white;
					if (CardState == CardState.NoCD) return;
					EnterCD();
					break;
			}
			cardState = value;
		}
	}

	/// <summary>
	/// 状态检测
	/// </summary>
	private void CheckState()
	{
		if (cdAllows && PlayerManager.Instance.SunNum >= sunConsumptionNum)
		{
			CardState = CardState.CanPlace;
		}
		else if (!cdAllows && PlayerManager.Instance.SunNum >= sunConsumptionNum)
		{
			CardState = CardState.NoCD;
		}
		else if (!cdAllows && PlayerManager.Instance.SunNum < sunConsumptionNum)
		{
			CardState = CardState.NoBoth;
		}
		else if (cdAllows && PlayerManager.Instance.SunNum < sunConsumptionNum)
		{
			CardState = CardState.NoSun;
		}
	}


	#region 鼠标操作逻辑

	/// <summary>
	/// 鼠标移入,整体放大
	/// </summary>
	/// <param name="eventData"></param>
	/// <exception cref="System.NotImplementedException"></exception>
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (cardState!=CardState.CanPlace) return;
		transform.localScale = transform.localScale + new Vector3(0.05f, 0.05f, 0);
	}


	/// <summary>
	/// 鼠标移除时,回到原来大小
	/// </summary>
	/// <param name="eventData"></param>
	/// <exception cref="System.NotImplementedException"></exception>
	public void OnPointerExit(PointerEventData eventData)
	{
		if (cardState != CardState.CanPlace) return;
		transform.localScale = new Vector3(1.098438f, 1, 1);
	}


	/// <summary>
	/// 鼠标点击
	/// </summary>
	/// <param name="eventData"></param>
	/// <exception cref="System.NotImplementedException"></exception>
	public void OnPointerClick(PointerEventData eventData)
	{
		if (cardState != CardState.CanPlace) return;
		if (!wantPlace) //如果能放置但不想放置，则
		{
			WantPlace = true;
		}
	}


	#endregion


	#region start和update

	private void Start()
	{
		image = GetComponent<Image>();
		maskImg = transform.Find("Mask").GetComponent<Image>();
		sunConsumptionText = transform.Find("SunNumber").GetComponent<Text>();
		sunConsumptionText.text = sunConsumptionNum.ToString();
		CdAllows = true; //初始的时候是没有cd的
		PlayerManager.Instance.AddSunNumUpdateActionListener(CheckState);
	}
	private void Update()
	{
		if (cardState != CardState.CanPlace) return;
		//如果需要放置植物，并且要放置的植物不为空
		if (wantPlace && plant != null)
		{
			//让植物跟随我们的鼠标
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //鼠标位置
			Grid grid = GridManager.Instance.GetGridByWorldPosition(mousePos); //网格位置

			plant.transform.position = new Vector3(mousePos.x, mousePos.y, 0); //让植物跟随我们的鼠标


			//如果距离网格比较近，需要在网格上出现一个透明的植物
			Vector2 GridPosition = GridManager.Instance.GetGridPosByMouse();


			//如果没有种植物并且离网格比较近
			if (grid.IsPlanted == false && Vector2.Distance(mousePos, GridPosition) < 1.5f)
			{
				//实例化透明植物(plantInGrid),直接用plant就行
				if (plantInGrid == null)//如果网格中的植物没有实例化,则实例化
				{
					plantInGrid = GameObject.Instantiate<GameObject>(plant.gameObject, GridPosition, Quaternion.identity, PlantManager.Instance.transform).GetComponent<PlantBase>();
					plantInGrid.InitForCreate(true); //实例化网格中的太阳花
				}
				else //如果实例化了,就让plantInGrid的坐标一直为离他最近的网格的坐标
				{
					plantInGrid.transform.position = GridManager.Instance.GetGridPosByMouse();
				}

				//如果点击鼠标左键,则放置植物
				if (Input.GetMouseButtonDown(0))
				{
					plant.InitForPlace(grid);//放置初始化
					plant = null;
					Destroy(plantInGrid.gameObject); //销毁网格植物
					plantInGrid = null;
					WantPlace = false;//不需要创造了,因为已经创造好了
					CdAllows = false; //不能创建植物
					PlayerManager.Instance.SunNum -= sunConsumptionNum;
				}
			}
			else
			{
				if (plantInGrid != null)
				{
					Destroy(plantInGrid.gameObject);
					plantInGrid = null;
				}
			}
		}
		if (Input.GetMouseButtonDown(1)) //如果点击鼠标右键,则取消
		{
			if (plant != null)
			{
				Destroy(plant.gameObject);
				plant = null;
			}
			if (plantInGrid != null)
			{
				Destroy(plantInGrid.gameObject);
				plantInGrid = null;
			}
			WantPlace = false;
		}
	}

	#endregion




	/// <summary>
	/// 进入cd
	/// </summary>
	private void EnterCD()
	{
		maskImg.fillAmount = 1;
		//遮罩后开始计算冷却(用协程的方式)
		StartCoroutine(CalculateCDTime()); // 停顿,并减少
	}

	/// <summary>
	/// 计算cd时间
	/// </summary>
	/// <returns></returns>
	IEnumerator CalculateCDTime()
	{
		float calCD = (1 / CDTime) * 0.1f;//每0.1s减多少CDtime
		currentTimeForCD = CDTime;
		while (currentTimeForCD >= 0)
		{
			yield return new WaitForSeconds(0.1f); //每0.1s运行一次
			maskImg.fillAmount -= calCD; //每次减calCD值
			currentTimeForCD -= 0.1f; //每0.1s减0.1,意味着每秒减1
		}
		//冷却结束,可以放置
		maskImg.fillAmount = 0; //重置保险
		CdAllows = true; //可以种植了
	}

}
