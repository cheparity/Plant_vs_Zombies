using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Shovel : MonoBehaviour,IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Transform shovel;
    private bool isUsing;

	public bool IsUsing { get => isUsing;
        set
        {
            isUsing = value;
            //铲植物中
            if (isUsing)
            {
                shovel.localRotation = Quaternion.Euler(0,0,45); //将铲子旋转
            }
            //把铲子放回去
            else
            {
				shovel.localRotation = Quaternion.Euler(0, 0, 0); //将铲子旋转回来
                shovel.transform.position = transform.position;
			}
		}
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsUsing == false)
            IsUsing = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        shovel.transform.localScale = new Vector2(1.4f, 1.4f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
		shovel.transform.localScale = new Vector2(1, 1);
	}

    private void Start()
    {
        shovel = transform.Find("Shovel"); //获取shovel

    }

    private void Update()
    {
        if (isUsing == true)
        {
            shovel.transform.position = Input.mousePosition; //坐标为鼠标坐标
            if (Input.GetMouseButtonDown(0))
            {
                Grid grid = GridManager.Instance.GetGridByMouse();
                if (grid.CurrentPlantBase == null) //如果没有植物,则不需要铲除
                    IsUsing = false;
                else if (Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition) ,grid.CurrentPlantBase.transform.position)<1.5f)
                {
                    grid.CurrentPlantBase.Dead(); //当前植物死亡
                    IsUsing = false;
                }
              
            }
            if (Input.GetMouseButtonDown(1))
            {
                IsUsing = false;
            }
        }
    }

}
