using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 网格信息，不用继承Monobehaviour
/// </summary>
public class Grid
{
    /// <summary>
    /// 坐标点，如（0，1）
    /// </summary>
    public Vector2 Point;
    /// <summary>
    /// 世界坐标
    /// </summary>
    public Vector2 Position;
    /// <summary>
    /// 是否有植物
    /// </summary>
    public bool IsPlanted;

    private PlantBase currentPlantBase;
	public PlantBase CurrentPlantBase { get => currentPlantBase; 
        set
        {
            currentPlantBase = value;
            if (currentPlantBase == null) //如果没有植物
            {
                IsPlanted = false;
            }
            else
            {
                IsPlanted = true;
            }

        }
    }

	///构造函数
	public Grid(Vector2 point, Vector2 position, bool isPlanted)
    {
        Point = point;
        Position = position;
        IsPlanted = isPlanted;
    }

	
}
