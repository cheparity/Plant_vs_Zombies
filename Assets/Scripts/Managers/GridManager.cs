using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private List<Vector2> pointList = new List<Vector2>();
    private List<Grid> gridsList = new List<Grid>();
    public static GridManager Instance;

    private void Awake()
    {
        Instance = this;
		CreatGridsList();
	}
    // Start is called before the first frame update
    void Start()
    {
        //CreatGridBaseCollider();
        //CreatGridsBasePointList(); //生成网格List

        //先生成网格Grid信息（包括是否种上植物），然后批量生成Grids
        
    }

    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //Debug.Log(GetGridPosByMouse());
    }
    /// <summary>
    /// 基于碰撞的形式，批量生成碰撞网格
    /// they are test code below
    /// </summary>
    private void CreatGridBaseCollider()
    {
        //思路：创建一个预制体网格，设置其collider组件
        //左下角为原点，i为x轴，j为y轴
        //偏移量：水平为：1.35          竖直为：1.63
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                {
                    GameObject grid_prefab = new GameObject();//创建一个预制体网格
                    grid_prefab.AddComponent<BoxCollider2D>().size = new Vector2(1, 1.5f);//添加BoxCollider component,and adjust its size
                    grid_prefab.transform.SetParent(transform); //set parent
                    grid_prefab.transform.position = gameObject.transform.position + new Vector3(1.35f * i, 1.63f * j, 0);
                    grid_prefab.name = i + "-" + j;
                }

            }
        }
    }
    /// <summary>
    /// 基于坐标形式，生成网格列表
    /// </summary>
    private void CreatGridsBasePointList()
    {
        //左下角为原点，i为x轴，j为y轴
        //偏移量：水平为：1.35          竖直为：1.63
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                {
                    pointList.Add(gameObject.transform.position + new Vector3(1.35f * i, 1.63f * j, 0));
                }

            }
        }
    }

    /// <summary>
    /// 基于坐标形式，生成网格列表
    /// </summary>
    private void CreatGridsList()
    {
        //左下角为原点，i为x轴，j为y轴
        //偏移量：水平为：1.35          竖直为：1.63
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                {
                    //pointList.Add(gameObject.transform.position + new Vector3(1.35f * i, 1.63f * j, 0));
                    //把new Grid加入list中
                    gridsList.Add(new Grid(new Vector2(i, j), gameObject.transform.position + new Vector3(1.35f * i, 1.63f * j, 0), false));
                }

            }
        }
    }

    /// <summary>
    /// 通过鼠标获取网格坐标点
    /// </summary>
    public Vector2 GetGridPosByMouse()
    {
        return GetGridPointByWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    /// <summary>
    /// 通过世界坐标获取网格坐标点
    /// </summary>
    public Vector2 GetGridPointByWorldPosition(Vector2 worldPos)
    {
        //思路：点击鼠标获取点击位置；遍历所有坐标点，寻找最近坐标；
        float min_dis = 100000;//设置一个很大的数字
        int pointNum = new int();//最小距离的pointlist
        for (int i = 0; i < gridsList.Count; i++)
        {
            float tmp_dis = Vector2.Distance(worldPos, gridsList[i].Position);
            if (tmp_dis < min_dis) //如果比距离比当前最小值要小
            {
                //更新最小值
                min_dis = tmp_dis;
                pointNum = i;
            }
        }
        return gridsList[pointNum].Position;
    }

    /// <summary>
    /// 基于世界坐标返回网格类
    /// </summary>
    /// <param name="worldPos"></param>
    /// <returns></returns>
    public Grid GetGridByWorldPosition(Vector2 worldPos)
    {
		//思路：点击鼠标获取点击位置；遍历所有坐标点，寻找最近坐标；
		float min_dis = 100000;//设置一个很大的数字
		int pointNum = new int();//最小距离的pointlist
		for (int i = 0; i < gridsList.Count; i++)
		{
			float tmp_dis = Vector2.Distance(worldPos, gridsList[i].Position);
			if (tmp_dis < min_dis) //如果比距离比当前最小值要小
			{
				//更新最小值
				min_dis = tmp_dis;
				pointNum = i;
			}
		}
        return gridsList[pointNum];
	}

    /// <summary>
    /// 通过数轴(y轴)来获取第一个网格，从下往上从0开始
    /// </summary>
    /// <param name="verticalNum"></param>
    /// <returns></returns>
    public Grid GetGridByVerticalNum(int verticalNum)
    {
        for(int i = 0; i < gridsList.Count; i++)
        {
            if (gridsList[i].Point==new Vector2(8, verticalNum)) //如果是(8, verticalNum)
            {
                return gridsList[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 通过鼠标来获取网格
    /// </summary>
    /// <returns></returns>
    public Grid GetGridByMouse()
    {
        return GetGridByWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
}
